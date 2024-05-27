using System.Collections.Concurrent;
using System.Diagnostics;

namespace TestShani;


class ProcessManager
{
    
    private RegistryManager registryManager;
    private ConcurrentQueue<QueuedAction> actionQueue;

    public ProcessManager(ConcurrentQueue<QueuedAction> actionQueue)
    {
        this.registryManager = new RegistryManager();
        this.actionQueue = actionQueue;
    }

    public void ListAllProcesses()
    {
        var processes = Process.GetProcesses();
        Console.WriteLine("Currently running processes:");
        Console.WriteLine("ID\tName\t\tStart Time\t\tCPU Time");
        foreach (var process in processes)
        {
            try
            {
                Console.WriteLine($"{process.Id}\t{process.ProcessName}\t{process.StartTime}\t{process.TotalProcessorTime}");
            }
            catch (Exception)
            {
                Console.WriteLine($"{process.Id}\t{process.ProcessName}\t<Access Denied>\t<Access Denied>");
            }
        }
    }

    public void ShowProcessDetails()
    {
        Console.Write("Enter Process ID: ");
        if (int.TryParse(Console.ReadLine(), out int processId))
        {
            try
            {
                var process = Process.GetProcessById(processId);
                Console.WriteLine($"ID: {process.Id}");
                Console.WriteLine($"Name: {process.ProcessName}");
                Console.WriteLine($"Start Time: {process.StartTime}");
                Console.WriteLine($"CPU Time: {process.TotalProcessorTime}");
                Console.WriteLine($"Memory Usage: {process.WorkingSet64}");
                Console.WriteLine($"Main Window Title: {process.MainWindowTitle}");

                string keyName = process.ProcessName + DateTime.Now.ToString("yyyyMMdd-HHmmss");
                registryManager.SetRegistryValue(keyName, DateTime.Now.ToString("yyyyMMdd-HHmmss"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving process details: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Invalid Process ID.");
        }
    }

    public async void StartNewProcess()
    {
        Console.Write("Enter executable path: ");
        string path = Console.ReadLine();
        Console.Write("Enter delay in seconds before starting the process: ");
        if (int.TryParse(Console.ReadLine(), out int delaySeconds))
        {
            var queuedAction = new QueuedAction
            {
                ActionType = "Start",
                ExecutablePath = path,
                DelaySeconds = delaySeconds,
                InitiatedAt = DateTime.Now
            };

            actionQueue.Enqueue(queuedAction);

            await Task.Delay(delaySeconds * 1000);
            try
            {
                var process = Process.Start(path);
                if (process != null)
                {
                    string keyName = "Init_" + process.ProcessName + DateTime.Now.ToString("yyyyMMdd-HHmmss");
                    registryManager.SetRegistryValue(keyName, DateTime.Now.ToString("yyyyMMdd-HHmmss"));
  
                }
                else
                {
                    Console.WriteLine("Failed to start the process.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting process: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Invalid delay input.");
        }
    }

    public async void StopRunningProcess()
    {
        Console.Write("Enter Process ID: ");
        if (int.TryParse(Console.ReadLine(), out int processId))
        {
            Console.Write("Enter delay in seconds before stopping the process: ");
            if (int.TryParse(Console.ReadLine(), out int delaySeconds))
            {
                var queuedAction = new QueuedAction
                {
                    ActionType = "Stop",
                    ExecutablePath = processId.ToString(),
                    DelaySeconds = delaySeconds,
                    InitiatedAt = DateTime.Now
                };

                actionQueue.Enqueue(queuedAction);

                await Task.Delay(delaySeconds * 1000);
                try
                {
                    var process = Process.GetProcessById(processId);
                    process.Kill();
                    if (process.HasExited) {
                        string keyName = "Term_" + process.ProcessName + DateTime.Now.ToString("yyyyMMdd-HHmmss");
                        registryManager.SetRegistryValue(keyName, DateTime.Now.ToString("yyyyMMdd-HHmmss"));
                    }
                    else
                    {
                        Console.WriteLine("Failed to kill the process. please try again");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error stopping process: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid delay input.");
            }
        }
        else
        {
            Console.WriteLine("Invalid Process ID.");
        }
    }
}
