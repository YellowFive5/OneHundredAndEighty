#region Usings

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DirectShowLib;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Enums;
using OneHundredAndEightyCore.Windows.CamsDetection;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public enum DetectionServiceStatus
    {
        WaitingThrow,
        ProcessingThrow,
        DartsExtraction
    }

    public enum DetectionServiceWorkingMode
    {
        Crossing,
        Detection
    }

    public class DetectionService : IDetectionService
    {
        private readonly DrawService drawService;
        private readonly ConfigService configService;
        private readonly ThrowService throwService;
        private readonly Logger logger;
        private readonly CamsDetectionBoard camsDetectionBoard;

        private List<CamService> cams;
        private CancellationTokenSource cts;
        private CancellationToken cancelToken;
        private DetectionServiceWorkingMode workingMode;
        private bool detectionEnabled;

        public DetectionService(DrawService drawService,
                                ConfigService configService,
                                ThrowService throwService,
                                Logger logger,
                                CamsDetectionBoard camsDetectionBoard)
        {
            this.drawService = drawService;
            this.configService = configService;
            this.throwService = throwService;
            this.logger = logger;
            this.camsDetectionBoard = camsDetectionBoard;
        }

        public delegate void ThrowDetectedDelegate(DetectedThrow thrw);

        public event ThrowDetectedDelegate OnThrowDetected;

        public delegate void ExceptionOccurredDelegate(Exception ex);

        public event ExceptionOccurredDelegate OnErrorOccurred;

        public delegate void StatusDelegate(DetectionServiceStatus status);

        public event StatusDelegate OnStatusChanged;

        public void CheckCamsAndTryCapture(List<CamService> camsList)
        {
            foreach (var cam in camsList)
            {
                cam.TryQueryFrame();
            }
        }

        public async void RunDetection(List<CamService> camsList,
                                       DetectionServiceWorkingMode mode)
        {
            cams = camsList;
            workingMode = mode;
            cts = new CancellationTokenSource();
            cancelToken = cts.Token;

            var extractionSleepTime = configService.ExtractionSleepTimeValue;
            var thresholdSleepTime = configService.ThresholdSleepTimeValue;
            var moveDetectedSleepTime = configService.MovesDetectedSleepTimeValue;
            detectionEnabled = configService.DetectionEnabled && !App.NoCams;

            try
            {
                await Task.Run(async () =>
                               {
                                   OnStatusChanged?.Invoke(DetectionServiceStatus.WaitingThrow);

                                   cams.ForEach(c =>
                                                {
                                                    c.DoDetectionCaptures();
                                                    c.PreviousRoiFrameUpdateFromRoiFrame();
                                                    c.ThrowExtractedRoiFrameUpdateBlackBlank();
                                                });

                                   while (!cancelToken.IsCancellationRequested)
                                   {
                                       foreach (var cam in cams)
                                       {
                                           cam.DoDetectionCaptures();
                                           cam.ThrowExtractedRoiFrameExtractFromRoiPreviousFrame();

                                           var result = DetectMoves(cam);

                                           if (result == MovesDetectionResult.Nothing)
                                           {
                                               await Task.Delay(TimeSpan.FromSeconds(thresholdSleepTime));
                                               continue;
                                           }

                                           if (result == MovesDetectionResult.Extraction)
                                           {
                                               OnStatusChanged?.Invoke(DetectionServiceStatus.DartsExtraction);

                                               await Task.Delay(TimeSpan.FromSeconds(extractionSleepTime));

                                               foreach (var camToRefresh in cams)
                                               {
                                                   camToRefresh.DoDetectionCaptures();
                                                   camToRefresh.RoiFrameUpdateBlackBlank();
                                                   camToRefresh.PreviousRoiFrameUpdateBlackBlank();
                                                   camToRefresh.ThrowExtractedRoiFrameUpdateBlackBlank();
                                                   Application.Current.Dispatcher.InvokeAsync(() =>
                                                                                              {
                                                                                                  camsDetectionBoard.SetCamImages(camToRefresh.camNumber,
                                                                                                                                  camToRefresh.GetImage(),
                                                                                                                                  camToRefresh.GetRoiImage(),
                                                                                                                                  camToRefresh.GetThrowExtractedRoiFrameImage());
                                                                                              });
                                               }

                                               Application.Current.Dispatcher.InvokeAsync(() =>
                                                                                          {
                                                                                              camsDetectionBoard.ClearProjectionImage();
                                                                                              camsDetectionBoard.ClearPointsBox();
                                                                                          });

                                               OnStatusChanged?.Invoke(DetectionServiceStatus.WaitingThrow);
                                               continue;
                                           }

                                           if (result == MovesDetectionResult.Throw)
                                           {
                                               OnStatusChanged?.Invoke(DetectionServiceStatus.ProcessingThrow);

                                               await Task.Delay(TimeSpan.FromSeconds(moveDetectedSleepTime));

                                               foreach (var camWithThrow in cams)
                                               {
                                                   camWithThrow.DoDetectionCaptures();
                                                   camWithThrow.ThrowExtractedRoiFrameExtractFromRoiPreviousFrame();
                                                   FindAndProcessDartContour(camWithThrow);
                                                   camWithThrow.PreviousRoiFrameUpdateFromRoiFrame();
                                                   Application.Current.Dispatcher.InvokeAsync(() =>
                                                                                              {
                                                                                                  camsDetectionBoard.SetCamImages(camWithThrow.camNumber,
                                                                                                                                  camWithThrow.GetImage(),
                                                                                                                                  camWithThrow.GetRoiImage(),
                                                                                                                                  camWithThrow.GetThrowExtractedRoiFrameImage());
                                                                                              });
                                               }

                                               var thrw = throwService.GetThrow();
                                               InvokeOnThrowDetected(thrw);
                                               OnStatusChanged?.Invoke(DetectionServiceStatus.WaitingThrow);
                                           }
                                       }
                                   }
                               });
            }
            catch (Exception e)
            {
                OnErrorOccurred?.Invoke(e);
            }
            finally
            {
                cts?.Cancel();
                throwService.ClearRays();
                cams.ForEach(c => { c.Dispose(); });
            }
        }

        public void StopDetection()
        {
            cts?.Cancel();
        }

        #region Internal detection mechanism

        private MovesDetectionResult DetectMoves(CamService cam)
        {
            var maxDartContourArc = configService.MaxContourArcValue * 1.5;
            var maxDartContourArea = configService.MaxContourAreaValue * 1.5;
            var maxDartContourWidth = configService.MaxContourWidthValue * 1.5;

            var minDartContourArc = configService.MinContourArcValue;
            var minDartContourArea = configService.MinContourAreaValue;
            var minDartContourWidth = configService.MinContourWidthValue;

            var allContours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(cam.ThrowExtractedRoiFrame,
                                  allContours,
                                  new Mat(),
                                  RetrType.External,
                                  ChainApproxMethod.ChainApproxNone);

            if (!detectionEnabled || allContours.Size == 0)
            {
                return MovesDetectionResult.Nothing;
            }

            var contourWithMaxArc = new VectorOfPoint();
            var contourWithMaxArea = new VectorOfPoint();
            var contourWithMaxWidth = new VectorOfPoint();

            var maxArс = 0.0;
            var maxArea = 0.0;
            var maxWidth = 0.0;

            for (var i = 0; i < allContours.Size; i++)
            {
                var tempContour = allContours[i];

                var tempContourArс = CvInvoke.ArcLength(tempContour, true);
                if (tempContourArс > maxArс)
                {
                    maxArс = tempContourArс;
                    contourWithMaxArc = tempContour;
                }

                var tempContourArea = CvInvoke.ContourArea(tempContour);
                if (tempContourArea > maxArea)
                {
                    maxArea = tempContourArea;
                    contourWithMaxArea = tempContour;
                }

                var rect = CvInvoke.MinAreaRect(tempContour);
                var box = CvInvoke.BoxPoints(rect);
                var contourBoxPoint1 = new PointF(box[0].X, (float) cam.roiPosYSlider + box[0].Y);
                var contourBoxPoint2 = new PointF(box[1].X, (float) cam.roiPosYSlider + box[1].Y);
                var contourBoxPoint4 = new PointF(box[3].X, (float) cam.roiPosYSlider + box[3].Y);
                var side1 = MeasureService.FindDistance(contourBoxPoint1, contourBoxPoint2);
                var side2 = MeasureService.FindDistance(contourBoxPoint4, contourBoxPoint1);
                var tempContourWidth = side1 < side2
                                           ? side1
                                           : side2;
                if (tempContourWidth > maxWidth)
                {
                    maxWidth = tempContourWidth;
                    contourWithMaxWidth = tempContour;
                }
            }

            if (workingMode == DetectionServiceWorkingMode.Crossing ||
                maxArс >= minDartContourArc &&
                maxArс <= maxDartContourArc &&
                maxArea >= minDartContourArea &&
                maxArea <= maxDartContourArea &&
                maxWidth >= minDartContourWidth &&
                maxWidth <= maxDartContourWidth &&
                contourWithMaxArc.Equals(contourWithMaxArea) &&
                contourWithMaxArea.Equals(contourWithMaxWidth) &&
                contourWithMaxWidth.Equals(contourWithMaxArc)
            )
            {
                return MovesDetectionResult.Throw;
            }

            if (maxArс > maxDartContourArc ||
                maxArea > maxDartContourArea ||
                maxWidth > maxDartContourWidth)
            {
                return MovesDetectionResult.Extraction;
            }

            return MovesDetectionResult.Nothing;
        }

        private void FindAndProcessDartContour(CamService cam)
        {
            var dartContour = TryFindDartContour(cam);
            if (dartContour != null)
            {
                var toDartRay = ProcessDartContour(cam, dartContour);
                throwService.SaveRay(toDartRay);
                Application.Current.Dispatcher.Invoke(() => { camsDetectionBoard.DrawRay(toDartRay); });
            }
        }

        private DartContour TryFindDartContour(CamService cam)
        {
            var frame = workingMode == DetectionServiceWorkingMode.Detection
                            ? cam.ThrowExtractedRoiFrame
                            : cam.RoiFrame;

            var allContours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(frame,
                                  allContours,
                                  new Mat(),
                                  RetrType.External,
                                  ChainApproxMethod.ChainApproxNone);

            var contour = new VectorOfPoint();
            var contourArea = 0.0;

            for (var i = 0; i < allContours.Size; i++)
            {
                var tempContour = allContours[i];
                var tempContourArea = CvInvoke.ContourArea(tempContour);
                if (tempContourArea > contourArea)
                {
                    contourArea = tempContourArea;
                    contour = tempContour;
                }
            }

            DartContour dartContour = null;
            if (contourArea > 0)
            {
                dartContour = new DartContour(contour, contourArea);
            }

            return dartContour;
        }

        private Ray ProcessDartContour(CamService cam, DartContour dartContour)
        {
            // Moments and centerpoint
            // var contourMoments = CvInvoke.Moments(processedContour);
            // var contourCenterPoint = new PointF((float) (contourMoments.M10 / contourMoments.M00),
            //                                     (float) camService.roiPosYSlider + (float) (contourMoments.M01 / contourMoments.M00));

            // Find contour rectangle
            var rect = CvInvoke.MinAreaRect(dartContour.ContourPoints);
            var box = CvInvoke.BoxPoints(rect);

            var contourBoxPoint1 = new PointF(box[0].X, (float) cam.roiPosYSlider + box[0].Y);
            var contourBoxPoint2 = new PointF(box[1].X, (float) cam.roiPosYSlider + box[1].Y);
            var contourBoxPoint3 = new PointF(box[2].X, (float) cam.roiPosYSlider + box[2].Y);
            var contourBoxPoint4 = new PointF(box[3].X, (float) cam.roiPosYSlider + box[3].Y);

            // Setup vertical contour middlepoints
            var contourHeight = MeasureService.FindDistance(contourBoxPoint1, contourBoxPoint2);
            var contourWidth = MeasureService.FindDistance(contourBoxPoint4, contourBoxPoint1);
            PointF contourBoxMiddlePoint1;
            PointF contourBoxMiddlePoint2;

            if (contourWidth > contourHeight)
            {
                contourBoxMiddlePoint1 = MeasureService.FindMiddle(contourBoxPoint1, contourBoxPoint2);
                contourBoxMiddlePoint2 = MeasureService.FindMiddle(contourBoxPoint4, contourBoxPoint3);
            }
            else
            {
                contourBoxMiddlePoint1 = MeasureService.FindMiddle(contourBoxPoint4, contourBoxPoint1);
                contourBoxMiddlePoint2 = MeasureService.FindMiddle(contourBoxPoint3, contourBoxPoint2);
            }

            // Find spikeLine to surface
            var spikeLinePoint1 = contourBoxMiddlePoint1;
            var spikeLinePoint2 = contourBoxMiddlePoint2;
            var spikeLineLength = cam.surfaceSlider - contourBoxMiddlePoint2.Y;
            var spikeAngle = MeasureService.FindAngle(contourBoxMiddlePoint2, contourBoxMiddlePoint1);
            spikeLinePoint1.X = (float) (contourBoxMiddlePoint2.X + Math.Cos(spikeAngle) * spikeLineLength);
            spikeLinePoint1.Y = (float) (contourBoxMiddlePoint2.Y + Math.Sin(spikeAngle) * spikeLineLength);

            // Find point of impact with surface
            PointF? camPoi = MeasureService.FindLinesIntersection(spikeLinePoint1,
                                                                  spikeLinePoint2,
                                                                  new PointF(0,
                                                                             (float) cam.surfaceSlider),
                                                                  new PointF(cam.resolutionWidth,
                                                                             (float) cam.surfaceSlider));

            // Translate cam surface POI to dartboard projection
            var frameSemiWidth = cam.resolutionWidth / 2;
            var camFovSemiAngle = cam.camFovAngle / 2;
            var projectionToCenter = new PointF();
            var surfacePoiToCenterDistance = MeasureService.FindDistance(new PointF((float) cam.surfaceCenterSlider,
                                                                                    (float) cam.surfaceSlider),
                                                                         camPoi.GetValueOrDefault());
            var surfaceLeftToPoiDistance = MeasureService.FindDistance(new PointF((float) cam.surfaceCenterSlider - cam.resolutionWidth / 3,
                                                                                  (float) cam.surfaceSlider),
                                                                       camPoi.GetValueOrDefault());
            var surfaceRightToPoiDistance = MeasureService.FindDistance(new PointF((float) cam.surfaceCenterSlider + cam.resolutionWidth / 3,
                                                                                   (float) cam.surfaceSlider),
                                                                        camPoi.GetValueOrDefault());
            var projectionCamToCenterDistance = frameSemiWidth / Math.Sin(Math.PI * camFovSemiAngle / 180.0) * Math.Cos(Math.PI * camFovSemiAngle / 180.0);
            var projectionCamToPoiDistance = Math.Sqrt(Math.Pow(projectionCamToCenterDistance, 2) + Math.Pow(surfacePoiToCenterDistance, 2));
            var projectionPoiToCenterDistance = Math.Sqrt(Math.Pow(projectionCamToPoiDistance, 2) - Math.Pow(projectionCamToCenterDistance, 2));
            var poiCamCenterAngle = Math.Asin(projectionPoiToCenterDistance / projectionCamToPoiDistance);

            projectionToCenter.X = (float) (cam.camSetupPoint.X - Math.Cos(cam.toBullAngle) * projectionCamToCenterDistance);
            projectionToCenter.Y = (float) (cam.camSetupPoint.Y - Math.Sin(cam.toBullAngle) * projectionCamToCenterDistance);

            if (surfaceLeftToPoiDistance < surfaceRightToPoiDistance)
            {
                poiCamCenterAngle *= -1;
            }

            var projectionPoi = new PointF((float) (cam.camSetupPoint.X + Math.Cos(cam.toBullAngle + poiCamCenterAngle) * 2000),
                                           (float) (cam.camSetupPoint.Y + Math.Sin(cam.toBullAngle + poiCamCenterAngle) * 2000));

            // Draw line from cam through projection POI
            var rayPoint = projectionPoi;
            var angle = MeasureService.FindAngle(cam.camSetupPoint, rayPoint);
            rayPoint.X = (float) (cam.camSetupPoint.X + Math.Cos(angle) * 2000);
            rayPoint.Y = (float) (cam.camSetupPoint.Y + Math.Sin(angle) * 2000);

            return new Ray(cam.camNumber, cam.camSetupPoint, rayPoint, dartContour.Area);
        }

        #endregion

        public void InvokeOnThrowDetected(DetectedThrow thrw, bool manualThrow = false)
        {
            if (thrw != null)
            {
                OnThrowDetected?.Invoke(thrw);
                
                Application.Current.Dispatcher.Invoke(() =>
                                                      {
                                                          if (workingMode == DetectionServiceWorkingMode.Crossing)
                                                          {
                                                              camsDetectionBoard.DrawThrow(thrw);
                                                              return;
                                                          }

                                                          if (!manualThrow)
                                                          {
                                                              camsDetectionBoard.PrintThrow(thrw);
                                                              camsDetectionBoard.DrawThrow(thrw);
                                                          }
                                                      });
            }
        }

        public string FindConnectedCams()
        {
            var allCams = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice).ToList();
            var str = new StringBuilder();
            for (var i = 0; i < allCams.Count; i++)
            {
                var cam = allCams[i];
                var camId = cam.DevicePath.Substring(44, 10);
                str.AppendLine($"[{cam.Name}]-[ID:'{camId}']");
            }

            return allCams.Count == 0
                       ? "No cameras found"
                       : str.ToString();
        }

        public string FindContourOnRoiFrame(CamService cam)
        {
            var allContours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(cam.RoiFrame,
                                  allContours,
                                  new Mat(),
                                  RetrType.External,
                                  ChainApproxMethod.ChainApproxNone);

            var contour = new VectorOfPoint();
            var contourArс = 0.0;
            var contourArea = 0.0;
            var contourWidth = 0.0;

            for (var i = 0; i < allContours.Size; i++)
            {
                var tempContour = allContours[i];
                var tempContourArс = CvInvoke.ArcLength(tempContour, true);
                if (tempContourArс > contourArс)
                {
                    contour = tempContour;
                    contourArс = tempContourArс;
                }
            }

            if (contourArс > 0)
            {
                contourArea = CvInvoke.ContourArea(contour);
                var rect = CvInvoke.MinAreaRect(contour);
                var box = CvInvoke.BoxPoints(rect);
                var contourBoxPoint1 = new PointF(box[0].X, (float) cam.roiPosYSlider + box[0].Y);
                var contourBoxPoint2 = new PointF(box[1].X, (float) cam.roiPosYSlider + box[1].Y);
                var contourBoxPoint4 = new PointF(box[3].X, (float) cam.roiPosYSlider + box[3].Y);
                var side1 = MeasureService.FindDistance(contourBoxPoint1, contourBoxPoint2);
                var side2 = MeasureService.FindDistance(contourBoxPoint4, contourBoxPoint1);

                contourWidth = side1 < side2
                                   ? side1
                                   : side2;
            }

            return Converter.ToString($"Contour Arc:{(int) contourArс}\tArea:{(int) contourArea}\tWidth:{(int) contourWidth}");
        }
    }
}