using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using WorkoutWotch.Services.Contracts.Logger;
using WorkoutWotch.Services.Logger;
using WorkoutWotch.UnitTests.System.Reactive.Linq;
using Xunit;

namespace WorkoutWotch.UnitTests.Services.Logger
{
    public class LoggerServiceTests
    {
        [Fact]
        public void is_debug_enabled_reflects_threshold()
        {
            var service = new LoggerService();

            service.Threshold = LogLevel.Debug;
            Assert.True(service.IsDebugEnabled);

            service.Threshold = LogLevel.Info;
            Assert.False(service.IsDebugEnabled);
        }

        [Fact]
        public void is_info_enabled_reflects_threshold()
        {
            var service = new LoggerService();

            service.Threshold = LogLevel.Info;
            Assert.True(service.IsInfoEnabled);

            service.Threshold = LogLevel.Warning;
            Assert.False(service.IsInfoEnabled);
        }

        [Fact]
        public void is_performacne_enabled_reflects_threshold()
        {
            var service = new LoggerService();

            service.Threshold = LogLevel.Performance;
            Assert.True(service.IsPerformanceEnabled);

            service.Threshold = LogLevel.Warning;
            Assert.False(service.IsPerformanceEnabled);
        }

        [Fact]
        public void is_warning_enabled_reflects_threshold()
        {
            var service = new LoggerService();

            service.Threshold = LogLevel.Warning;
            Assert.True(service.IsWarningEnabled);

            service.Threshold = LogLevel.Error;
            Assert.False(service.IsWarningEnabled);
        }

        [Theory]
        [InlineData(new[]{LogLevel.Warning,LogLevel.Debug,LogLevel.Error,LogLevel.Info,LogLevel.Performance})]
        public void is_error_enabled_is_always_true(LogLevel[] levels)
        {
            var service = new LoggerService();

            foreach (LogLevel logLevel in levels)
            {
                service.Threshold = LogLevel.Info;
                Assert.True(service.IsErrorEnabled);
            }
        }

        [Fact]
        public void get_logger_throws_if_type_is_null()
        {
            var service = new LoggerService();
            Assert.Throws<ArgumentNullException>(() => service.GetLogger((Type) null));
        }

        [Fact]
        public void get_logger_throws_if_name_is_null()
        {
            var service = new LoggerService();
            Assert.Throws<ArgumentNullException>(() => service.GetLogger((string)null));
        }

        [Fact]
        public void get_logger_for_type_returns_a_logger_with_full_name_of_the_type_as_its_name()
        {
            var service = new LoggerService();
            var logger = service.GetLogger(this.GetType());
            Assert.Equal(this.GetType().FullName,logger.Name);
        }

        [Fact]
        public async Task log_entries_ticks_for_log_calls_within_the_configured_threshold()
        {
            var service = new LoggerService();
            var logger = service.GetLogger("test");

            var entriesTask =
                service
                    .Entries
                    .Take(3)
                    .ToListAsync()
                .ToTask();

            service.Threshold = LogLevel.Info;
            logger.Debug("whatever");
            logger.Debug("foo");
            logger.Debug("bar");
            logger.Info("An informational message");
            logger.Debug("foo");
            logger.Warning("A warning message");
            logger.Debug("foo");
            logger.Debug("foo");
            logger.Error("An Error message");


            var entries = await entriesTask;
            Assert.Equal("An informational message", entries[0].Message);
            Assert.Equal(LogLevel.Info, entries[0].Level);
            Assert.Equal("A warning message", entries[1].Message);
            Assert.Equal(LogLevel.Warning, entries[1].Level);
            Assert.Equal("An Error message", entries[2].Message);
            Assert.Equal(LogLevel.Error, entries[2].Level);
        }

        [Fact]
        public async Task log_entries_can_be_formatted()
        {
            var service = new LoggerService();
            var logger = service.GetLogger("test");

            var entryTask =
                service
                    .Entries
                    .FirstAsync()
                    .Timeout(TimeSpan.FromSeconds(3))
                    .ToTask();

            logger.Debug("A message with a parameter: {0}",42);

            var entry = await entryTask;

            Assert.Equal("A message with a parameter: 42", entry.Message);
        }

        [Fact]
        public async Task log_entries_can_contain_exception_details()
        {
            var service = new LoggerService();
            var logger = service.GetLogger("test");

            var entryTask =
                service
                    .Entries
                    .FirstAsync()
                    .Timeout(TimeSpan.FromSeconds(3))
                    .ToTask();

            logger.Debug(new InvalidOperationException("foo"), "A message with an exception and a parameter ({0})", 42);

            var entry = await entryTask;

            Assert.Equal("A message with an exception and a parameter (42) : System.InvalidOperationException: foo", entry.Message);
        }
    }
}
