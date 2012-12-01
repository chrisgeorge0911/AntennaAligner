using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TxDataMunger
{
    class Program
    {
        static void Main()
        {
            IEnumerable<TxDataEntry> txData = ReadTxData();

            IEnumerable<TxDataEntry> ireTxData = ReadIreCsvData();
            
            GenerateJavascriptArray(txData.Union(ireTxData));
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
                    string channelInfoString = "";
                    foreach ( var channel in tx.Channels)
                    {
                        if ( channel.ChannelNumber == -1)
                        {
                            continue;
                        }

                        if (channelInfoString != "")
                            channelInfoString += ", ";

                        channelInfoString += String.Format("{{ch: '{0}',pwr: '{1}'}}", channel.ChannelNumber, channel.PowerInWatts);
                    }
                    channelInfoString = String.Format("var ch{0} = [{1}];", count, channelInfoString);

                    file.WriteLine(channelInfoString);

                    var arrayString = String.Format("    txList[{0}] = {{ name: '{1}', position: {{ coords: {{ latitude: {2}, longitude: {3} }}, gridref: '{4}' }}, channel: ch{0}, asl: '{5}', pol: '{6}' }};",
                        count, tx.Name, tx.Coord.Latitude, tx.Coord.Longitude, tx.Ngr, tx.Asl, tx.Pol);

                    count++;
                    file.WriteLine(arrayString);
                }

                file.WriteLine("return txList;");
                file.WriteLine("}");
            }
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

      
        /// <summary>
        /// Read the UK dtt data
        /// </summary>
        /// <returns></returns>
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
                
                string ch4 = line.Substring(62, 2).Trim();
                string ch4Pwr = line.Substring(65, 4).Trim();

                string ch5 = line.Substring(88, 2).Trim();
                string ch5Pwr = line.Substring(91, 4).Trim();

                string ch6 = line.Substring(101, 2).Trim();
                string ch6Pwr = line.Substring(104, 4).Trim();
                
                string ngr = line.Substring(119, 8).Replace(" ","");
                string pol = line.Substring(117, 1);

                string asl = "0";
                if ( line.Length > 130 )
                    asl = line.Substring(128, 3).Trim();

                var channels = new List<TxChannel>
                                   {
                                       new TxChannel(ch1, ch1Pwr),
                                       new TxChannel(ch2, ch2Pwr),
                                       new TxChannel(ch3, ch3Pwr),
                                       new TxChannel(ch4, ch4Pwr),
                                       new TxChannel(ch5, ch5Pwr),
                                       new TxChannel(ch6, ch6Pwr)
                                   };
                    
                txData.Add( new TxDataEntry(
                    name,
                    ngr,
                    channels,
                    asl,
                    pol
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

        private static IEnumerable<TxDataEntry> ReadIreCsvData()
        {
            string[] lines = File.ReadAllLines(@"..\..\txdata\iredttdata.csv");

            IList<TxDataEntry> txData = new List<TxDataEntry>();

            bool inData = false;
            foreach (string line in lines)
            {
                string[] dataline = line.Split(',');
            
                string name = dataline[0];
                string ch1 = dataline[2];
                string ch1Pwr = dataline[10] + "k";
                //string ch1aod = line.Substring(44, 3).Trim();
                string ch2 = dataline[3];
                string ch2Pwr = dataline[10] + "k";
                //string ch2aod = line.Substring(57, 3).Trim();
                string ch3 = dataline[4];
                string ch3Pwr = dataline[10] + "k";
                //string ch3aod = line.Substring(83, 3).Trim();
                double lats = Convert.ToDouble(dataline[8].Replace("\"", ""));
                double longs = Convert.ToDouble(dataline[9].Replace("\"", ""));
                string pol = dataline[6];

                string asl = "0";

                txData.Add(new TxDataEntry(
                    name,
                    lats,
                    longs,
                    new List<TxChannel>{ 
                    new TxChannel(ch1, ch1Pwr),
                    new TxChannel(ch2, ch2Pwr),
                    new TxChannel(ch3, ch3Pwr)},
                    asl,
                    pol
                    ));
            }

            return txData;
        }

            

       

        
    }
}
