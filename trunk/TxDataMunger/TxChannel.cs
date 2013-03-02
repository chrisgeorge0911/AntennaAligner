using System;

namespace TxDataMunger
{
    public class TxChannel
    {
        private readonly int m_ChannelNumber;
        private readonly double m_PowerInWatts; // Watts

        public TxChannel(string channelNumber, string power)
        {
            if (channelNumber == "" || channelNumber == "?")
                m_ChannelNumber = -1;
            else
            {
                m_ChannelNumber = Int32.Parse(channelNumber);
                m_PowerInWatts = TxUtils.GetPowerInWatts(power);
            }
        }

        public int ChannelNumber
        {
            get { return m_ChannelNumber; }
        }

        public double PowerInWatts
        {
            get { return m_PowerInWatts; }
        }
    }
}