﻿using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Alerts;

namespace AccidentalFish.ApplicationSupport.Azure.Alerts.Implementation
{
    class NullAlertSender : IAlertSender
    {
        public Task SendAsync(string title, string message)
        {
            return Task.FromResult(0);
        }
    }
}
