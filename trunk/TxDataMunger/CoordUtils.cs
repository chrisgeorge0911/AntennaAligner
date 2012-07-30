using DotNetCoords;

namespace TxDataMunger
{
    class CoordUtils
    {
        public static Coordinate GetCoord(string ngrGridRef)
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
    }
}
