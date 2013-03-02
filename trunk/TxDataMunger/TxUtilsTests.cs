using NUnit.Framework;

namespace TxDataMunger
{
    [TestFixture]
    public class TxUtilsTests
    {
        [TestCase("10", 10)]
        [TestCase("1k", 1000)]
        [TestCase("1k5", 1500)]
        [TestCase("2.5", 2.5)]
        [TestCase("125k", 125000)]
        public void GetPowerInWattsTests(string powerString, double powerValue)
        {
            Assert.AreEqual(powerValue, TxUtils.GetPowerInWatts(powerString));
        }

    }
}
