using System;
using System.Text.RegularExpressions;

namespace TxDataMunger
{
    class TxChannel
    {
        private readonly int m_channelNumber;
        private readonly float m_powerInWatts; // Watts

        public TxChannel(string channelNumber, string power)
        {
            if (channelNumber == "" || channelNumber == "?")
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
                powerValue = powerValue * 1000;

            return powerValue;
        }
    }
}
