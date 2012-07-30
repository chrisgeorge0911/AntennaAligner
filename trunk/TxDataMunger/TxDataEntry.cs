using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TxDataMunger
{
    class TxDataEntry
    {
        public string Name { get; private set; }
        public string Ngr { get; private set; }
        public TxChannel Ch1 { get; private set; }
        public TxChannel Ch2 { get; private set; }
        public TxChannel Ch3 { get; private set; }
        public string Asl { get; private set; }
        public string Pol { get; set; }

        private Coordinate m_coord;

        public TxDataEntry(string name, string ngr, TxChannel ch1, TxChannel ch2, TxChannel ch3, string asl, string pol)
        {
            Name = name;
            Ngr = ngr;
            Ch1 = ch1;
            Ch2 = ch2;
            Ch3 = ch3;
            Asl = asl;
            Pol = pol;
        }

        public TxDataEntry(string name, double lats, double longs, TxChannel ch1, TxChannel ch2, TxChannel ch3, string asl, string pol)
        {
            Name = name;
            Ngr = "";
            Ch1 = ch1;
            Ch2 = ch2;
            Ch3 = ch3;
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
