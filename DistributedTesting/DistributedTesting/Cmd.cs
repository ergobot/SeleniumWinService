using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedTesting
{
    public static class Cmd
    {
        public static string[] Run(string command, bool output)
        {
            /*
             *  New array of two strings.
             *  string[0] is the error message.
             *  string[1] is the output message.
             */
            string[] message = new string[2];

            // ProcessStartInfo allows better control over
            // the soon to executed process
            ProcessStartInfo info = new ProcessStartInfo();

            // Input to the process is going to come from the Streamwriter
            info.RedirectStandardInput = true;

            // Output from the process is going to be put into message[1]
            info.RedirectStandardOutput = true;

            // Error, if any, from the process is going to be put into message[0]
            info.RedirectStandardError = true;

            // This must be set to false
            info.UseShellExecute = false;

            // We want to open the command line
            info.FileName = "cmd.exe";

            // We don't want to see a command line window
            info.CreateNoWindow = true;

            // Instantiate a Process object
            Process proc = new Process();



            // Set the Process object's start info to the above StartProcessInfo
            proc.StartInfo = info;

            // Start the process
            proc.Start();



            // The stream writer is replacing the keyboard as the input
            using (StreamWriter writer = proc.StandardInput)
            {
                // If the streamwriter is able to write
                if (writer.BaseStream.CanWrite)
                {
                    // Write the command that was passed into the method
                    writer.WriteLine(command);
                    // Exit the command window
                    writer.WriteLine("exit");
                }
                // close the StreamWriter
                writer.Close();
            }

            // Get any Error's that may exist
            message[0] = proc.StandardError.ReadToEnd();

            // If the output flag was set to true
            if (output)
            {
                // Get the output from the command line
                message[1] = proc.StandardOutput.ReadToEnd();
            }

            // close the process
            proc.Close();

            // return the any error/output
            return message;
        }

        public static string[] Run(string[] command, bool output)
        {
            string[] message = new string[2];

            ProcessStartInfo info = new ProcessStartInfo();

            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;

            info.UseShellExecute = false;
            info.FileName = "cmd.exe";
            info.CreateNoWindow = true;

            Process proc = new Process();
            proc.StartInfo = info;
            proc.Start();

            using (StreamWriter writer = proc.StandardInput)
            {
                if (writer.BaseStream.CanWrite)
                {
                    foreach (string q in command)
                    {
                        writer.WriteLine(q);
                    }
                    writer.WriteLine("exit");
                }
            }

            message[0] = proc.StandardError.ReadToEnd();

            if (output)
            {
                message[1] = proc.StandardOutput.ReadToEnd();
            }

            return message;
        }





    }
}
