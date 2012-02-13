using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HilbertTransform
{
    public class GeoCoord2D : Point2D
    {
        public GeoCoord2D(double lat, double lon) :
            base(lon, lat)
        {
            if (!LatIsValid(lat))
                throw new ArgumentException("Latitude is out of range.");
            if (!LonIsValid(lon))
                throw new ArgumentException("Longitude is out of range.");
        }

        public double Latitude
        {
            get { return base.y; }
            set
            {
                if (!LatIsValid(value))
                    throw new ArgumentException("Latitude is out of range.");
                base.y = value;
            }
        }

        public double Longitude
        {
            get { return base.x; }
            set
            {
                if (!LonIsValid(value))
                    throw new ArgumentException("Longitude is out of range.");
                base.x = value;
            }
        }

        public override double X
        {
            get
            {
                return base.X;
            }
            set
            {
                Longitude = value;
            }
        }

        public override double Y
        {
            get
            {
                return base.Y;
            }
            set
            {
                Latitude = value;
            }
        }

        public static bool LatIsValid(double lat)
        {
            if (lat < -90 || lat > 90)
                return false;
            return true;
        }

        public static bool LonIsValid(double lon)
        {
            if (lon < -180 || lon > 180)
                return false;
            return true;
        }
    }
}
