using System.Collections.Concurrent;


namespace TestShani
{
    class Menu
    {
        private ProcessManager processManager;
        private RegistryManager registryManager;
        private ConcurrentQueue<QueuedAction> actionQueue;

        public Menu()
        {
            actionQueue = new ConcurrentQueue<QueuedAction>();
            processManager = new ProcessManager(actionQueue);
            registryManager = new RegistryManager();
        }

        public void Show()
        {
            while (true)
            {
                ShowMenu();
                string input = Console.ReadLine();
                Console.Clear();

                switch (input)
                {
                    case "1":
                        processManager.ListAllProcesses();
                        break;
                    case "2":
                        processManager.ShowProcessDetails();
                        break;
                    case "3":
                        processManager.StartNewProcess();
                        break;
                    case "4":
                        processManager.StopRunningProcess();
                        break;
                    case "5":
                        registryManager.ShowMonitoredProcesses();
                        break;
                    case "6":
                        ShowQueuedActions();
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("Process Monitor");
            Console.WriteLine("1. List all processes");
            Console.WriteLine("2. Show details of a specific process");
            Console.WriteLine("3. Start a new process");
            Console.WriteLine("4. Stop a running process");
            Console.WriteLine("5. Show monitored processes");
            Console.WriteLine("6. Show queued actions");
            Console.WriteLine("7. Exit");
            Console.Write("Select an option: ");
        }

        private void ShowQueuedActions()
        {
            var pendingActions = actionQueue.Where(action => action.IsPending()).ToList();

            if (pendingActions.Count == 0)
            {
                Console.WriteLine("No pending actions.");
            }
            else
            {
                Console.WriteLine("Pending Actions:");
                foreach (var action in pendingActions)
                {
                    Console.WriteLine(action);
                }
            }
        }
    }
}
