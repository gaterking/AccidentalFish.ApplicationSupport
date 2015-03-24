﻿namespace AccidentalFish.ApplicationSupport.Core.Components
{
    public interface IApplicationResourceSettingProvider
    {
        string SqlConnectionString(IComponentIdentity componentIdentity);

        string SqlContextType(IComponentIdentity componentIdentity);

        string StorageAccountConnectionString(IComponentIdentity componentIdentity);

        string DefaultTableName(IComponentIdentity componentIdentity);

        string DefaultQueueName(IComponentIdentity componentIdentity);

        string DefaultBlobContainerName(IComponentIdentity componentIdentity);

        string DefaultLeaseBlockName(IComponentIdentity componentIdentity);

        string DefaultTopicName(IComponentIdentity componentIdentity);

        string ServiceBusConnectionString(IComponentIdentity componentIdentity);
        
        string DefaultSubscriptionName(IComponentIdentity componentIdentity);
        string DefaultBrokeredMessageQueueName(IComponentIdentity componentIdentity);
    }
}
