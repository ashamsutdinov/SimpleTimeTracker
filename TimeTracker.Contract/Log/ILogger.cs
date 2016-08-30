using System;

namespace TimeTracker.Contract.Log
{
    public interface ILogger
    {
        void Debug(object sender, string message, params object[] parameters);

        void Info(object sender, string message, params object[] parameters);

        void Warning(object sender, string message, params object[] parameters);

        void Error(object sender, string message, params object[] parameters);

        void Error(object sender, Exception ex, string message, params object[] parameters);
    }
}
