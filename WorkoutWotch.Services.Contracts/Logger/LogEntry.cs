using System;

namespace WorkoutWotch.Services.Contracts.Logger
{
    public struct LogEntry
    {
        public DateTime Timestamp { get; }
        public string Name { get; }
        public LogLevel Level { get; }
        public int ThreadId { get; }
        public string Message { get; }

        public LogEntry(DateTime timestamp, string name, LogLevel level, int threadId, string message)
        {
            Timestamp = timestamp;
            Name = name;
            Level = level;
            ThreadId = threadId;
            Message = message;
        }
    }
}