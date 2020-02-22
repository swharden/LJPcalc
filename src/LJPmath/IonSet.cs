using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LJPmath
{
    /// <summary>
    /// An IonSet contains a List of ions and functions to load/save/search/sort them
    /// </summary>
    public class IonSet
    {
        public readonly List<Ion> ions = new List<Ion>();

        public IonSet()
        {

        }

        public IonSet(List<Ion> ions)
        {
            foreach (Ion sourceIon in ions)
            {
                Ion copiedIon = new Ion(sourceIon);
                this.ions.Add(copiedIon);
            }
        }

        public IonSet(string loadFromThisFilePath)
        {
            if (!System.IO.File.Exists(loadFromThisFilePath))
                throw new ArgumentException("ion set file does not exist");
            Load(loadFromThisFilePath);
        }

        public override string ToString()
        {
            return $"Ion set containing {ions.Count} ions";
        }

        public bool Contains(string name)
        {
            foreach (Ion tableIon in ions)
                if (string.Compare(name, tableIon.name, StringComparison.OrdinalIgnoreCase) == 0)
                    return true;
            return false;
        }

        public string GetTableString(bool prefix = false)
        {
            var txt = new StringBuilder();

            if (prefix)
            {
                Version ver = typeof(IonSet).Assembly.GetName().Version;
                string dateTimeStamp = DateTime.Now.ToString("G");
                txt.AppendLine($"# LJPcalc {ver.Major}.{ver.Minor}.{ver.Revision} custom ion set created {dateTimeStamp}");
                txt.AppendLine();
            }

            //              ################################################################################ 80 characters
            txt.AppendLine(" Name               | Charge | Conductivity (E-4) | C0 (mM)      | CL (mM)      ");
            txt.AppendLine("--------------------|--------|--------------------|--------------|--------------");

            int precision = 7;

            foreach (Ion ion in ions)
            {
                string name = ion.name.PadRight(18);
                string charge = (ion.charge > 0) ? ("+" + ion.charge.ToString()).PadRight(6) : ion.charge.ToString().PadRight(6);
                string conductivity = Math.Round((ion.conductivity * 1E4), precision).ToString().PadRight(18);
                string c0 = Math.Round(ion.c0, precision).ToString().PadRight(12);
                string cL = Math.Round(ion.cL, precision).ToString().PadRight(12);
                txt.AppendLine($" {name} | {charge} | {conductivity} | {c0} | {cL}");
            }

            return txt.ToString().TrimEnd();
        }

        public void Save(string filePath)
        {
            System.IO.File.WriteAllText(filePath, GetTableString(prefix: true));
        }

        public void Load(string filePath, bool ignoreDuplicates = false, bool sort = false)
        {
            ions.Clear();
            foreach (string line in System.IO.File.ReadAllLines(filePath))
            {
                Ion ion = IonFromMarkdownLine(line);
                if (ion != null)
                {
                    if (ignoreDuplicates && Contains(ion.name))
                        continue;

                    ions.Add(ion);
                }
            }

            if (sort)
            {
                var sortedIons = ions.OrderBy(ion => ion.name).ToList();
                ions.Clear();
                ions.AddRange(sortedIons);
            }
        }

        private Ion IonFromMarkdownLine(String line)
        {
            const double K_CONDUCTANCE = 73.5;

            line = line.Trim();

            var parts = line.Split('|');
            bool isThreeItems = (parts.Length == 3);
            bool isFiveItems = (parts.Length == 5);
            bool isThreeOrFiveItems = isThreeItems || isFiveItems;
            if (isThreeOrFiveItems == false)
                return null;

            if (parts[1].StartsWith("---") || parts[1].Contains("Charge"))
                return null;

            string strCond = parts[2];
            strCond = strCond.Replace(" ", "").ToUpper();
            bool isNormalizedToK = false;
            if (strCond.Contains("*K"))
            {
                strCond = strCond.Replace("*K", "");
                isNormalizedToK = true;
            }

            try
            {
                string name = parts[0].Trim();
                int charge = int.Parse(parts[1]);
                double conductance = double.Parse(strCond) * 1E-4;
                if (isNormalizedToK)
                    conductance *= K_CONDUCTANCE;

                double c0 = (parts.Length == 5) ? double.Parse(parts[3]) : 0;
                double cL = (parts.Length == 5) ? double.Parse(parts[4]) : 0;

                Ion ion = new Ion(name, charge, conductance, c0, cL);
                Debug.WriteLine($"Parsed ion: {ion}");
                return ion;
            }
            catch
            {
                Debug.WriteLine($"Could not parse line: {line}");
                return null;
            }
        }
    }
}
