using System;
using System.Reactive.Linq;
using PCLMock;

namespace WorkoutWotch.Services.Contracts.Logger.Mocks
{
    public class LoggerServiceMock : MockBase<ILoggerService>, ILoggerService
    {
        public LoggerServiceMock(MockBehavior behavior = MockBehavior.Strict) : base(behavior)
        {
            if (behavior == MockBehavior.Loose)
            {
                this.When(x=> x.GetLogger(It.IsAny<Type>())).Return(new LoggerMock(behavior));
                this.When(x=> x.GetLogger(It.IsAny<string>())).Return(new LoggerMock(behavior));
                this.When(x=> x.Entries).Return(Observable.Empty<LogEntry>());
            }
        }

        public LogLevel Threshold
        {
            get { return this.Apply(x => x.Threshold); }
            set { this.ApplyPropertySet(x => x.Threshold, value); }
        }

        public bool IsDebugEnabled => this.Apply(x => x.IsDebugEnabled);
        public bool IsInfoEnabled => this.Apply(x => x.IsInfoEnabled);
        public bool IsPerformanceEnabled => this.Apply(x => x.IsPerformanceEnabled);
        public bool IsWarningEnabled => this.Apply(x => x.IsWarningEnabled);
        public bool IsErrorEnabled => this.Apply(x => x.IsErrorEnabled);
        public ILogger GetLogger(Type forType)
        {
            return this.Apply(x => x.GetLogger(forType));
        }

        public ILogger GetLogger(string name)
        {
            return this.Apply(x => x.GetLogger(name));
        }

        public IObservable<LogEntry> Entries => this.Apply(x => x.Entries);
    }
}
