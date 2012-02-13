using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HilbertTransform
{

    /// <summary>
    /// Hilbert spatil coordinates Transformation.
    /// </summary>
    public class HCTransform
    {
        /// <summary>
        /// QuadIndex Transformation Matrix.
        /// Transform: (CurveType, QuadIndex) -> CurveQuadIndex
        /// </summary>
        private static int[,] qtMatrix = new int[,] {
            {0, 1, 2, 3}, //UpLeft Translations
            {3, 2, 1, 0}, //UpRight
            {1, 0, 3, 2}, //DownLeft
            {2, 3, 0, 1}, //DownRight
            {0, 3, 2, 1}, //LeftTop
            {3, 0, 1, 2}, //LeftBottom
            {1, 2, 3, 0}, //RightTop
            {2, 1, 0, 3}  //RightBottom
        };

        //Short acronyms for curve types.
        private static CurveType ul = CurveType.UpLeft;
        private static CurveType ur = CurveType.UpRight;
        private static CurveType dl = CurveType.DownLeft;
        private static CurveType dr = CurveType.DownRight;
        private static CurveType lt = CurveType.LeftTop;
        private static CurveType lb = CurveType.LeftBottom;
        private static CurveType rt = CurveType.RightTop;
        private static CurveType rb = CurveType.RightBottom;

        /// <summary>
        /// Curve Type Transformation Matrix.
        /// Transform: (CurveType, CurveQuadIndex) -> CurveType (child curve)
        /// </summary>
        private static CurveType[,] ctMatrix = new CurveType[,] {
            {lt, ul, ul, rb}, //UpLeft
            {rt, ur, ur, lb}, //UpRight
            {lb, dl, dl, rt}, //DownLeft
            {rb, dr, dr, lt}, //DownRight
            {ul, lt, lt, dr}, //LeftTop
            {dl, lb, lb, ur}, //LeftBottom
            {ur, rt, rt, dl}, //RightTop
            {dr, rb, rb, ul}  //RightBottom
        };

        /// <summary>
        /// Transforms a 2D point to an Hilbert value.
        /// </summary>
        /// <param name="p">The point to be transformed.</param>
        /// <param name="lowerLeft">The lower-left corner of the bound.</param>
        /// <param name="upperRight">The upper-right corner of the bound.</param>
        /// <param name="order">Hilbert curve order (or level).</param>
        /// <param name="curve">The direction of the curve.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">If p is not within bound
        /// or curve order is less than 1.</exception>
        public static int Transform(Point2D p, Point2D lowerLeft,
            Point2D upperRight, int order, CurveType curve)
        {
            //TODO: Specify the precision of coordinates.

            if (order < 1) throw new ArgumentException("Order must be 1 or greater.");

            double xBisector = (lowerLeft.X + upperRight.X) / 2;
            double yBisector = (lowerLeft.Y + upperRight.Y) / 2;

            //Get the quadrant the point is in.
            int quadIndex = GetQuadIndex(p, lowerLeft, upperRight);
            //Translate quadrant into curve quadrant order.
            int curveQuadIndex = GetCurveQuadIndex(quadIndex, curve);

            //Base case when the order is 1
            if (order == 1) return curveQuadIndex;

            //Other cases when order is greater than 1.
            
            Point2D nextLowerLeft = new Point2D();
            Point2D nextUpperRight = new Point2D();

            if (quadIndex == 0)
            {
                nextLowerLeft.X = lowerLeft.X;
                nextLowerLeft.Y = yBisector;
                nextUpperRight.X = xBisector;
                nextUpperRight.Y = upperRight.Y;
            }
            else if (quadIndex == 1)
            {
                nextLowerLeft.X = lowerLeft.X;
                nextLowerLeft.Y = lowerLeft.Y;
                nextUpperRight.X = xBisector;
                nextUpperRight.Y = yBisector;
            }
            else if (quadIndex == 2)
            {
                nextLowerLeft.X = xBisector;
                nextLowerLeft.Y = lowerLeft.Y;
                nextUpperRight.X = upperRight.X;
                nextUpperRight.Y = yBisector;
            }
            else if (quadIndex == 3)
            {
                nextLowerLeft.X = xBisector;
                nextLowerLeft.Y = yBisector;
                nextUpperRight.X = upperRight.X;
                nextUpperRight.Y = upperRight.Y;
            }

            return (int)Math.Pow(4, order - 1) * curveQuadIndex +
                Transform(p, nextLowerLeft, nextUpperRight, order - 1,
                ctMatrix[(int)curve, curveQuadIndex]);
        }

        public static void DrawCurveKml(Point2D lowerLeft,
            Point2D upperRight, int order, CurveType curve)
        {
            //TODO: Specify the precision of coordinates.

            if (order < 1) throw new ArgumentException("Order must be 1 or greater.");

            if (order == 1)
            {
                DrawBaseCurveKml(lowerLeft, upperRight, curve);
                return;
            }

            double xBisector = (lowerLeft.X + upperRight.X) / 2;
            double yBisector = (lowerLeft.Y + upperRight.Y) / 2;
            
            if (curve == CurveType.UpLeft)
            {
                DrawCurveKml(new Point2D(lowerLeft.X, yBisector),
                    new Point2D(xBisector, upperRight.Y), order - 1,
                    ctMatrix[(int)curve, 0]);

                DrawCurveKml(new Point2D(lowerLeft.X, lowerLeft.Y),
                    new Point2D(xBisector, yBisector), order - 1,
                    ctMatrix[(int)curve, 1]);
                
                DrawCurveKml(new Point2D(xBisector, lowerLeft.Y),
                    new Point2D(upperRight.X, yBisector), order - 1,
                    ctMatrix[(int)curve, 2]);
                
                DrawCurveKml(new Point2D(xBisector, yBisector),
                    new Point2D(upperRight.X, upperRight.Y), order - 1,
                    ctMatrix[(int)curve, 3]);
            }
            else if (curve == CurveType.UpRight)
            {
                DrawCurveKml(new Point2D(xBisector, yBisector),
                    new Point2D(upperRight.X, upperRight.Y), order - 1,
                    ctMatrix[(int)curve, 0]);

                DrawCurveKml(new Point2D(xBisector, lowerLeft.Y),
                    new Point2D(upperRight.X, yBisector), order - 1,
                    ctMatrix[(int)curve, 1]);

                DrawCurveKml(new Point2D(lowerLeft.X, lowerLeft.Y),
                    new Point2D(xBisector, yBisector), order - 1,
                    ctMatrix[(int)curve, 2]);

                DrawCurveKml(new Point2D(lowerLeft.X, yBisector),
                    new Point2D(xBisector, upperRight.Y), order - 1,
                    ctMatrix[(int)curve, 3]);
            }
            else if (curve == CurveType.DownLeft)
            {
                DrawCurveKml(new Point2D(lowerLeft.X, lowerLeft.Y),
                    new Point2D(xBisector, yBisector), order - 1,
                    ctMatrix[(int)curve, 0]);

                DrawCurveKml(new Point2D(lowerLeft.X, yBisector),
                    new Point2D(xBisector, yBisector), order - 1,
                    ctMatrix[(int)curve, 1]);

                DrawCurveKml(new Point2D(xBisector, yBisector),
                    new Point2D(upperRight.X, upperRight.Y), order - 1,
                    ctMatrix[(int)curve, 2]);

                DrawCurveKml(new Point2D(xBisector, lowerLeft.Y),
                    new Point2D(upperRight.X, yBisector), order - 1,
                    ctMatrix[(int)curve, 3]);
            }
            else if (curve == CurveType.DownRight)
            {
                DrawCurveKml(new Point2D(xBisector, lowerLeft.Y),
                    new Point2D(upperRight.X, yBisector), order - 1,
                    ctMatrix[(int)curve, 0]);

                DrawCurveKml(new Point2D(xBisector, yBisector),
                    new Point2D(upperRight.X, upperRight.Y), order - 1,
                    ctMatrix[(int)curve, 1]);

                DrawCurveKml(new Point2D(lowerLeft.X, yBisector),
                    new Point2D(xBisector, upperRight.Y), order - 1,
                    ctMatrix[(int)curve, 2]);

                DrawCurveKml(new Point2D(lowerLeft.X, lowerLeft.Y),
                    new Point2D(xBisector, yBisector), order - 1,
                    ctMatrix[(int)curve, 3]);
            }
            else if (curve == CurveType.LeftTop)
            {
                DrawCurveKml(new Point2D(lowerLeft.X, yBisector),
                    new Point2D(xBisector, upperRight.Y), order - 1,
                    ctMatrix[(int)curve, 0]);

                DrawCurveKml(new Point2D(xBisector, yBisector),
                    new Point2D(upperRight.X, upperRight.Y), order - 1,
                    ctMatrix[(int)curve, 1]);

                DrawCurveKml(new Point2D(xBisector, lowerLeft.Y),
                    new Point2D(upperRight.X, yBisector), order - 1,
                    ctMatrix[(int)curve, 2]);

                DrawCurveKml(new Point2D(lowerLeft.X, lowerLeft.Y),
                    new Point2D(xBisector, yBisector), order - 1,
                    ctMatrix[(int)curve, 3]);
            }
            else if (curve == CurveType.LeftBottom)
            {
                DrawCurveKml(new Point2D(lowerLeft.X, lowerLeft.Y),
                    new Point2D(xBisector, yBisector), order - 1,
                    ctMatrix[(int)curve, 0]);

                DrawCurveKml(new Point2D(xBisector, lowerLeft.Y),
                    new Point2D(upperRight.X, yBisector), order - 1,
                    ctMatrix[(int)curve, 1]);

                DrawCurveKml(new Point2D(xBisector, yBisector),
                    new Point2D(upperRight.X, upperRight.Y), order - 1,
                    ctMatrix[(int)curve, 2]);

                DrawCurveKml(new Point2D(lowerLeft.X, yBisector),
                    new Point2D(xBisector, upperRight.Y), order - 1,
                    ctMatrix[(int)curve, 3]);
            }
            else if (curve == CurveType.RightTop)
            {
                DrawCurveKml(new Point2D(xBisector, yBisector),
                    new Point2D(upperRight.X, upperRight.Y), order - 1,
                    ctMatrix[(int)curve, 0]);

                DrawCurveKml(new Point2D(lowerLeft.X, yBisector),
                    new Point2D(xBisector, upperRight.Y), order - 1,
                    ctMatrix[(int)curve, 1]);

                DrawCurveKml(new Point2D(lowerLeft.X, lowerLeft.Y),
                    new Point2D(xBisector, yBisector), order - 1,
                    ctMatrix[(int)curve, 2]);

                DrawCurveKml(new Point2D(xBisector, lowerLeft.Y),
                    new Point2D(upperRight.X, yBisector), order - 1,
                    ctMatrix[(int)curve, 3]);
            }
            else if (curve == CurveType.RightBottom)
            {
                DrawCurveKml(new Point2D(xBisector, lowerLeft.Y),
                    new Point2D(upperRight.X, yBisector), order - 1,
                    ctMatrix[(int)curve, 0]);

                DrawCurveKml(new Point2D(lowerLeft.X, lowerLeft.Y),
                    new Point2D(xBisector, yBisector), order - 1,
                    ctMatrix[(int)curve, 1]);

                DrawCurveKml(new Point2D(lowerLeft.X, yBisector),
                    new Point2D(xBisector, upperRight.Y), order - 1,
                    ctMatrix[(int)curve, 2]);

                DrawCurveKml(new Point2D(xBisector, yBisector),
                    new Point2D(upperRight.X, upperRight.Y), order - 1,
                    ctMatrix[(int)curve, 3]);
            }
            
        }

        public static void DrawBaseCurveKml(Point2D lowerLeft,
            Point2D upperRight, CurveType curve)
        {
            double ux = (3 * upperRight.X + lowerLeft.X) / 4;
            double lx = (3 * lowerLeft.X + upperRight.X) / 4;
            double uy = (3 * upperRight.Y + lowerLeft.Y) / 4;
            double ly = (3 * lowerLeft.Y + upperRight.Y) / 4;

            if (curve == CurveType.UpLeft)
            {
                Console.Write(lx.ToString("###.##############\\,"));
                Console.Write(uy.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(lx.ToString("###.##############\\,"));
                Console.Write(ly.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(ux.ToString("###.##############\\,"));
                Console.Write(ly.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(ux.ToString("###.##############\\,"));
                Console.Write(uy.ToString("##.##############\\,"));
                Console.Write("0 ");
            }
            else if (curve == CurveType.UpRight)
            {
                Console.Write(ux.ToString("###.##############\\,"));
                Console.Write(uy.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(ux.ToString("###.##############\\,"));
                Console.Write(ly.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(lx.ToString("###.##############\\,"));
                Console.Write(ly.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(lx.ToString("###.##############\\,"));
                Console.Write(uy.ToString("##.##############\\,"));
                Console.Write("0 ");
            }
            else if (curve == CurveType.DownLeft)
            {
                Console.Write(lx.ToString("###.##############\\,"));
                Console.Write(ly.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(lx.ToString("###.##############\\,"));
                Console.Write(uy.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(ux.ToString("###.##############\\,"));
                Console.Write(uy.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(ux.ToString("###.##############\\,"));
                Console.Write(ly.ToString("##.##############\\,"));
                Console.Write("0 ");
            }
            else if (curve == CurveType.DownRight)
            {
                Console.Write(ux.ToString("###.##############\\,"));
                Console.Write(ly.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(ux.ToString("###.##############\\,"));
                Console.Write(uy.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(lx.ToString("###.##############\\,"));
                Console.Write(uy.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(lx.ToString("###.##############\\,"));
                Console.Write(ly.ToString("##.##############\\,"));
                Console.Write("0 ");
            }
            else if (curve == CurveType.LeftTop)
            {
                Console.Write(lx.ToString("###.##############\\,"));
                Console.Write(uy.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(ux.ToString("###.##############\\,"));
                Console.Write(uy.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(ux.ToString("###.##############\\,"));
                Console.Write(ly.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(lx.ToString("###.##############\\,"));
                Console.Write(ly.ToString("##.##############\\,"));
                Console.Write("0 ");
            }
            else if (curve == CurveType.LeftBottom)
            {
                Console.Write(lx.ToString("###.##############\\,"));
                Console.Write(ly.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(ux.ToString("###.##############\\,"));
                Console.Write(ly.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(ux.ToString("###.##############\\,"));
                Console.Write(uy.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(lx.ToString("###.##############\\,"));
                Console.Write(uy.ToString("##.##############\\,"));
                Console.Write("0 ");
            }
            else if (curve == CurveType.RightTop)
            {
                Console.Write(ux.ToString("###.##############\\,"));
                Console.Write(uy.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(lx.ToString("###.##############\\,"));
                Console.Write(uy.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(lx.ToString("###.##############\\,"));
                Console.Write(ly.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(ux.ToString("###.##############\\,"));
                Console.Write(ly.ToString("##.##############\\,"));
                Console.Write("0 ");
            }
            else if (curve == CurveType.RightBottom)
            {
                Console.Write(ux.ToString("###.##############\\,"));
                Console.Write(ly.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(lx.ToString("###.##############\\,"));
                Console.Write(ly.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(lx.ToString("###.##############\\,"));
                Console.Write(uy.ToString("##.##############\\,"));
                Console.Write("0 ");

                Console.Write(ux.ToString("###.##############\\,"));
                Console.Write(uy.ToString("##.##############\\,"));
                Console.Write("0 ");
            }
        }

        /// <summary>
        /// Gets the quadrant in which the point p is in. This method first
        /// subdivide the space into 4 quadrants. The quadrants have the
        /// following index: upper left 0, lower left 1, lower right 2,
        /// upper right 3.
        /// </summary>
        /// <param name="p">The point</param>
        /// <param name="lowerLeft">The lower left point of the space.</param>
        /// <param name="upperRight">The upper right point of the space.</param>
        /// <returns>An integer representing the quadrant index.</returns>
        /// <exception cref="ArgumentException">If point is not in the bounds of
        /// lower left and upper right.</exception>
        public static int GetQuadIndex(Point2D p, Point2D lowerLeft,
            Point2D upperRight)
        {
            //Check if p is in bound.
            if (p.X < lowerLeft.X || p.X > upperRight.X ||
                p.Y < lowerLeft.Y || p.Y > upperRight.Y)
                throw new ArgumentException("Point is out of bound.");

            //Find the boundaries of the quadrants.
            double xBisector = (lowerLeft.X + upperRight.X) / 2;
            double yBisector = (lowerLeft.Y + upperRight.Y) / 2;

            if (p.X <= xBisector)
            {
                if (p.Y <= yBisector) return 1;
                else return 0;
            }
            else
            {
                if (p.Y <= yBisector) return 2;
                else return 3;
            }
        }

        public static int GetCurveQuadIndex(int quadIndex,
            CurveType curve)
        {
            return qtMatrix[(int)curve, quadIndex];
        }



    }
}
