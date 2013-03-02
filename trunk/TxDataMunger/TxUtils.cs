using System.Text.RegularExpressions;

namespace TxDataMunger
{
    public static class TxUtils
    {
        public static double GetPowerInWatts(string power)
        {
            var powerStringRegex = new Regex(@"(?<thousands>[\d.]*k)?(?<value>[\d.]*)?");

            var data = powerStringRegex.Matches(power);

            var thousandsString = data[0].Groups["thousands"].Value.TrimEnd('k');
            var valueString = data[0].Groups["value"].Value;

            double powerValue = 0;
            if (thousandsString != "")
            {
                powerValue += double.Parse(thousandsString)*1000;
                if (valueString != "")
                {
                    powerValue += double.Parse(valueString)*100;
                }            
            }
            else
            {
                if (valueString != "")
                {
                    powerValue += double.Parse(valueString);
                }
            
            }
            
            return powerValue;

        }
    }
}