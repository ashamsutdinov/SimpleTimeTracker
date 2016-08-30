using System;
using TimeTracker.Contract.Log;

namespace TimeTracker.RestService.Log
{
    public class ConsoleLogger : 
        ILogger
    {
        public void Debug(object sender, string message, params object[] parameters)
        {
            var text = string.Format("{0}: {1}", sender, string.Format(message, parameters));
            WriteConsoleMessage(text, ConsoleColor.Green);
        }

        public void Info(object sender, string message, params object[] parameters)
        {
            var text = string.Format("{0}: {1}", sender, string.Format(message, parameters));
            WriteConsoleMessage(text, ConsoleColor.Blue);
        }

        public void Warning(object sender, string message, params object[] parameters)
        {
            var text = string.Format("{0}: {1}", sender, string.Format(message, parameters));
            WriteConsoleMessage(text, ConsoleColor.Yellow);
        }

        public void Error(object sender, string message, params object[] parameters)
        {
            var text = string.Format("{0}: {1}", sender, string.Format(message, parameters));
            WriteConsoleMessage(text, ConsoleColor.Red);
        }

        public void Error(object sender, Exception ex, string message, params object[] parameters)
        {
            var text1 = string.Format("{0}: {1}", sender, string.Format(message, parameters));
            WriteConsoleMessage(text1, ConsoleColor.Red);
            WriteConsoleMessage("EXCEPTION TRACE BEGIN", ConsoleColor.Red);
            var text2 = string.Format("Exception details: {0}", ex);
            if (ex != null)
            {
                Console.WriteLine();
                WriteConsoleMessage(text2, ConsoleColor.Red);
                if (ex.InnerException != null)
                {
                    Console.WriteLine();
                    var text3 = string.Format("Inner exception: {0}", ex.InnerException);
                    WriteConsoleMessage(text3, ConsoleColor.Red);
                }
            }
            WriteConsoleMessage("EXCEPTION TRACE END", ConsoleColor.Red);
        }

        private void WriteConsoleMessage(string message, ConsoleColor color)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }
    }
}
