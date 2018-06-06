using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutWotch.Services.Contracts.Logger
{
    public interface ILoggerService
    {
        LogLevel Threshold { get; set; }

        bool IsDebugEnabled { get; }

        bool IsInfoEnabled { get; }

        bool IsPerformanceEnabled { get; }

        bool IsWarningEnabled { get; }

        bool IsErrorEnabled { get; }

        ILogger GetLogger(Type forType);

        ILogger GetLogger(string name);

        /// <summary>
        /// Instead of writing the logs directly 
        /// to the file or console we expose the 
        /// log to the user to decide what to do with them
        /// </summary>
        IObservable<LogEntry> Entries { get; }
    }
}
