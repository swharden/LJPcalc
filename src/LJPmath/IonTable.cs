using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LJPmath
{
    /// <summary>
    /// The IonTable is an IonSet with ion names, charges, and mobilities pre-loaded from a reference file.
    /// </summary>
    public class IonTable : IonSet
    {
        public string filePath { get; private set; }

        public IonTable(string filePath = "IonTable.md")
        {
            filePath = FindFile(filePath);

            if (filePath is null)
                throw new ArgumentException("ion table file could not be found");
            else
                this.filePath = filePath;

            Load(filePath, ignoreDuplicates: true, sort: true);
            Debug.WriteLine($"Loaded {ions.Count} ions from {filePath}");
        }

        private string FindFile(string filePath)
        {
            filePath = System.IO.Path.GetFullPath(filePath);
            string fileName = System.IO.Path.GetFileName(filePath);

            // look at the full given path
            if (System.IO.File.Exists(filePath))
                return filePath;
            else
                Debug.WriteLine($"Ion table not found in: {filePath}");

            // look in the same folder as this EXE
            string exeFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string pathInExeFolder = System.IO.Path.Combine(exeFolder, fileName);
            pathInExeFolder = System.IO.Path.GetFullPath(pathInExeFolder);
            if (System.IO.File.Exists(pathInExeFolder))
                return pathInExeFolder;
            else
                Debug.WriteLine($"Ion table not found in: {pathInExeFolder}");

            // look 4 folders up (developers)
            string pathDev = System.IO.Path.Combine(exeFolder + "/../../../../", fileName);
            pathDev = System.IO.Path.GetFullPath(pathDev);
            if (System.IO.File.Exists(pathDev))
                return pathDev;
            else
                Debug.WriteLine($"Ion table not found in: {pathDev}");

            return null;
        }

        public override string ToString()
        {
            return $"Ion table containing {ions.Count} ions";
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
                    return new Ion(tableIon.name, tableIon.charge, tableIon.conductivity, ion.c0, ion.cL);

            return new Ion(ion);
        }

        public Ion Lookup(string name)
        {
            foreach (Ion tableIon in ions)
            {
                if (string.Compare(name, tableIon.name, StringComparison.OrdinalIgnoreCase) == 0)
                    return tableIon;
                else if (string.Compare(name, tableIon.nameWithCharge, StringComparison.OrdinalIgnoreCase) == 0)
                    return tableIon;
            }

            return new Ion(name, 0, 0, 0, 0);
        }
    }
}
