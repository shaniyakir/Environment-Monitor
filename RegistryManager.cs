using Microsoft.Win32;

namespace TestShani
{
    class RegistryManager
    {
        public void SetRegistryValue(string keyName, string value)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\EnvironmentMonitor"))
                {
                    if (key != null)
                    {
                        key.SetValue(keyName, value);
                    }
                    else
                    {
                        Console.WriteLine("Failed to create or open the registry key.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating or setting registry key: {ex.Message}");
            }
        }


        public void ShowMonitoredProcesses()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\EnvironmentMonitor"))
                {
                    if (key != null)
                    {
                        Console.WriteLine("Monitored Processes:");
                        foreach (string valueName in key.GetValueNames())
                        {
                            string value = key.GetValue(valueName)?.ToString();
                            if (value != null)
                            {
                                Console.WriteLine($"{valueName}: {value}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Registry key not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving monitored processes: {ex.Message}");
            }
        }
    }
}

