using LJPmath;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

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

            // design the HTTP request
            string functionKey = "MiPqBqy0Bv0EYQ1QslBBgyMIX6qeeutZFJ27rJC9H/3ObKooolIfYQ==";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://ljpcalcapi.azurewebsites.net/api/CalculateLJP?code=" + functionKey);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            // add experiment design JSON to body
            using var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            streamWriter.Write(txJson);
            streamWriter.Flush();
            streamWriter.Close();

            // execute the request and get the response
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var rxJson = streamReader.ReadToEnd();
            exp.AddResultsJson(rxJson);

            Console.WriteLine(exp.GetReport());

            Assert.AreEqual(-20.79558643, exp.LjpMillivolts, 0.1);
        }
    }
}
