using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace DistributedTesting
{
    public class NodeManager
    {


        // Start nodes
        // Stop nodes
        // Check for running nodes
        // Stop browsers that contain "firefox.exe" -foreground"

        public void StopSeleniumBrowsers()
        {
            string procName = "firefox.exe";

            string wmiQuery = string.Format("select CommandLine from Win32_Process where Name='{0}'", procName);

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQuery);
            ManagementObjectCollection retObjectCollection = searcher.Get();

            foreach (ManagementObject retObject in retObjectCollection)
            {
                if (retObject["CommandLine"].ToString().Contains("-foreground"))
                {
                    Process.GetProcessById(Convert.ToInt32(retObject["ProcessID"])).Kill();
                }
            }
        }
        public Boolean HasNodes()
        {
            string procName = "java.exe";

            string wmiQuery = string.Format("select CommandLine from Win32_Process where Name='{0}'", procName);

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQuery);
            ManagementObjectCollection retObjectCollection = searcher.Get();

            foreach (ManagementObject retObject in retObjectCollection)
            {
                if (retObject["CommandLine"].ToString().Contains("-role node"))
                {
                    return true;
                }
            }
            return false;
        }
        public void StopNodes()
        {
            string procName = "java.exe";

            string wmiQuery = string.Format("select CommandLine from Win32_Process where Name='{0}'", procName);
            
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQuery);
            ManagementObjectCollection retObjectCollection = searcher.Get();
            
            foreach (ManagementObject retObject in retObjectCollection)
            {
                if(retObject["CommandLine"].ToString().Contains("-role node"))
                {
                    Process.GetProcessById(Convert.ToInt32(retObject["ProcessID"])).Kill();
                }
            }

        }
        public void StartNode()//StreamWriter sWriter)
        {
            // Starting default stuff for now
            //String command = "java -jar selenium-server-standalone-2.35.0 -role hub";
            
            String command = new NodeCommand().Command;
            String[] output = Cmd.Run(command, true);



            if (String.IsNullOrEmpty(output[0]))
            {
                //sWriter.WriteLine(output[0] + "-" + DateTime.Now);
            }
            if (String.IsNullOrEmpty(output[1]))
            {
                //sWriter.WriteLine(output[1] + "-" + DateTime.Now);
            }
        }

        

       
    }
}
