using System;

namespace WorkoutWotch.Services.Contracts.Logger
{
    public interface ILogger
    {
        string Name { get; }
        /// <summary>
        /// Allows us to skip in some scenarios what would be an expensive log
        /// </summary>
        bool IsDebugEnabled { get; }

        bool IsInfoEnabled { get; }

        bool IsPerformanceEnabled { get; }

        bool IsWarningEnabled { get; }

        bool IsErrorEnabled { get; }

        void Debug(string message);

        void Debug(string format, params object[] args);

        void Debug(Exception exception, string format, params object[] args);

        void Info(string message);

        void Info(string format, params object[] args);

        void Info(Exception exception, string format, params object[] args);

        void Warning(string message);

        void Warning(string format, params object[] args);

        void Warning(Exception exception, string format, params object[] args);

        void Error(string message);

        void Error(string format, params object[] args);

        void Error(Exception exception, string format, params object[] args);


        /// <summary>
        /// For example this is used in a using statement to log something. when we exit the using statement if log the time it took for example.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        IDisposable Performance(string message);

        IDisposable Performance(string format, params object[] args);
    }
}