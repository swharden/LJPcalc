using LJPcalc.web.InputModels;
using LJPmath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LJPcalc.web.Services
{
    public class SampleIonSet
    {
        public List<UserIon> Ions;
        public string Title;
        public string Description;
        public double TemperatureC;
        public double LjpMV;

        public static SampleIonSet FromKnownSet(KnownIonSet knownSet) => new SampleIonSet()
        {
            Title = knownSet.name,
            Description = knownSet.details,
            TemperatureC = knownSet.temperatureC,
            LjpMV = knownSet.expectedLjp_mV,
            Ions = knownSet.ions.Select(x => UserIon.FromIon(x)).ToList()
        };
    }
}
