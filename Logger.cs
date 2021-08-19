using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAmmoManager
{
    /// <summary>
    /// Static logger class that allows direct logging of anything to a text file
    /// </summary>
    public static class Logger
    {
        public static void Log(string logFileName, object message)
        {
            File.AppendAllText($"scripts\\SimpleAmmoManager\\{logFileName}.log", DateTime.Now + " : " + message + Environment.NewLine);
        }

        public static void Clear(string logFileName)
        {
            File.Delete($"scripts\\SimpleAmmoManager\\{logFileName}.log");
        }
    }
}
