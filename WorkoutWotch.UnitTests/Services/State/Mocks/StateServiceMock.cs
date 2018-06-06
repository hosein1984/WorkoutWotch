using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using PCLMock;
using WorkoutWotch.Services.Contracts.State;

namespace WorkoutWotch.UnitTests.Services.State.Mocks
{
    public class StateServiceMock : MockBase<IStateService>, IStateService
    {
        public StateServiceMock(MockBehavior behavior = MockBehavior.Strict) : base(behavior)
        {
            if (behavior == MockBehavior.Loose)
            {
                this.When(x=>x.SaveAsync()).Return(Task.FromResult(true));
                this.When(x=>x.RegisterSaveCallback(It.IsAny<Func<IStateService,Task>>())).Return(Disposable.Empty);
            }
        }


        public Task<T> GetAsync<T>(string key)
        {
            return this.Apply(x => x.GetAsync<T>(key));
        }

        public Task SetAsync<T>(string key, T value)
        {
            return this.Apply(x => x.SetAsync(key, value));
        }

        public Task RemoveAsync<T>(string key)
        {
            return this.Apply(x => x.RemoveAsync<T>(key));
        }

        public Task SaveAsync()
        {
            return this.Apply(x => x.SaveAsync());
        }

        public IDisposable RegisterSaveCallback(Func<IStateService, Task> saveTaskFactory)
        {
            return this.Apply(x => x.RegisterSaveCallback(saveTaskFactory));
        }
    }
}
