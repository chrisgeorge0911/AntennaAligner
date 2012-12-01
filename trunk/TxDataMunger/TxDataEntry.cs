using System.Collections.Generic;

namespace TxDataMunger
{
    class TxDataEntry
    {
        public string Name { get; private set; }
        public string Ngr { get; private set; }
        public List<TxChannel> Channels { get; private set; }
        public string Asl { get; private set; }
        public string Pol { get; set; }

        private Coordinate m_coord;

        public TxDataEntry(string name, string ngr, List<TxChannel> channels, string asl, string pol)
        {
            Name = name;
            Ngr = ngr;
            Channels = channels;
            Asl = asl;
            Pol = pol;
        }

        public TxDataEntry(string name, double lats, double longs, List<TxChannel> channels, string asl, string pol)
        {
            Name = name;
            Ngr = "";
            Channels = channels;
            Asl = asl;
            Pol = pol;
            m_coord = new Coordinate(lats, longs);
        }

        public Coordinate Coord
        {
            get { return m_coord ?? (m_coord = CoordUtils.GetCoord(Ngr)); }
        }
    }


}
