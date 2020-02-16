﻿using System;
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
        private readonly string filePath;
        private readonly string fileName;

        public IonTable(String csvFilePath = "IonTable.csv")
        {
            if (!System.IO.File.Exists(csvFilePath))
            {
                string baseName = System.IO.Path.GetFileName(csvFilePath);
                string pathFourFoldersUp = "../../../../" + baseName;
                if (System.IO.File.Exists(pathFourFoldersUp))
                {
                    Debug.WriteLine($"copying table from {pathFourFoldersUp}");
                    System.IO.File.Copy(pathFourFoldersUp, baseName);
                }
                else
                {
                    throw new ArgumentException("csv file does not exist");
                }
            }

            filePath = System.IO.Path.GetFullPath(csvFilePath);
            fileName = System.IO.Path.GetFileName(csvFilePath);

            List<Ion> ionsUnsorted = ReadIonsFromFile(csvFilePath);
            ions.AddRange(ionsUnsorted.OrderBy(ion => ion.name).ToList());
            Debug.WriteLine($"Loaded {ions.Count} ions from {fileName}");
        }

        public override string ToString()
        {
            return $"Ion table ({fileName}) containing {ions.Count} ions";
        }

        public bool Contains(string name)
        {
            return (Lookup(name).charge != 0);
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

        private List<Ion> ReadIonsFromFile(String csvFilePath)
        {
            List<Ion> unsortedIons = new List<Ion>();
            String rawText = System.IO.File.ReadAllText(csvFilePath);
            String[] lines = rawText.Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                Ion ion = IonFromLine(lines[i]);
                if (ion != null)
                    unsortedIons.Add(ion);
            }
            return unsortedIons;
        }

        private Ion IonFromLine(String line)
        {
            line = line.Trim();
            if (line.StartsWith("#"))
                return null;
            if (line.Length < 5)
                return null;

            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            String[] parts = CSVParser.Split(line);

            if (parts[1] == "" || parts[2] == "")
                return null;

            try
            {
                Ion ion = new Ion(
                        name: parts[0].Trim().Trim('"'),
                        charge: int.Parse(parts[1]),
                        conductance: double.Parse(parts[2]) * 1E-4,
                        cL: 0,
                        c0: 0
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
