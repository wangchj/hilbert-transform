using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HilbertTransform
{

    class Program
    {
        static void Main(string[] args)
        {
            //int quadIndex = HCTransform.GetQuadIndex(new Point2D(10, 0),
            //    new Point2D(0, 0), new Point2D(10, 10));
            //Console.WriteLine(quadIndex);

//            int v = HCTransform.Transform(new Point2D(-1f, -4f),
//                new Point2D(-4f, -4f), new Point2D(4f, 4f),
//                3, CurveType.LeftTop);
//            Console.WriteLine(v);

            //HCTransform.DrawCurveKml(new Point2D(-85.59, 32.51),
            //    new Point2D(-85.41, 32.69), 7, CurveType.UpLeft);

            Console.WriteLine(HCTransform.Transform(
                new Point2D(-85.481880, 32.608915),
                new Point2D(-85.59, 32.51),
                new Point2D(-85.41, 32.69),
                7,
                CurveType.UpLeft));
            Console.WriteLine(HCTransform.Transform(
                new Point2D(-85.480510, 32.609240),
                new Point2D(-85.59, 32.51),
                new Point2D(-85.41, 32.69),
                7,
                CurveType.UpLeft));
            Console.WriteLine(HCTransform.Transform(
                new Point2D(-85.479774, 32.608709),
                new Point2D(-85.59, 32.51),
                new Point2D(-85.41, 32.69),
                7,
                CurveType.UpLeft));
            Console.WriteLine(HCTransform.Transform(
                new Point2D(-85.481590, 32.610287),
                new Point2D(-85.59, 32.51),
                new Point2D(-85.41, 32.69),
                7,
                CurveType.UpLeft));

            Console.WriteLine(HCTransform.Transform(
                new Point2D(-85.5345952175, 32.55961729),
                new Point2D(-85.59, 32.51),
                new Point2D(-85.41, 32.69),
                7,
                CurveType.UpLeft));

            Console.ReadLine();
        }
    }
}
