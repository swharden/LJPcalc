using LJPmath;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LJPconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var ionSet = new List<Ion>
            {
                new Ion("Zn", 9, 0.0284),
                new Ion("K", 0, 3),
                new Ion("Cl", 18, 3.0568)
            };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);
            DisplayIonSet(ionSet);

            double ljp = Calculate.LjpForIons(ionSet);
            Console.WriteLine($"LJP = {ljp * 1000} mV");

            Exit();
        }

        private static void Exit(bool pauseFirst = true)
        {
            if (pauseFirst)
            {
                Console.WriteLine("\npress ENTER to exit...");
                Console.ReadLine();
            }
            Environment.Exit(0);
        }

        private static void DisplayIonSet(List<Ion> ionSet)
        {
            Console.WriteLine("");
            Console.WriteLine("ION TABLE:");

            int columnWidth = 10;
            Console.Write("Ion".PadRight(10));
            Console.Write("Charge".PadRight(7));
            Console.Write("Cond (E4)".PadRight(columnWidth));
            Console.Write("Mu (E11)".PadRight(columnWidth));
            Console.Write("Phi".PadRight(columnWidth));
            Console.Write("c0".PadRight(columnWidth));
            Console.Write("cL".PadRight(columnWidth));
            Console.WriteLine("");

            foreach (Ion ion in ionSet)
            {
                Console.Write(ion.name.PadRight(10));
                Console.Write(ion.charge.ToString().PadRight(7));
                Console.Write((ion.conductance * 1e4).ToString().PadRight(columnWidth));
                Console.Write(Math.Round(ion.mu / 1E11, 5).ToString().PadRight(columnWidth));
                Console.Write(ion.phi.ToString().PadRight(columnWidth));
                Console.Write(ion.cL.ToString().PadRight(columnWidth));
                Console.Write(ion.c0.ToString().PadRight(columnWidth));
                Console.WriteLine("");
            }

            Console.WriteLine("");
        }
    }
}
