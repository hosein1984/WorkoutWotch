using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkoutWotch.Services.Contracts.Scheduler;

namespace WorkoutWotch.Services.Scheduler
{
    public sealed class SchedulerService : ISchedulerService
    {
        public SchedulerService()
        {
            SynchronizationContextScheduler = new SynchronizationContextScheduler(SynchronizationContext.Current);
        }
        public IScheduler DefaultScheduler => System.Reactive.Concurrency.DefaultScheduler.Instance;
        public IScheduler CurrentThreadScheduler => System.Reactive.Concurrency.CurrentThreadScheduler.Instance;
        public IScheduler ImmediateScheduler => System.Reactive.Concurrency.ImmediateScheduler.Instance;
        public IScheduler SynchronizationContextScheduler { get; }
        public IScheduler TaskPoolScheduler => System.Reactive.Concurrency.TaskPoolScheduler.Default;

    }
}
