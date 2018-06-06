using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLMock;
using WorkoutWotch.Services.Contracts.Logger.Mocks;
using WorkoutWotch.Services.Logger;
using WorkoutWotch.Services.State;
using WorkoutWotch.UnitTests.Services.State.Mocks;
using Xunit;

namespace WorkoutWotch.UnitTests.Services.State
{
    public class StateServiceTests
    {
        [Fact]
        public async Task save_async_execute_all_tasks_returned_by_saved_callbacks()
        {
            var service = new StateService(new BlobCacheMock(), new LoggerServiceMock(MockBehavior.Loose));
            var firstExecuted = false;
            var secondExecuted = false;
            service.RegisterSaveCallback(_ => Task.Run(() => firstExecuted = true));
            service.RegisterSaveCallback(_ => Task.Run(() => secondExecuted = true));

            await service.SaveAsync();

            Assert.True(firstExecuted);
            Assert.True(secondExecuted);
        }

        [Fact]
        public async Task save_async_does_not_fail_if_a_save_callack_fails()
        {
            var service = new StateService(new BlobCacheMock(), new LoggerServiceMock(MockBehavior.Loose));
            service.RegisterSaveCallback(_ => Task.Run(() => throw new Exception("Failed")));

            await service.SaveAsync();
        }

        [Fact]
        public void register_save_callback_throws_if_save_task_factory_is_null()
        {
            var service = new StateService(new BlobCacheMock(), new LoggerServiceMock(MockBehavior.Loose));
            Assert.Throws<ArgumentNullException>(() => service.RegisterSaveCallback(null));

        }

        [Fact]
        public async Task
            register_save_callback_returns_a_registeration_handle_that_unregisters_the_callback_when_disposed()
        {
            var service = new StateService(new BlobCacheMock(), new LoggerServiceMock(MockBehavior.Loose));
            var firstExecuted = false;
            var secondExecuted = false;
            var handle = service.RegisterSaveCallback(_ => Task.Run(() => firstExecuted = true));
            service.RegisterSaveCallback(_ => Task.Run(() => secondExecuted = true));

            handle.Dispose();
            await service.SaveAsync();

            Assert.False(firstExecuted);
            Assert.True(secondExecuted);
        }
    }
}
