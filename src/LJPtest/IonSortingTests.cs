using LJPmath;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LJPtest
{
    class IonSortingTests
    {
        [Test]
        public void Test_BadOrder_FixedByAutoSort()
        {
            /* Known to crash due to poor ion order https://github.com/swharden/LJPcalc/issues/9 */
            // fixed by autoSort prior to calculation

            var ionSet = new List<Ion>(){
                    new Ion("Zn", 9, .0284),
                    new Ion("K", 0, 3),
                    new Ion("Cl", 18, 3.062),
                    new Ion("Mg", 5, 3),
                    new Ion("Ag", 1, 1),
                };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            var ljp = Calculate.Ljp(ionSet);
            Assert.AreEqual(-11.9, ljp.mV, 0.5);
        }

        [Test]
        public void Test_BadOrderWithoutAutoSort_ThrowsAfterFiveSeconds()
        {
            /* Known to crash due to poor ion order https://github.com/swharden/LJPcalc/issues/9 */
            // fixed by autoSort prior to calculation

            var ionSet = new List<Ion>(){
                    new Ion("Zn", 9, .0284),
                    new Ion("K", 0, 3),
                    new Ion("Cl", 18, 3.062),
                    new Ion("Mg", 5, 3),
                    new Ion("Ag", 1, 1),
                };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            Assert.Throws<OperationCanceledException>(() => Calculate.Ljp(ionSet, autoSort: false));
        }
    }
}
