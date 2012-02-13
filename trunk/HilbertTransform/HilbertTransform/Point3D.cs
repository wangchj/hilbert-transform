using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HilbertTransform
{
    /// <summary>
    /// A position in three-dimensional space specified by X, Y, and Z.
    /// </summary>
    public class Point3D : Point2D
    {
        protected float z;

        public Point3D(float x, float y, float z) :
            base(x, y)
        {
            this.z = z;
        }

        public virtual float Z
        {
            get { return this.z; }
            set { this.z = value; }
        }
    }
}
