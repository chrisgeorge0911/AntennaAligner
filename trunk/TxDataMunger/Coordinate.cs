namespace TxDataMunger
{
    public class Coordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Coordinate(string coordinate)
        {
            string[] coordinates = coordinate.Split(',');

            Latitude = double.Parse(coordinates[0]);
            Longitude = double.Parse(coordinates[1]);
        }

        public Coordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }        
    }


}
