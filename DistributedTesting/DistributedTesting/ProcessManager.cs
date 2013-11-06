using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace DistributedTesting
{
    public static class ProcessManager
    {
        public static List<String> GetProcessByName(String processName)
        {
            List<String> processes = new List<String>();
            string wmiQuery = string.Format("select CommandLine from Win32_Process where Name='{0}'", processName);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQuery);
            ManagementObjectCollection retObjectCollection = searcher.Get();
            foreach (ManagementObject retObject in retObjectCollection)
            {

                processes.Add(retObject["CommandLine"].ToString());
            }
            return processes;
        }
    }
}
