using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using PCLMock;

namespace WorkoutWotch.UnitTests.Services.State.Mocks
{
    public class BlobCacheMock : MockBase<IBlobCache>, IBlobCache
    {
        public BlobCacheMock(MockBehavior behavior = MockBehavior.Strict) : base(behavior)
        {
        }

        public void Dispose()
        {
            this.Apply(x => x.Dispose());
        }

        public IObservable<Unit> Insert(string key, byte[] data, DateTimeOffset? absoluteExpiration = null)
        {
            return this.Apply(x => x.Insert(key, data, absoluteExpiration));
        }

        public IObservable<byte[]> Get(string key)
        {
            return this.Apply(x => x.Get(key));
        }

        public IObservable<IEnumerable<string>> GetAllKeys()
        {
            return this.Apply(x => x.GetAllKeys());
        }

        public IObservable<DateTimeOffset?> GetCreatedAt(string key)
        {
            return this.Apply(x => x.GetCreatedAt(key));
        }

        public IObservable<Unit> Flush()
        {
            return this.Apply(x => x.Flush());
        }

        public IObservable<Unit> Invalidate(string key)
        {
            return this.Apply(x => x.Invalidate(key));
        }

        public IObservable<Unit> InvalidateAll()
        {
            return this.Apply(x => x.InvalidateAll());
        }

        public IObservable<Unit> Vacuum()
        {
            return this.Apply(x => x.Vacuum());
        }

        public IObservable<Unit> Shutdown => this.Apply(x => x.Shutdown);
        public IScheduler Scheduler => this.Apply(x => x.Scheduler);
    }
}
