using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LJPmath
{
    public class IonTable
    {
        public readonly List<Ion> ions = new List<Ion>();

        public IonTable(String filePath = "IonTable.md", bool sortAlphabetically = true)
        {
            if (!System.IO.File.Exists(filePath))
            {
                string baseName = System.IO.Path.GetFileName(filePath);
                string pathFourFoldersUp = "../../../../" + baseName;
                if (System.IO.File.Exists(pathFourFoldersUp))
                    filePath = pathFourFoldersUp;
                else
                    throw new ArgumentException("ion table file does not exist");
            }

            ions = ReadIonsFromFile(filePath);
            if (sortAlphabetically)
                Sort();
            Debug.WriteLine($"Loaded {ions.Count} ions from {filePath}");
        }

        private void Sort()
        {
            var sortedIons = ions.OrderBy(ion => ion.name).ToList();
            ions.Clear();
            ions.AddRange(sortedIons);
        }

        public override string ToString()
        {
            return $"Ion table containing {ions.Count} ions";
        }

        public bool Contains(string name)
        {
            foreach (Ion tableIon in ions)
                if (string.Compare(name, tableIon.name, StringComparison.OrdinalIgnoreCase) == 0)
                    return true;
            return false;
        }

        public List<Ion> Lookup(List<Ion> ionList)
        {
            for (int i = 0; i < ionList.Count; i++)
                ionList[i] = Lookup(ionList[i]);
            return ionList;
        }

        public Ion Lookup(Ion ion)
        {
            foreach (Ion tableIon in ions)
                if (string.Compare(ion.name, tableIon.name, StringComparison.OrdinalIgnoreCase) == 0)
                    return new Ion(tableIon.name, tableIon.charge, tableIon.conductance, ion.c0, ion.cL);

            return new Ion(ion);
        }

        public Ion Lookup(string name)
        {
            foreach (Ion tableIon in ions)
                if (string.Compare(name, tableIon.name, StringComparison.OrdinalIgnoreCase) == 0)
                    return tableIon;

            return new Ion(name, 0, 0, 0, 0);
        }

        private List<Ion> ReadIonsFromFile(String ionFilePath)
        {
            List<Ion> unsortedIons = new List<Ion>();
            String rawText = System.IO.File.ReadAllText(ionFilePath);
            String[] lines = rawText.Split('\n');
            List<string> ionsSeen = new List<string>();
            for (int i = 1; i < lines.Length; i++)
            {
                Ion ion = IonFromMarkdownLine(lines[i]);
                if (ion != null && !ionsSeen.Contains(ion.name))
                {
                    ionsSeen.Add(ion.name);
                    unsortedIons.Add(ion);
                }
            }
            return unsortedIons;
        }

        public List<Ion> GetDuplicates()
        {
            var duplicateNames = new List<string>();

            var seenNames = new List<string>();
            foreach (Ion ion in ions)
                if (seenNames.Contains(ion.name))
                    duplicateNames.Add(ion.name);
                else
                    seenNames.Add(ion.name);

            var duplicateIons = new List<Ion>();
            foreach (Ion ion in ions)
                if (duplicateNames.Contains(ion.name))
                    duplicateIons.Add(ion);

            return duplicateIons;
        }

        [Obsolete]
        private Ion IonFromCsvLine(String line)
        {
            line = line.Replace("\t", "");
            line = line.Trim();
            if (line.StartsWith("#"))
                return null;
            if (line.Length < 5)
                return null;

            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            String[] parts = CSVParser.Split(line);

            if (parts.Length != 3)
                Console.WriteLine($"WARNING improperly formatted ion table line:\n{line}");

            if (parts[1] == "" || parts[2] == "")
                return null;

            const double K_CONDUCTANCE = 73.5;

            try
            {
                string name = parts[0].Trim().Trim('"');
                int charge = int.Parse(parts[1]);

                string conductanceString = parts[2].Replace(" ", "");
                bool isNormalizedToK = (conductanceString.EndsWith("*K", StringComparison.OrdinalIgnoreCase));
                conductanceString = conductanceString.Replace("*K", "");
                double conductance = double.Parse(conductanceString) * 1E-4;
                if (isNormalizedToK)
                    conductance *= K_CONDUCTANCE;

                Ion ion = new Ion(name, charge, conductance, cL: 0, c0: 0);
                return ion;
            }
            catch
            {
                Debug.WriteLine($"Could not parse line: {line}");
                return null;
            }
        }

        private Ion IonFromMarkdownLine(String line)
        {
            const double K_CONDUCTANCE = 73.5;

            var parts = line.Split('|');
            if (parts.Length != 3)
                return null;

            if (parts[1] == "---" || parts[1] == "Charge")
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
                Ion ion = new Ion(name, charge, conductance, cL: 0, c0: 0);
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
