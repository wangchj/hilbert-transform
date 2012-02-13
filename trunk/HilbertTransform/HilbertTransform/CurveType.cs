using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HilbertTransform
{
    public enum CurveType
    {
        /// <summary>
        /// Curve pointing Up with entry from the left.
        /// </summary>
        UpLeft = 0,
        /// <summary>
        /// Curve pointing Up with entry from the right.
        /// </summary>
        UpRight = 1,
        /// <summary>
        /// Curve pointing Down with entry from the left.
        /// </summary>
        DownLeft = 2,
        /// <summary>
        /// Curve pointing Down with entry from the right.
        /// </summary>
        DownRight = 3,
        /// <summary>
        /// Curve pointing Left with entry from the top.
        /// </summary>
        LeftTop = 4,
        /// <summary>
        /// Curve pointing Left with entry from the Bottom.
        /// </summary>
        LeftBottom = 5,
        /// <summary>
        /// Curve pointing Right with entry from the top.
        /// </summary>
        RightTop = 6,
        /// <summary>
        /// Curve pointing Right with entry from the bottom.
        /// </summary>
        RightBottom = 7
    }
}
