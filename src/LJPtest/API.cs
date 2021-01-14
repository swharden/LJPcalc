using LJPmath;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace LJPtest
{
    class API
    {
        [Test]
        public void Test_HttpApi_Screenshot()
        {
            // use the ion set from screenshot on original JLJP website
            var ionSet = new List<Ion>
            {
                new Ion("Zn", 9, 0.0284),
                new Ion("K", 0, 3),
                new Ion("Cl", 18, 3.0568)
            };
            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            // design an experiment and encode it as JSON
            var exp = new Experiment(ionSet.ToArray(), temperatureC: 25);
            string txJson = exp.ToJson();
            Console.WriteLine(txJson);

            // execute the HTTP request, get the response, and update the experiment
            string functionKey = "MiPqBqy0Bv0EYQ1QslBBgyMIX6qeeutZFJ27rJC9H/3ObKooolIfYQ==";
            string url = "https://ljpcalcapi.azurewebsites.net/api/CalculateLJP?code=" + functionKey;
            var data = new StringContent(txJson, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var response = client.PostAsync(url, data);
            string rxJson = response.Result.Content.ReadAsStringAsync().Result;
            exp.AddResultsJson(rxJson);
            Console.WriteLine(exp.GetReport());

            Assert.AreEqual(-20.79558643, exp.LjpMillivolts, 0.1);
        }
    }
}
