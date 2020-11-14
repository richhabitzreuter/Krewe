using System;
using System.Threading;
using System.Diagnostics;

namespace Krewe
{
    public class krewe
    {
        private static bool lowPriority = false;

        private static string[] appList = { "devenv", "Discord", "chrome", "brave", "LeagueClient" };

        private static void SetLowPriority(bool reset = false)
        {
            for (int i = 0; i < appList.Length; i++)
            {
                var processes = Process.GetProcessesByName(appList[i]);
                for (int y = 0; y < processes.Length; y++)
                    processes[y].PriorityClass = reset ? ProcessPriorityClass.Normal : ProcessPriorityClass.Idle;
            }
            lowPriority = !reset;
        }

        private static bool CheckProcesses(string processName)
        {
            var processes = Process.GetProcesses();
            for (int i = 0; i < processes.Length; i++)
            {
                if (processes[i].ProcessName == processName)
                {
                    processes[i].PriorityClass = ProcessPriorityClass.High;
                    return true;
                }
            }
            return false;
        }

        private static void Main(string[] args)
        {
            var processName = "League of Legends";
            var sleepDelay = 3000;
            var keepRunning = true;

            try
            { 
                processName = args[0];
                sleepDelay = int.Parse(args[1]) * 1000;
                keepRunning = args[2] == "true";
            }
            catch { }

            while (true)
            {
                if (!CheckProcesses(processName))
                {
                    if (!keepRunning)
                    {
                        SetLowPriority(true);
                        break;
                    }
                    else
                    {
                        if (lowPriority)
                            SetLowPriority(true);
                    }
                }
                else if (keepRunning && !lowPriority)
                    SetLowPriority();
                
                GC.Collect();
                Thread.Sleep(sleepDelay);
            }
        }
    }
}
