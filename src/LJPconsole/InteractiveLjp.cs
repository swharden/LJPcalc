using LJPmath;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace LJPconsole
{
    public class InteractiveLjp
    {
        readonly List<Ion> ionSet = new List<Ion>();

        public InteractiveLjp()
        {
            EvaluateCommand("help");
            while (true)
            {
                Console.Write("\n>> ");
                string command = Console.ReadLine().Trim();
                EvaluateCommand(command);
            }
        }

        public void EvaluateCommand(string command)
        {
            Debug.WriteLine($"evaluating command: [{command}]");

            int firstSpaceIndex = command.IndexOf(" ");
            if (firstSpaceIndex > 0)
            {
                string action = command.Substring(0, firstSpaceIndex);
                string suffix = command.Substring(firstSpaceIndex + 1);
                Debug.WriteLine($"command is an action ({action}) with suffix ({suffix})");

                if (action.Equals("add", StringComparison.OrdinalIgnoreCase))
                    CommandAdd(suffix);
                else if (action.Equals("remove", StringComparison.OrdinalIgnoreCase))
                    CommandRemove(suffix);
                else
                    CommandUnknown();
            }
            else
            {
                Debug.WriteLine($"command is a single word ({command})");

                if (command.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    Environment.Exit(0);
                else if (command.Equals("quit", StringComparison.OrdinalIgnoreCase))
                    Environment.Exit(0);
                else if (command.Equals("help", StringComparison.OrdinalIgnoreCase))
                    Help();
                else if (command.Equals("?", StringComparison.OrdinalIgnoreCase))
                    Help();
                else if (command.Equals("demo", StringComparison.OrdinalIgnoreCase))
                    Demo();
                else if (command.Equals("clear", StringComparison.OrdinalIgnoreCase))
                    CommandClear();
                else if (command.Equals("add", StringComparison.OrdinalIgnoreCase))
                    CommandRequiresSuffix();
                else if (command.Equals("remove", StringComparison.OrdinalIgnoreCase))
                    CommandRequiresSuffix();
                else if (command.Equals("calc", StringComparison.OrdinalIgnoreCase))
                    CommandCalculate();
                else if (command.Equals("calculate", StringComparison.OrdinalIgnoreCase))
                    CommandCalculate();
                else if (command.Equals("ljp", StringComparison.OrdinalIgnoreCase))
                    CommandCalculate();
                else if (command.Equals("ion", StringComparison.OrdinalIgnoreCase))
                    DisplayIonSet();
                else if (command.Equals("ions", StringComparison.OrdinalIgnoreCase))
                    DisplayIonSet();
                else if (command.Equals("ionset", StringComparison.OrdinalIgnoreCase))
                    DisplayIonSet();
                else if (command.Equals("site", StringComparison.OrdinalIgnoreCase))
                    LaunchSite();
                else if (command.Equals("www", StringComparison.OrdinalIgnoreCase))
                    LaunchSite();
                else
                    CommandUnknown();
            }
        }

        #region show error message then ion table

        private void CommandUnknown()
        {
            Console.WriteLine("Unknown command. Type help for assistance.");
        }

        private void CommandRequiresSuffix()
        {
            Console.WriteLine("This command requires additional information. Type help for assistance.");
        }

        private void CommandArgumentError()
        {
            Console.WriteLine("Command argument error. Type help for assistance.");
        }

        #endregion

        #region modify ion table

        private void CommandAdd(string suffix)
        {
            try
            {
                Ion ion = null;
                string[] parts = suffix.Split(' ');
                if (parts.Length == 3)
                {
                    ion = new Ion(parts[0], double.Parse(parts[1]), double.Parse(parts[2]));
                    IonTable ionTable = new IonTable();
                    ion = ionTable.Lookup(ion);
                }
                else if (parts.Length == 4)
                {
                    ion = new Ion(parts[0], int.Parse(parts[1]), double.Parse(parts[2]), double.Parse(parts[3]), double.Parse(parts[4]));
                }

                if (ion is null)
                    throw new ArgumentException();

                Console.WriteLine($"Adding {ion}");
                ionSet.Add(ion);
            }
            catch
            {
                CommandArgumentError();
            }

        }

        private void CommandRemove(string suffix)
        {
            try
            {
                for (int i = 0; i < ionSet.Count; i++)
                {
                    if (ionSet[i].name.Equals(suffix, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"Removing: {ionSet[i]}");
                        ionSet.RemoveAt(i);
                        return;
                    }
                }
                Console.WriteLine("ERROR: ion not found in the current ion set");
            }
            catch
            {
                CommandArgumentError();
            }
        }

        private void CommandClear()
        {
            Console.WriteLine("Clearing ion set...");
            ionSet.Clear();
        }

        private void Demo()
        {
            Console.WriteLine("Loading demo ion set...");
            EvaluateCommand("clear");
            EvaluateCommand("add Zn 9 0.0284");
            EvaluateCommand("add K 0 3");
            EvaluateCommand("add Cl 18 3.0568");
            EvaluateCommand("ljp");
        }

        #endregion

        #region display state

        public void CommandCalculate()
        {
            Console.WriteLine("calculating...");
            try
            {
                double ljp_V = LJPmath.Calculate.Ljp(ionSet);
                DisplayIonSet();
                Console.WriteLine($"LJP = {ljp_V * 1000} mV");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: LJP cannot be calculated from current ion set.");
                Console.WriteLine();
                Console.WriteLine("Technical details:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                DisplayIonSet();
            }
        }

        public void Help()
        {
            Version ver = typeof(Program).Assembly.GetName().Version;

            Console.WriteLine();
            Console.WriteLine($"  LJPcalc console (version {ver.Major}.{ver.Minor}) by Scott Harden");
            Console.WriteLine();
            Console.WriteLine("  General commands:");
            Console.WriteLine("    exit, quit - exit the program");
            Console.WriteLine("    help, ? - display this message");
            Console.WriteLine("    ions - display the current ion set");
            Console.WriteLine("    demo - populate the ion list with demonstration ion set");
            Console.WriteLine("    site - launch the LJPcalc website");
            Console.WriteLine();
            Console.WriteLine("  Modifying the ion list:");
            Console.WriteLine("    add [ion, c0, cL] - add an ion (using the ion table)");
            Console.WriteLine("    add [ion, charge, conductivity, c0, cL] - add an ion manually");
            Console.WriteLine("    remove [ion] - remove an ion from the list");
            Console.WriteLine("    clear - remove all ions from the list");
            Console.WriteLine();
            Console.WriteLine("  Calculations:");
            Console.WriteLine("    ljp - perform LJP calculation and display result");
        }

        private void DisplayIonSet()
        {
            Console.WriteLine("");
            Console.WriteLine("ION TABLE:");

            int columnWidth = 10;
            Console.Write("Ion".PadRight(10));
            Console.Write("Charge".PadRight(7));
            Console.Write("Cond (E4)".PadRight(columnWidth));
            Console.Write("Mu (E11)".PadRight(columnWidth));
            Console.Write("Phi".PadRight(columnWidth));
            Console.Write("c0 (mM)".PadRight(columnWidth));
            Console.Write("cL (mM)".PadRight(columnWidth));
            Console.WriteLine("");

            foreach (Ion ion in ionSet)
            {
                Console.Write(ion.name.PadRight(10));
                Console.Write(ion.charge.ToString().PadRight(7));
                Console.Write((ion.conductance * 1e4).ToString().PadRight(columnWidth));
                Console.Write(Math.Round(ion.mu / 1E11, 5).ToString().PadRight(columnWidth));
                Console.Write(Math.Round(ion.phi, 5).ToString().PadRight(columnWidth));
                Console.Write(Math.Round(ion.cL, 5).ToString().PadRight(columnWidth));
                Console.Write(Math.Round(ion.c0, 5).ToString().PadRight(columnWidth));
                Console.WriteLine("");
            }

            Console.WriteLine("");
        }

        #endregion

        private void LaunchSite()
        {
            string url = "https://github.com/swharden/LJPcalc";
            Console.WriteLine($"Launching: {url}");
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    Process.Start("xdg-open", url);
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    Process.Start("open", url);
                else
                    throw;
            }
        }
    }
}
