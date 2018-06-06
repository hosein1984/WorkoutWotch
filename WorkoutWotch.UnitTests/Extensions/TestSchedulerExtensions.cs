using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using HelperTrinity;
using Microsoft.Reactive.Testing;

namespace WorkoutWotch.UnitTests.Extensions
{
    public static class TestSchedulerExtensions
    {
        public static void AdvanceBy(this TestScheduler @this, TimeSpan time)
        {
            @this.AssertNotNull(nameof(@this));
            @this.AdvanceBy(time.Ticks);
        }

        public static void AdvanceTo(this TestScheduler @this, DateTime dateTime)
        {
            @this.AssertNotNull(nameof(@this));
            @this.AdvanceTo(dateTime.Ticks);
        }

        public static void ScheduleRelative(this TestScheduler @this, TimeSpan dueTime, Action action)
        {
            @this.AssertNotNull(nameof(@this));
            @this.ScheduleRelative(dueTime.Ticks, action);
        }

        public static void ScheduleRelative<TState>(this TestScheduler @this, TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            @this.AssertNotNull(nameof(@this));
            @this.ScheduleRelative(state, dueTime.Ticks, action);
        }
    }
}
