using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace TabLimitter
{
    class Program
    {
        List<int> ids = new List<int>();
        List<string> names = new List<string>();

        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            Program pRef = new Program();

            Console.WriteLine("Type the process name of all focused processes. Type \"done\" to stop.");

            string input = "";
            while(!input.Equals("done"))
            {
                input = Console.ReadLine();
                pRef.names.Add(input);
            }

            pRef.LoadProcessIds();
            pRef.MonitorIds();
        }

        Process[] GetProcesses()
        {
            List<Process> processes = new List<Process>();

            foreach (var name in names)
            {
                foreach (var process in Process.GetProcessesByName(name))
                {
                    processes.Add(process);
                }
            }

            return processes.ToArray();
        }

        void LoadProcessIds()
        {
            Console.WriteLine("--------------");

            ids.Clear();

            Console.ForegroundColor = ConsoleColor.Green;

            foreach (var process in GetProcesses())
            {
                Console.WriteLine("loading PID: " + process.Id);
                ids.Add(process.Id);
            }

            Console.ResetColor();
        }

        void MonitorIds()
        {
            while (true)
            {
                Thread.Sleep(500);

                int diff = GetProcesses().Length - ids.Count;

                if (diff < 0)
                    LoadProcessIds();
                else if (diff > 0)
                {
                    Console.WriteLine("--------------");

                    Console.ForegroundColor = ConsoleColor.Red;

                    foreach (var process in GetProcesses())
                    {
                        if (!ids.Contains(process.Id))
                        {
                            Console.WriteLine("killed process with PID: " + process.Id);
                            process.Kill();
                        }
                    }

                    Console.ResetColor();
                }
            }
        }
    }
}
