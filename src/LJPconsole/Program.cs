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
            var ionTable = new IonTable();

            var ionSet = new List<Ion>
            {
                new Ion(ionTable.Lookup("Zn"), 9, 0.0284),
                new Ion(ionTable.Lookup("K"), 0, 3), // second from last is "X"
                new Ion(ionTable.Lookup("Cl"), 18, 3.0568) // last becomes "last"
            };

            //double calculated_mV = Calculate.SolveAndCalculateLjp(ionSet) * 1000;
            //Console.WriteLine($"LJP: {calculated_mV} mV");

            Console.WriteLine("\npress ENTER to exit...");
            Console.ReadLine();
        }
    }
}
