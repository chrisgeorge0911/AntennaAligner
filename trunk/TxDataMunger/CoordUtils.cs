using System;
using DotNetCoords;

namespace TxDataMunger
{
    public class CoordUtils
    {
        public static Coordinate GetCoord(string ngrGridRef)
        {
            if ( string.IsNullOrEmpty(ngrGridRef))
                return new Coordinate(0,0);

            try
            {
                LatLng latlng;
                if (ngrGridRef.Length == 7)
                {
                    var irishref = new IrishRef(ngrGridRef);
                    latlng = irishref.ToLatLng();
                }
                else
                {
                    var osref = new OSRef(ngrGridRef);
                    latlng = osref.ToLatLng();
                }

                return new Coordinate(latlng.Latitude, latlng.Longitude);
            }
            catch (FormatException)
            {
                return new Coordinate(0,0);
            }
            
        }
    }
}
