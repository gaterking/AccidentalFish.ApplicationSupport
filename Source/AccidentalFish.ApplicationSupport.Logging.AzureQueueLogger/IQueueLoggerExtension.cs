﻿using System;
using AccidentalFish.ApplicationSupport.Logging.AzureQueueLogger.Model;

namespace AccidentalFish.ApplicationSupport.Logging.AzureQueueLogger
{
    /// <summary>
    /// Allows the injection of functionality into the queue logger (and any other loggers that accept an ILoggerExtension in their constructor)
    /// </summary>
    public interface IQueueLoggerExtension
    {
        /// <summary>
        /// Called before the logger actually logs the item giving the extension a view of the item,
        /// the exception and an oppurtunity to preent logging occurring.
        /// </summary>
        /// <param name="item">The log item intended for logging</param>
        /// <param name="originalException">The exception, if any, associated with the log item</param>
        /// <param name="intendsToLog">Does the item meet the loggers ruleset for logging the item</param>
        /// <returns>True if the logger should log the item (academic if intendsToLog is false)</returns>
        bool BeforeLog(LogQueueItem item, Exception originalException, bool intendsToLog);
    }
}
