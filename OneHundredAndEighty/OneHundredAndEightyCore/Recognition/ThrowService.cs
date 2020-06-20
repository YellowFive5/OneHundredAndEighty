#region Usings

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public class ThrowService
    {
        private readonly Logger logger;
        private readonly List<Ray> rays;

        public ThrowService(Logger logger)
        {
            this.logger = logger;
            rays = new List<Ray>();
        }

        public void SaveRay(Ray ray)
        {
            rays.Add(ray);
        }

        public void ClearRays()
        {
            rays.Clear();
        }

        public DetectedThrow GetThrow()
        {
            if (rays.Count < 2)
            {
                rays.Clear();
                return null;
            }

            var firstBestRay = rays.OrderByDescending(i => i.ContourArc).ElementAt(0);
            var secondBestRay = rays.OrderByDescending(i => i.ContourArc).ElementAt(1);
            rays.Clear();

            var poi = MeasureService.FindLinesIntersection(firstBestRay.CamPoint,
                                                           firstBestRay.RayPoint,
                                                           secondBestRay.CamPoint,
                                                           secondBestRay.RayPoint);
            if (float.IsNaN(poi.X) || float.IsNaN(poi.Y))
            {
                return null;
            }

            var thrw = PrepareThrowData(poi);

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
            var projectionCenterPoint = new PointF((float) DrawService.ProjectionFrameSide / 2,
                                                   (float) DrawService.ProjectionFrameSide / 2);
            var angle = MeasureService.FindAngle(projectionCenterPoint, poi);
            var distance = MeasureService.FindDistance(projectionCenterPoint, poi);
            var sector = 0;
            var type = ThrowType.Single;

            if (distance >= DrawService.ProjectionCoefficient * 95 &&
                distance <= DrawService.ProjectionCoefficient * 105)
            {
                type = ThrowType.Tremble;
            }
            else if (distance >= DrawService.ProjectionCoefficient * 160 &&
                     distance <= DrawService.ProjectionCoefficient * 170)
            {
                type = ThrowType.Double;
            }

            // Find sector
            if (distance <= DrawService.ProjectionCoefficient * 7)
            {
                sector = 50;
                type = ThrowType.Bulleye;
            }
            else if (distance > DrawService.ProjectionCoefficient * 7 &&
                     distance <= DrawService.ProjectionCoefficient * 17)
            {
                sector = 25;
                type = ThrowType._25;
            }
            else if (distance > DrawService.ProjectionCoefficient * 170)
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

            return new DetectedThrow(poi, sector, type, DrawService.ProjectionFrameSide);
        }
    }
}