using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HelperTrinity;
using WorkoutWotch.Services.Contracts.Logger;
using WorkoutWotch.Utility;

namespace WorkoutWotch.Services.Logger
{
    public class LoggerService : ILoggerService
    {
        public LoggerService()
        {
            _entries = new Subject<LogEntry>();
        }

        private bool IsLevelEnabled(LogLevel level)
            => Threshold <= level;

        private readonly Subject<LogEntry> _entries;
        public LogLevel Threshold { get; set; }
        public bool IsDebugEnabled => IsLevelEnabled(LogLevel.Debug);
        public bool IsInfoEnabled => IsLevelEnabled(LogLevel.Info);
        public bool IsPerformanceEnabled => IsLevelEnabled(LogLevel.Performance);
        public bool IsWarningEnabled => IsLevelEnabled(LogLevel.Warning);
        public bool IsErrorEnabled => IsLevelEnabled(LogLevel.Error);

        public ILogger GetLogger(Type forType)
        {
            forType.AssertNotNull(nameof(forType));
            return GetLogger(forType.FullName);
        }

        public ILogger GetLogger(string name)
        {
            name.AssertNotNull(nameof(name));

            return new Logger(this, name);
        }

        public IObservable<LogEntry> Entries => _entries.AsObservable();

        private sealed class Logger : ILogger
        {
            private readonly LoggerService _owner;
            private readonly string _name;

            public Logger(LoggerService owner, string name)
            {
                _owner = owner;
                _name = name;
            }

            public string Name => _name;
            public bool IsDebugEnabled => _owner.IsDebugEnabled;
            public bool IsInfoEnabled => _owner.IsInfoEnabled;
            public bool IsPerformanceEnabled => _owner.IsPerformanceEnabled;
            public bool IsWarningEnabled => _owner.IsWarningEnabled;
            public bool IsErrorEnabled => _owner.IsErrorEnabled;



            public void Debug(string message)
            {
                this.Log(LogLevel.Debug, message);
            }



            public void Debug(string format, params object[] args)
            {
                Log(LogLevel.Debug, format, args);
            }

            public void Debug(Exception exception, string format, params object[] args)
            {

                Log(LogLevel.Debug, exception, format, args);
            }

            public void Info(string message)
            {
                this.Log(LogLevel.Info, message);
            }

            public void Info(string format, params object[] args)
            {

                Log(LogLevel.Info, format, args);
            }

            public void Info(Exception exception, string format, params object[] args)
            {

                Log(LogLevel.Info, exception, format, args);
            }

            public void Warning(string message)
            {

                this.Log(LogLevel.Warning, message);
            }

            public void Warning(string format, params object[] args)
            {
                Log(LogLevel.Warning, format, args);
            }

            public void Warning(Exception exception, string format, params object[] args)
            {

                Log(LogLevel.Warning, exception, format, args);
            }

            public void Error(string message)
            {
                Log(LogLevel.Error, message);
            }

            public void Error(string format, params object[] args)
            {

                Log(LogLevel.Error, format, args);
            }

            public void Error(Exception exception, string format, params object[] args)
            {

                Log(LogLevel.Error, exception, format, args);
            }

            public IDisposable Performance(string message)
            {
                message.AssertNotNull(nameof(message));

                if (!IsPerformanceEnabled)
                {
                    return Disposable.Empty;
                }

                return new PerfBlock(this, message);
            }

            private void Log(LogLevel level, string format, params object[] args)
            {
                format.AssertNotNull(nameof(format));
                args.AssertNotNull(nameof(format));

                if (!_owner.IsLevelEnabled(level))
                {
                    return;
                }

                var message = string.Format(CultureInfo.InvariantCulture, format, args);
                Log(level, message);
            }

            private void Log(LogLevel level, Exception exception, string format, params object[] args)
            {
                exception.AssertNotNull(nameof(exception));
                format.AssertNotNull(nameof(format));
                args.AssertNotNull(nameof(format));

                if (!_owner.IsLevelEnabled(level))
                {
                    return;
                }

                var message = string.Format(CultureInfo.InvariantCulture, format, args) + " : " + exception.ToString();
                Log(level, message);
            }

            public IDisposable Performance(string format, params object[] args)
            {
                format.AssertNotNull(nameof(format));
                args.AssertNotNull(nameof(args));
                if (!IsPerformanceEnabled)
                {
                    return Disposable.Empty;
                }

                var message = String.Format(CultureInfo.InvariantCulture, format, args);
                return new PerfBlock(this, message);
            }


            private void Log(LogLevel level, string message)
            {
                message.AssertNotNull(nameof(message));
                if (!_owner.IsLevelEnabled(level))
                {
                    return;
                }

                var entry = new LogEntry(DateTime.UtcNow, Name, level, Environment.CurrentManagedThreadId, message);
                _owner._entries.OnNext(entry);
            }

            private sealed class PerfBlock : DisposableBase
            {
                private readonly Logger _owner;
                private readonly string _message;
                private readonly Stopwatch _stopwatch;

                public PerfBlock(Logger owner, string message)
                {
                    _owner = owner;
                    _message = message;
                    _stopwatch = Stopwatch.StartNew();
                }

                protected override void Dispose(bool disposing)
                {
                    base.Dispose(disposing);

                    if (disposing)
                    {
                        _stopwatch.Stop();
                        this._owner.Log(LogLevel.Performance, "{0} [{1} ({2}ms)]", _message, this._stopwatch.Elapsed,
                            this._stopwatch.ElapsedMilliseconds);
                    }
                }
            }
        }
    }
}
