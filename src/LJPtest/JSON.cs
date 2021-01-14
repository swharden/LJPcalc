using LJPmath;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LJPtest
{
    class JSON
    {
        private Ion[] GetTestSet()
        {
            var ionSet = new List<Ion>()
            {
                new Ion("K", 50, 3),
                new Ion("Gluconate", 50, 0),
                new Ion("Cs", 70, 0),
                new Ion("HSO3", 70, 0),
                new Ion("TEA (TetraethylAmmonium)", 10, 10),
                new Ion("Cl", 12, 131.6),
                new Ion("Mg", 5, 1.3),
                new Ion("HEPES", 10, 0),
                new Ion("EGTA(2-)", .3, 0),
                new Ion("Tris", 10, 0),
                new Ion("ATP (Adenosine 5'-Triphosphate)", 4, 0),
                new Ion("Na", 0.3, 139.25),
                new Ion("HCO3", 0, 26),
                new Ion("H2PO4", 0, 1.25),
                new Ion("Ca", 0, 2),
                new Ion("4-AP (4-aminopyridine)", 0, 5),
            };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            return ionSet.ToArray();
        }

        [Test]
        public void Test_JSON_experiment()
        {
            Ion[] ions1 = GetTestSet();
            double temperature1 = 25;
            Experiment exp1 = new Experiment(ions1, temperature1);
            string json1 = exp1.ToJson();
            Assert.That(string.IsNullOrWhiteSpace(json1) == false, "json is empty");
            Console.WriteLine(json1);

            Experiment exp2 = Experiment.FromJson(json1);
            Ion[] ions2 = exp2.Ions;
            double temperature2 = exp2.TemperatureC;

            Assert.AreEqual(temperature1, temperature2);
            Assert.IsNotNull(ions2);
            Assert.AreEqual(ions1.Length, ions2.Length);
            for (int i = 0; i < ions1.Length; i++)
            {
                Assert.AreEqual(ions1[i].name, ions2[i].name);
                Assert.AreEqual(ions1[i].charge, ions2[i].charge);
                Assert.AreEqual(ions1[i].conductivity, ions2[i].conductivity);
                Assert.AreEqual(ions1[i].mu, ions2[i].mu);
                Assert.AreEqual(ions1[i].c0, ions2[i].c0);
                Assert.AreEqual(ions1[i].cL, ions2[i].cL);
            }

            Assert.AreEqual(exp1.ToJson(), exp2.ToJson());
        }
    }
}
