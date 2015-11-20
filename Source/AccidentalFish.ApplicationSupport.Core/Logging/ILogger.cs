﻿using System;

namespace AccidentalFish.ApplicationSupport.Core.Logging
{
    /// <summary>
    /// Interface for synchronous loggers
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        void Verbose(string message);

        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        void Verbose(string message, Exception exception);

        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        void Debug(string message);

        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        void Debug(string message, Exception exception);

        /// <summary>
        /// Log an informational message
        /// </summary>
        /// <param name="message">The message to log</param>
        void Information(string message);
        /// <summary>
        /// Log an informational message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        void Information(string message, Exception exception);

        /// <summary>
        /// Log a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        void Warning(string message);
        /// <summary>
        /// Log a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        void Warning(string message, Exception exception);

        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="message">The message to log</param>
        void Error(string message);
        /// <summary>
        /// Log a error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        void Error(string message, Exception exception);

        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="message">The message to log</param>
        void Fatal(string message);
        /// <summary>
        /// Log a error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        void Fatal(string message, Exception exception);

        /// <summary>
        /// Log a message at the specified level
        /// </summary>
        /// <param name="level">The level to log at</param>
        /// <param name="message">The message to log</param>
        void Log(LogLevelEnum level, string message);

        /// <summary>
        /// Log a message at the specified level
        /// </summary>
        /// <param name="level">The level to log at</param>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        void Log(LogLevelEnum level, string message, Exception exception);
    }

    /// <summary>
    /// Log implementations that are actually wrappers for underlying implementations can
    /// implement this interface to expose a method that allows consumers to get to the underlying
    /// logger if required.
    /// </summary>
    /// <typeparam name="T">Type of the underlying logger</typeparam>
    public interface ILogger<out T> : ILogger
    {
        /// <summary>
        /// The underlying logger
        /// </summary>
        T ActualLogger { get; }
    }
}
