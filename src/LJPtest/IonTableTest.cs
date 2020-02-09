using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LJPtest
{
    class IonTableTest
    {
        [Test]
        public void Test_Lookup_ValuesMatchExpected()
        {
            var ionCl = new LJPmath.Ion("Cl");
            Assert.That(ionCl.charge == -1);
            Assert.That(ionCl.mu == 4.95159e+11);
        }

        [Test]
        public void Test_Lookup_IsCaseInsensitive()
        {
            var ionZn = new LJPmath.Ion("Zn");
            var ionZnUpper = new LJPmath.Ion("ZN");
            var ionZnLower = new LJPmath.Ion("zn");

            Assert.That(ionZn.mu == ionZnUpper.mu);
            Assert.That(ionZn.mu == ionZnLower.mu);
            Assert.That(ionZn.mu == ionZnLower.mu);
        }

        [Test]
        public void Test_Lookup_UnknownIon()
        {
            var ionZn = new LJPmath.Ion("Zn");
            var ionUnknown = new LJPmath.Ion("lolz");

            Assert.That(ionZn.mu != ionUnknown.mu);
        }
    }
}
