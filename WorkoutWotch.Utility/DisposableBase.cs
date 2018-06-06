using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using HelperTrinity;

namespace WorkoutWotch.Utility
{
    /// <summary>
    /// Ensure only one call against your disposal object is ever made.
    /// So if dozens of threads try to dispose of an object at once, it will only allow one through
    /// </summary>
    public abstract class DisposableBase : Object, IDisposable
    {
        private const int DisposablNotStarted = 0;
        private const int DisposablStarted = 1;
        private const int DisposablCompleted = 2;

        // see the constants defined aove for valid values
        private int _disposeStage;

#if DEBUG
        ~DisposableBase()
        {
            var message = string.Format(CultureInfo.InvariantCulture,
                "Failed to proactively dispose of object, so it is being finalized {0}", ObjectName);
            Debug.Assert(false, message);
            this.Dispose(false);
        }
#endif

        public event EventHandler Disposing;

        protected bool IsDisposing
            => Interlocked.CompareExchange(ref this._disposeStage, DisposablStarted, DisposablStarted) ==
               DisposablStarted;

        protected bool IsDisposed
            => Interlocked.CompareExchange(ref this._disposeStage, DisposablCompleted, DisposablCompleted) ==
               DisposablCompleted;

        protected bool IsDisposedOrDisposing
            => Interlocked.CompareExchange(ref this._disposeStage, DisposablNotStarted, DisposablNotStarted) ==
               DisposablNotStarted;

        protected virtual string ObjectName => this.GetType().FullName;


        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref this._disposeStage, DisposablStarted, DisposablNotStarted) !=
                DisposablNotStarted)
            {
                return;
            }

            this.OnDisposing();
            this.Disposing = null;

            this.Dispose(true);
            this.MarkAsDisposed();
        }

        protected void VerifyNotDisposing()
        {
            if (this.IsDisposing)
            {
                throw new ObjectDisposedException(this.ObjectName);
            }
        }

        protected void VerifyNotDisposed()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.ObjectName);
            }
        }

        protected void VerifyNotDisposedOrDisposing()
        {
            if (this.IsDisposedOrDisposing)
            {
                throw new ObjectDisposedException(this.ObjectName);
            }
        }

        /// <summary>
        /// Override this in subclasses. This method only ever gonna be called once per object
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
        }

        protected virtual void OnDisposing()
        {
            this.Disposing.Raise(this);
        }

        protected void MarkAsDisposed()
        {
            GC.SuppressFinalize(this);
            Interlocked.Exchange(ref _disposeStage, DisposablCompleted);
        }

    }
}
