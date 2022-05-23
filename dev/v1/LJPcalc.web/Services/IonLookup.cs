using System.Linq;
using LJPmath;

namespace LJPcalc.web.Services
{
    public class IonLookup
    {
        public readonly IonTable Table;

        public IonLookup()
        {
            var mdTableBytes = Properties.Resources.IonTable;
            string mdTableString = System.Text.Encoding.Default.GetString(mdTableBytes);
            string[] mdTableLines = mdTableString.Split("\n");
            Table = new IonTable(mdTableLines);
        }

        public string[] GetIonNames() => Table.ions.Select(x => x.name).ToArray();

        public Ion GetIon(string name, double c0 = 0, double cL = 0)
        {
            var ion = Table.Lookup(name);
            ion.c0 = c0;
            ion.cL = cL;
            return ion;
        }
    }
}
