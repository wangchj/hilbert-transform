using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HilbertTransform
{
    /// <summary>
    /// A position in two-dimensional space specified by X and Y.
    /// </summary>
    public class Point2D
    {
        protected double x;
        protected double y;

        public Point2D() { }

        public Point2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public virtual double X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public virtual double Y
        {
            get { return this.y; }
            set { this.y = value; }
        }
    }
}
