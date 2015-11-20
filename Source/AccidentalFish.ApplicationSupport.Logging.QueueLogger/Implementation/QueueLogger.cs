﻿using System;
using System.Diagnostics;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using AccidentalFish.ApplicationSupport.Logging.QueueLogger.Model;

namespace AccidentalFish.ApplicationSupport.Logging.QueueLogger.Implementation
{
    internal class QueueLogger : ILogger<IQueue<LogQueueItem>>
    {
        private readonly IRuntimeEnvironment _runtimeEnvironment;
        private readonly IQueue<LogQueueItem> _queue;
        private readonly IFullyQualifiedName _source;
        private readonly IQueueLoggerExtension _queueLoggerExtension;
        private readonly LogLevelEnum _minimumLoggingLevel;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public QueueLogger(
            IRuntimeEnvironment runtimeEnvironment,
            IQueue<LogQueueItem> queue,
            IFullyQualifiedName source,
            IQueueLoggerExtension queueLoggerExtension,
            LogLevelEnum minimumLoggingLevel,
            ICorrelationIdProvider correlationIdProvider)
        {
            _runtimeEnvironment = runtimeEnvironment;
            _queue = queue;
            _source = source;
            _queueLoggerExtension = queueLoggerExtension;
            _minimumLoggingLevel = minimumLoggingLevel;
            _correlationIdProvider = correlationIdProvider;
        }

        public void Verbose(string message)
        {
            Log(LogLevelEnum.Verbose, message);
        }

        public void Verbose(string message, Exception exception)
        {
            Log(LogLevelEnum.Verbose, message, exception);
        }

        public void Debug(string message)
        {
            Log(LogLevelEnum.Debug, message);
        }

        public void Debug(string message, Exception exception)
        {
            Log(LogLevelEnum.Debug, message, exception);
        }

        public void Information(string message)
        {
            Log(LogLevelEnum.Information, message);
        }

        public void Information(string message, Exception exception)
        {
            Log(LogLevelEnum.Information, message, exception);
        }

        public void Warning(string message)
        {
            Log(LogLevelEnum.Warning, message);
        }

        public void Warning(string message, Exception exception)
        {
            Log(LogLevelEnum.Warning, message, exception);
        }

        public void Error(string message)
        {
            Log(LogLevelEnum.Error, message);
        }

        public void Error(string message, Exception exception)
        {
            Log(LogLevelEnum.Error, message, exception);
        }

        public void Fatal(string message)
        {
            Log(LogLevelEnum.Fatal, message);
        }

        public void Fatal(string message, Exception exception)
        {
            Log(LogLevelEnum.Fatal, message, exception);
        }

        public void Log(LogLevelEnum level, string message)
        {
            Log(level, message, null);
        }

        public void Log(LogLevelEnum level, string message, Exception exception)
        {
            LogQueueItem item = CreateLogQueueItem(level, message, exception);
            bool willLog = level >= _minimumLoggingLevel;
            if (_queueLoggerExtension.BeforeLog(item, exception, willLog))
            {
                if (willLog)
                {
                    try
                    {
                        _queue.Enqueue(item);
                    }
                    catch (Exception)
                    {
                        Trace.TraceError("Unable to enqueue log queue item");
                    }
                }
            }
        }

        private LogQueueItem CreateLogQueueItem(LogLevelEnum level, string message, Exception exception)
        {
            return new LogQueueItem
            {
                CorrelationId = _correlationIdProvider.CorrelationId,
                ExceptionName = exception?.GetType().FullName,
                InnerExceptionName = exception?.InnerException?.GetType().FullName,
                Level = level,
                LoggedAt = DateTimeOffset.UtcNow,
                Message = message,
                RoleIdentifier = _runtimeEnvironment.RoleIdentifier,
                RoleName = _runtimeEnvironment.RoleName,
                Source = _source.FullyQualifiedName,
                StackTrace = exception?.StackTrace
            };
        }

        public IQueue<LogQueueItem> ActualLogger => _queue;
    }
}
