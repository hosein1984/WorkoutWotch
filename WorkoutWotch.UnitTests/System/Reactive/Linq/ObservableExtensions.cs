using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperTrinity;

namespace WorkoutWotch.UnitTests.System.Reactive.Linq
{
    public static class ObservableExtensions
    {
        /// <summary>
        /// Mostly useful for testing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static IObservable<IList<T>> ToListAsync<T>(this IObservable<T> source, TimeSpan? timeout = null)
        {
            source.AssertNotNull(nameof(source));
            return source
                .Timeout(timeout.GetValueOrDefault(TimeSpan.FromSeconds(3)))
                .Buffer(int.MaxValue)
                .FirstAsync();

        }
    }
}
