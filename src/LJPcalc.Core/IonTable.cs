using System.Diagnostics;

namespace LJPcalc.Core;

/// <summary>
/// The IonTable is an IonSet with ion names, charges, and mobilities pre-loaded from a reference file.
/// </summary>
public class IonTable : IonSet
{
    public string filePath { get; private set; }

    public IonTable(string[] markdownLines)
    {
        Load(markdownLines, ignoreDuplicates: true, sort: true);
        Debug.WriteLine($"Loaded {ions.Count} ions from {filePath}");
    }

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
        filePath = Path.GetFullPath(filePath);
        string fileName = Path.GetFileName(filePath);

        // look at the full given path
        if (File.Exists(filePath))
            return filePath;
        else
            Debug.WriteLine($"Ion table not found in: {filePath}");

        // look in the same folder as this EXE
        var assembly = typeof(Calculate).Assembly;
        var assemblyPath = assembly.Location;
        string exeFolder = Path.GetDirectoryName(assemblyPath);
        string pathInExeFolder = Path.Combine(exeFolder, fileName);
        pathInExeFolder = Path.GetFullPath(pathInExeFolder);
        if (File.Exists(pathInExeFolder))
            return pathInExeFolder;
        else
            Debug.WriteLine($"Ion table not found in: {pathInExeFolder}");

        // look 4 folders up (developers)
        string pathDev = Path.Combine(exeFolder + "/../../../../", fileName);
        pathDev = Path.GetFullPath(pathDev);
        if (File.Exists(pathDev))
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

    public Ion[] Lookup(Ion[] ionList)
    {
        for (int i = 0; i < ionList.Length; i++)
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
