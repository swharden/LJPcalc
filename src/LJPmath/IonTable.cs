using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LJPmath
{
    public class IonTable
    {
        public readonly List<Ion> ions = new List<Ion>();
        private readonly string filePath;
        private readonly string fileName;

        public IonTable(String csvFilePath = "IonTable.csv")
        {
            if (!System.IO.File.Exists(csvFilePath))
                throw new ArgumentException("csv file does not exist");

            filePath = System.IO.Path.GetFullPath(csvFilePath);
            fileName = System.IO.Path.GetFileName(csvFilePath);

            LoadFromFile(csvFilePath);
        }

        public override string ToString()
        {
            return $"Ion table ({fileName}) containing {ions.Count} ions";
        }

        public bool Contains(string name)
        {
            return (Lookup(name).charge != 0);
        }

        public Ion Lookup(string name)
        {
            foreach (Ion ion in ions)
            {
                if (string.Compare(name, ion.name, StringComparison.OrdinalIgnoreCase) == 0)
                    return ion;
                if (string.Compare(name, ion.description, StringComparison.OrdinalIgnoreCase) == 0)
                    return ion;
            }

            return new Ion(name, name, 0, 0);
        }

        private void LoadFromFile(String csvFilePath)
        {
            ions.Clear();
            String rawText = System.IO.File.ReadAllText(csvFilePath);
            String[] lines = rawText.Split('\n');
            for (int i=1; i<lines.Length; i++)
            {
                Ion ion = IonFromLine(lines[i]);
                if (ion != null)
                    ions.Add(ion);
            }
            Debug.WriteLine($"Loaded {ions.Count} ions from {System.IO.Path.GetFileName(csvFilePath)}");
        }

        private Ion IonFromLine(String line)
        {
            line = line.Trim();
            if (line.StartsWith("#"))
                return null;

            try
            {
                string[] parts = line.Split(',');
                Ion ion = new Ion(
                        name: parts[0].Trim(),
                        description: parts[1].Trim(),
                        charge: int.Parse(parts[2]),
                        conductance: double.Parse(parts[3])
                    );
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
