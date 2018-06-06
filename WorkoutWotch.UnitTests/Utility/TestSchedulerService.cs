using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using WorkoutWotch.Services.Contracts.Scheduler;

namespace WorkoutWotch.UnitTests.Utility
{
    public sealed class TestSchedulerService: TestScheduler, ISchedulerService
    {
        public IScheduler DefaultScheduler => this;
        public IScheduler CurrentThreadScheduler => this;
        public IScheduler ImmediateScheduler => this;
        public IScheduler SynchronizationContextScheduler => this;
        public IScheduler TaskPoolScheduler => this;

        public IDisposable Pump()
        {
            return Pump(TimeSpan.FromMilliseconds(10));
        }

        /// <summary>
        /// Useful hack to allow test to automatically pump any scheduled items
        /// </summary>
        /// <param name="frequency"></param>
        /// <returns></returns>
        private IDisposable Pump(TimeSpan frequency)
        {
            return Observable.Timer(TimeSpan.Zero, frequency).Subscribe(_ => Start());
        }
    }
}
