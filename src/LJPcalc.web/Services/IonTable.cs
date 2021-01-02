using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LJPcalc.web.Services
{
    public static class IonTable
    {
        public static string[] GetIonNames()
        {
            var mdTableBytes = Properties.Resources.IonTable;
            string mdTable = System.Text.Encoding.Default.GetString(mdTableBytes);
            var table = new LJPmath.IonTable(mdTable.Split("\n"));
            return table.ions.Select(x => x.name).ToArray();
        }

        public static LJPmath.Ion GetIon(string name, double c0 = 0, double cL = 0)
        {
            var mdTableBytes = Properties.Resources.IonTable;
            string mdTable = System.Text.Encoding.Default.GetString(mdTableBytes);
            var table = new LJPmath.IonTable(mdTable.Split("\n"));
            var ion = table.Lookup(name);
            ion.c0 = c0;
            ion.cL = cL;
            return ion;
        }
    }
}
