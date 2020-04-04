#region Usings

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV.Structure;
using NLog;
using OneHundredAndEightyCore.Game;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public class ThrowService
    {
        private readonly DrawService drawService;
        private readonly Logger logger;
        private readonly List<Ray> rays;

        public ThrowService(DrawService drawService, Logger logger)
        {
            this.logger = logger;
            this.drawService = drawService;
            rays = new List<Ray>();
        }

        public DetectedThrow GetThrow()
        {
            logger.Debug($"Calculate throw start");

            if (rays.Count < 2)
            {
                logger.Debug($"Rays count < 2. Calculate throw end.");

                rays.Clear();
                return null;
            }

            rays.ForEach(r => logger.Info($"Ray:'{r}'"));

            var firstBestRay = rays.OrderByDescending(i => i.ContourArc).ElementAt(0);
            var secondBestRay = rays.OrderByDescending(i => i.ContourArc).ElementAt(1);
            rays.Clear();

            logger.Info($"Best rays:'{firstBestRay}' and '{secondBestRay}'");

            var poi = MeasureService.FindLinesIntersection(firstBestRay.CamPoint,
                                                           firstBestRay.RayPoint,
                                                           secondBestRay.CamPoint,
                                                           secondBestRay.RayPoint);
            if (float.IsNaN(poi.X) || float.IsNaN(poi.Y))
            {
                logger.Info($"Corrupted poi. Abort");
                return null;
            }

            var thrw = PrepareThrowData(poi);

            drawService.ProjectionDrawLine(firstBestRay.CamPoint, firstBestRay.RayPoint, new Bgr(Color.Aqua).MCvScalar, true);
            drawService.ProjectionDrawLine(secondBestRay.CamPoint, secondBestRay.RayPoint, new Bgr(Color.Aqua).MCvScalar, false);
            drawService.ProjectionDrawThrow(poi, false);
            drawService.PrintThrow(thrw);


            logger.Info($"Throw:{thrw}");
            logger.Debug($"Calculate throw end.");
            return thrw;
        }

        private DetectedThrow PrepareThrowData(PointF poi)
        {
            var sectors = new List<int>()
                          {
                              14, 9, 12, 5, 20,
                              1, 18, 4, 13, 6,
                              10, 15, 2, 17, 3,
                              19, 7, 16, 8, 11
                          };
            var angle = MeasureService.FindAngle(DrawService.projectionCenterPoint, poi);
            var distance = MeasureService.FindDistance(DrawService.projectionCenterPoint, poi);
            var sector = 0;
            var type = ThrowType.Single;

            if (distance >= drawService.projectionCoefficent * 95 &&
                distance <= drawService.projectionCoefficent * 105)
            {
                type = ThrowType.Tremble;
            }
            else if (distance >= drawService.projectionCoefficent * 160 &&
                     distance <= drawService.projectionCoefficent * 170)
            {
                type = ThrowType.Double;
            }

            // Find sector
            if (distance <= drawService.projectionCoefficent * 7)
            {
                sector = 50;
                type = ThrowType.Bulleye;
            }
            else if (distance > drawService.projectionCoefficent * 7 &&
                     distance <= drawService.projectionCoefficent * 17)
            {
                sector = 25;
                type = ThrowType._25;
            }
            else if (distance > drawService.projectionCoefficent * 170)
            {
                sector = 0;
                type = ThrowType.Zero;
            }
            else
            {
                var startRadSector = MeasureService.StartRadSector_1114;
                var radSector = startRadSector;
                foreach (var proceedSector in sectors)
                {
                    if (angle >= radSector && angle < radSector + MeasureService.SectorStepRad)
                    {
                        sector = proceedSector;
                        break;
                    }

                    sector = 11; // todo - works, but not looks pretty

                    radSector += MeasureService.SectorStepRad;
                }
            }

            return new DetectedThrow(poi, sector, type, drawService.projectionFrameSide);
        }

        public void SaveRay(Ray ray)
        {
            rays.Add(ray);
        }
    }
}