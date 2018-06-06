using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using HelperTrinity;
using WorkoutWotch.Services.Contracts.Logger;
using WorkoutWotch.Services.Contracts.State;
using WorkoutWotch.Utility;

namespace WorkoutWotch.Services.State
{
    public sealed class StateService : IStateService
    {
        private readonly IBlobCache _blobCache;
        private readonly ILogger _logger;
        private readonly IList<Func<IStateService, Task>> _saveCallbacks;
        private readonly object _sync;

        public StateService(IBlobCache blobCache, ILoggerService loggerSerive)
        {
            blobCache.AssertNotNull(nameof(blobCache));
            loggerSerive.AssertNotNull(nameof(loggerSerive));

            _blobCache = blobCache;
            _logger = loggerSerive.GetLogger(this.GetType());
            _saveCallbacks = new List<Func<IStateService, Task>>();
            _sync = new object(); 
        }
        public Task<T> GetAsync<T>(string key)
        {
            key.AssertNotNull(nameof(key));
            return _blobCache.GetObject<T>(key).ToTask();
        }

        public Task SetAsync<T>(string key, T value)
        {
            key.AssertNotNull(nameof(key));
            return _blobCache.InsertObject(key, value).ToTask();
        }

        public Task RemoveAsync<T>(string key)
        {
            key.AssertNotNull(nameof(key));
            return _blobCache.InvalidateObject<T>(key).ToTask();
        }

        public async Task SaveAsync()
        {
            IList<Task> saveTasks;
            lock (_sync)
            {
                saveTasks = _saveCallbacks.Select(x => x(this)).ToList();
            }

            try
            {
                await Task.WhenAll(saveTasks);
            }
            catch (Exception e)
            {
                _logger.Error(e,"Failed to save.");
            }

        }

        public IDisposable RegisterSaveCallback(Func<IStateService, Task> saveTaskFactory)
        {
            saveTaskFactory.AssertNotNull(nameof(saveTaskFactory));
            lock (_sync)
            {
                _saveCallbacks.Add(saveTaskFactory);
            }

            return new RegisterationHandle(this,saveTaskFactory);
        }

        private void UnregisterSaveCallback(Func<IStateService, Task> saveTaskFactory)
        {
            saveTaskFactory.AssertNotNull(nameof(saveTaskFactory));
            lock (_sync)
            {
                _saveCallbacks.Remove(saveTaskFactory);
            }
        }

        private sealed class RegisterationHandle : DisposableBase
        {
            private readonly StateService _owner;
            private readonly Func<IStateService, Task> _saveTaskFactory;

            public RegisterationHandle(StateService owner, Func<IStateService, Task> saveTaskFactory)
            {
                owner.AssertNotNull(nameof(owner));
                saveTaskFactory.AssertNotNull(nameof(saveTaskFactory));

                _owner = owner;
                _saveTaskFactory = saveTaskFactory;
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (disposing)
                {
                    this._owner.UnregisterSaveCallback(this._saveTaskFactory);
                }
            }
        }

    }
}
