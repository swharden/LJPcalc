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
        public IonTable(String filePath = "IonTable.md")
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

            Load(filePath, ignoreDuplicates: true, sort: true);
            Debug.WriteLine($"Loaded {ions.Count} ions from {filePath}");
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
