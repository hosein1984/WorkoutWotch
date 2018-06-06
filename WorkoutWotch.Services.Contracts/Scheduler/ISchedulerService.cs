using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutWotch.Services.Contracts.Scheduler
{
    public interface ISchedulerService
    {
        IScheduler DefaultScheduler { get; }
        IScheduler CurrentThreadScheduler { get; }
        IScheduler ImmediateScheduler { get; }
        IScheduler SynchronizationContextScheduler { get; }
        IScheduler TaskPoolScheduler { get; }
    }
}
