using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using DotNetCoords;

namespace TxDataMunger
{
    class Program
    {
        static void Main()
        {
            IEnumerable<TxDataEntry> txData = ReadTxData();

            GenerateJavascriptArray(txData);

        }

        private static void GenerateJavascriptArray(IEnumerable<TxDataEntry> txData)
        {
            int count = 0;

            using (var file = new StreamWriter(@"..\..\..\NomadProject1\scripts\txdata.js"))
            {

                file.WriteLine("function getListOfTx() {");
                file.WriteLine("    var txList = new Array();");


                foreach (var tx in txData)
                {
                    var arrayString = String.Format("    txList[{0}] = {{ name: '{1}', position: {{ coords: {{ latitude: {2}, longitude: {3} }}, gridref: '{4}' }}, ch1: '{5}', ch1pwr: '{6}', ch2: '{7}', ch2pwr: '{8}', ch3: '{9}', ch3pwr: '{10}', asl: '{11}' }};",
                        count, tx.Name, tx.Coord.Latitude, tx.Coord.Longitude, tx.Ngr, tx.Ch1.ChannelNumber, tx.Ch1.PowerInWatts, tx.Ch2.ChannelNumber, tx.Ch2.PowerInWatts, tx.Ch3.ChannelNumber, tx.Ch3.PowerInWatts, tx.Asl);

                    count++;
                    file.WriteLine(arrayString);
                }

                file.WriteLine("return txList;");
                file.WriteLine("}");
            }
        }

        private static Coordinate GetCoord(string ngrGridRef)
        {
            LatLng latlng;
            if ( ngrGridRef.Length==7)
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


        //private static string NGRToNE(string ngrGridRef)
        //{

        //    double e;
        //    double n;

        //    var c = ngrGridRef[0];
        //    if (c == 'S')
        //    {
        //        e = 0;
        //        n = 0;
        //    }
        //    else if (c == 'T')
        //    {
        //        e = 500000;
        //        n = 0;
        //    }
        //    else if (c == 'N')
        //    {
        //        n = 500000;
        //        e = 0;
        //    }
        //    else if (c == 'O')
        //    {
        //        n = 500000;
        //        e = 500000;
        //    }
        //    else if (c == 'H')
        //    {
        //        n = 1000000;
        //        e = 0;
        //    }
        //    else
        //        return null;

        //    c = ngrGridRef[1];
        //    if (c == 'I')
        //        return null;


        //    int code = ((int)ngrGridRef[1]) - 65;
        //    if (code > 8)
        //        code -= 1;
        //    e += (code % 5) * 100000;


        //    double dCode = code / 5;
        //    n += (4 - Math.Floor(dCode)) * 100000;

        //    string ngr = ngrGridRef.Substring(2);
        //    if ((ngr.Length % 2) == 1)
        //        return null;
        //    if (ngr.Length > 10)
        //        return null;

        //    try
        //    {
        //        string s = ngr.Substring(0, ngr.Length / 2);
        //        while (s.Length < 5)
        //            s += '0';
        //        e += double.Parse(s);

        //        s = ngr.Substring(ngr.Length / 2);
        //        while (s.Length < 5)
        //            s += '0';
        //        n += double.Parse(s);

        //        return e + "," + n;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

      
        private static IEnumerable<TxDataEntry> ReadTxData()
        {
            string[] lines = File.ReadAllLines(@"..\..\txdata\dttdata.txt");

            IList<TxDataEntry> txData = new List<TxDataEntry>();

            bool inData = false;
            foreach (string line in lines)
            {
                
                if (line.StartsWith("------------------------------------------------"))
                {
                    inData = !inData;
                    continue;
                }

                if ( !inData)
                    continue;

                //if (line.StartsWith("Region:"))
                //{
                //    currentRegion = line.Substring(7);
                //    continue;
                //}

                //Winshill SK272241 60- 60W 57 60W 53 60W C/DV C/DV 66 48 56 68
                //Regex regex = new Regex(@"(?<name>.*)\s(?<ngr>[A-Z]{2}\d{6})\s(?<ch1>\d{2})[+-]{0,1}\s(?<ch1pwr>.*)\s(?<ch2>\d{2})[+-]{0,1}\s(?<ch2pwr>.*)\s(?<ch3>\d{2})[+-]{0,1}\s(?<ch3pwr>.*)\s");


                /*
                        Multiplex:  1-(PSB1)---- 2-(PSB2)---- A-(COM1)---- B-(PSB3)---- C-(COM2)---- D-(COM3)----
Tx                                  Ch ERPW mAOD Ch ERPW mAOD Ch ERPW mAOD Ch ERPW mAOD Ch ERPW mAOD Ch ERPW mAOD Gp P OS       ODm
0--------9--------------------------36---------------------------------------------------------------------------------------------
123.00 - Angus                      60  20k 547  53  20k 547  54  20k 547  57  10k 547  58  10k 547  61  10k 547  CD H NO394407 313
123.12   Auchtermuchty              49   20 130  42   20 130               45   20 130                            B  V NO214094  93
123.22   Balmullo                   49    2  90  42    2  90               45    2  90                            B  V NO426214  69
123.19   Balnaguard                 49    2 193  45    2 193               42    2 193                            B  V NN956511 176
                 * */
                
                //string type = line.Substring(7, 1);
                string name = line.Substring(9, 25).Trim().Replace("'","");
                string ch1 = line.Substring(36, 2).Trim();
                string ch1Pwr = line.Substring(39, 4).Trim();
                //string ch1aod = line.Substring(44, 3).Trim();
                string ch2 = line.Substring(49, 2).Trim();
                string ch2Pwr = line.Substring(52, 4).Trim();
                //string ch2aod = line.Substring(57, 3).Trim();
                string ch3 = line.Substring(75, 2).Trim();
                string ch3Pwr = line.Substring(78, 4).Trim();
                //string ch3aod = line.Substring(83, 3).Trim();
                string ngr = line.Substring(119, 8).Replace(" ","");

                string asl = "0";
                if ( line.Length > 130 )
                    asl = line.Substring(128, 3).Trim();


                txData.Add( new TxDataEntry(
                    name,
                    ngr,
                    new TxChannel(ch1, ch1Pwr),
                    new TxChannel(ch2, ch2Pwr),
                    new TxChannel(ch3, ch3Pwr),
                    asl
                    ));

                


                //try
                //{
                //    MatchCollection data = regex.Matches(line);

                //    txData.Add(new TxDataEntry(
                //        data[0].Groups["name"].Value,
                //        data[0].Groups["ngr"].Value,
                //        new TxChannel(data[0].Groups["ch1"].Value, data[0].Groups["ch1pwr"].Value),
                //        new TxChannel(data[0].Groups["ch2"].Value, data[0].Groups["ch2pwr"].Value),
                //        new TxChannel(data[0].Groups["ch3"].Value, data[0].Groups["ch3pwr"].Value), 
                //        currentRegion));
                //}
                //catch (Exception ex)
                //{ }
    
            }

            return txData;
        }

        private class TxDataEntry
        {
            public string Name { get; private set; }
            public string Ngr {get; private set; }
            public TxChannel Ch1 {get; private set;}
            public TxChannel Ch2 { get; private set; }
            public TxChannel Ch3 { get; private set; }
            public string Asl { get; private set; }

            private Coordinate m_coord;

            public TxDataEntry(string name, string ngr, TxChannel ch1, TxChannel ch2, TxChannel ch3, string asl)
            {
                Name = name;
                Ngr = ngr;
                Ch1 = ch1;
                Ch2 = ch2;
                Ch3 = ch3;
                Asl = asl;
            }
            
            public Coordinate Coord 
            { 
                get { return m_coord ?? (m_coord = GetCoord(Ngr)); }
            }
        }

        private class TxChannel
        {
            private readonly int m_channelNumber;
            private readonly float m_powerInWatts; // Watts

            public TxChannel(string channelNumber, string power)
            {
                if (channelNumber == "")
                    m_channelNumber = -1;
                else
                {
                    m_channelNumber = Int32.Parse(channelNumber);
                    m_powerInWatts = GetPowerInWatts(power);
                }                
            }

            public int ChannelNumber
            {
                get { return m_channelNumber; }
            }

            public float PowerInWatts
            {
                get { return m_powerInWatts; }
            }

            private float GetPowerInWatts(string power)
            {
                var powerStringRegex = new Regex(@"(?<value>[\d.]*)(?<units>\w*)");

                var data = powerStringRegex.Matches(power);

                var valueString = data[0].Groups["value"].Value;
                var valueUnit = data[0].Groups["units"].Value;

                var powerValue = float.Parse(valueString);

                if (valueUnit.Contains("k"))
                    powerValue = powerValue*1000;

                return powerValue;
            }
        }
    }
}
