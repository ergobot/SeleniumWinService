using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedTesting
{
    public class NodeCommand
    {
        public String Command { get; set; }

        public NodeCommand()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Properties.Settings.Default.javaCommand);
            sb.Append(" " + Properties.Settings.Default.jarFileLocation);
            sb.Append(Properties.Settings.Default.jarFileName);
            sb.Append(" " + Properties.Settings.Default.defaultNode);
            sb.Append(" " + Properties.Settings.Default.hubAddress);

            Command = sb.ToString();
            //Console.Out.WriteLine(Command);
        }

    }
}
