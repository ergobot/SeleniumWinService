using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DistributedTesting
{
    public partial class DistributedTestingService : ServiceBase
    {
        Timer timer;
        NodeManager nodeManager;
        
        public DistributedTestingService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            FileStream fs = new FileStream(@"c:\SystemActiveTimeInformation.txt",
            FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sWriter = new StreamWriter(fs);
            sWriter.BaseStream.Seek(0, SeekOrigin.End);
            sWriter.WriteLine("=====================================================================================");
            sWriter.WriteLine("System Turn On Time: \t " + DateTime.Now);
            sWriter.Flush();
            sWriter.Close();


            // Non-logging stuff
            nodeManager = new NodeManager();
        }
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            FileStream fs = new FileStream(@"c:\SystemActiveTimeInformation.txt",
            FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sWriter = new StreamWriter(fs);
            sWriter.BaseStream.Seek(0, SeekOrigin.End);
            switch (changeDescription.Reason)
            {
                case SessionChangeReason.SessionLock: 
                    
                    // Log
                    sWriter.WriteLine("System Locked Time: \t" + DateTime.Now); 
                    
                    // Make the timer
                    this.timer = new Timer(120000D); // 120,000 milliseconds (2 mins)
                    this.timer.AutoReset = true;
                    this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
                    
                    // Start the clock
                    this.timer.Start();

                    break;
                case SessionChangeReason.SessionUnlock:
                    // Log
                    sWriter.WriteLine("System Unlocked Time: \t " + DateTime.Now);

                    // Action
                    nodeManager.StopNodes();
                    nodeManager.StopSeleniumBrowsers();

                    if (this.timer != null)
                    {
                        this.timer.Stop();
                        this.timer = null;
                    }

                    break;
                default:
                    break;
            }
            sWriter.Flush();
            sWriter.Close();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            nodeManager.StartNode();
        }

        protected override void OnStop()
        {

            FileStream fs = new FileStream(@"c:\SystemActiveTimeInformation.txt",
            FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sWriter = new StreamWriter(fs);
            sWriter.BaseStream.Seek(0, SeekOrigin.End);
            sWriter.WriteLine("System Turn Off Time: \n " + DateTime.Now);
            sWriter.Flush();
            sWriter.Close();
        }
    }
}
