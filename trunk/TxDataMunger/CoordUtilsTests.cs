using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TxDataMunger
{
    [TestFixture]
    public class CoordUtilsTests
    {
        [TestCase("SN614524", 52.152055836792499, -4.0260902468556852)]
        [TestCase("J134460", 54.349978604149122, -6.2557852272036216)]        
        [TestCase("", 0,0)]
        [TestCase(null, 0, 0)]
        [TestCase("ABC12345", 0, 0)]
        public void GetCoordTest(string ngrGridRef, double latitude, double longitude) 
        {

            Coordinate result = CoordUtils.GetCoord(ngrGridRef);
            
            Assert.That(result.Latitude, Is.EqualTo(latitude));
            Assert.That(result.Longitude, Is.EqualTo(longitude));

        }


    }
}
