using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LJPconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            LJPmath.IonTable ionTable = new LJPmath.IonTable();
            Debug.WriteLine(ionTable);
            foreach (LJPmath.Ion ion in ionTable.ions)
                Debug.WriteLine(ion);
            Debug.WriteLine(ionTable.Lookup("hepes"));
            Debug.WriteLine(ionTable.Lookup("fake"));
        }
    }
}
