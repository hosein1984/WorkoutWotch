using System;
using System.Reactive.Disposables;
using PCLMock;

namespace WorkoutWotch.Services.Contracts.Logger.Mocks
{
    public class LoggerMock : MockBase<ILogger>, ILogger
    {
        public LoggerMock(MockBehavior behavior = MockBehavior.Strict) : base(behavior)
        {
            if (behavior == MockBehavior.Loose)
            {
                this.When(x => x.Performance(It.IsAny<string>())).Return(Disposable.Empty);
                this.When(x => x.Performance(It.IsAny<string>(),It.IsAny<object[]>())).Return(Disposable.Empty);
            }
        }

        public string Name => this.Apply(x => x.Name);
        public bool IsDebugEnabled => this.Apply(x => x.IsDebugEnabled);
        public bool IsInfoEnabled => this.Apply(x => x.IsInfoEnabled);
        public bool IsPerformanceEnabled => this.Apply(x => x.IsPerformanceEnabled);
        public bool IsWarningEnabled => this.Apply(x => x.IsWarningEnabled);
        public bool IsErrorEnabled => this.Apply(x => x.IsErrorEnabled);
        public void Debug(string message)
        {
            Apply(x => x.Debug(message));
        }

        public void Debug(string format, params object[] args)
        {
            Apply(x => x.Debug(format,args));

        }

        public void Debug(Exception exception, string format, params object[] args)
        {
            Apply(x => x.Debug(exception,format,args));
        }

        public void Info(string message)
        {
            Apply(x => x.Info(message));
        }

        public void Info(string format, params object[] args)
        {
            Apply(x => x.Info(format,args));
        }

        public void Info(Exception exception, string format, params object[] args)
        {
            Apply(x => x.Info(exception,format, args));
        }

        public void Warning(string message)
        {
            Apply(x => x.Warning(message));
        }

        public void Warning(string format, params object[] args)
        {
            Apply(x => x.Warning(format,args));
        }

        public void Warning(Exception exception, string format, params object[] args)
        {
            Apply(x => x.Warning(exception, format, args));
        }

        public void Error(string message)
        {
            Apply(x => x.Error(message));
        }

        public void Error(string format, params object[] args)
        {
            Apply(x => x.Error(format, args));
        }

        public void Error(Exception exception, string format, params object[] args)
        {
            Apply(x => x.Error(exception, format, args));
        }

        public IDisposable Performance(string message)
        {
            return Apply(x => x.Performance(message));
        }

        public IDisposable Performance(string format, params object[] args)
        {
            return Apply(x => x.Performance(format,args));
        }
    }
}
