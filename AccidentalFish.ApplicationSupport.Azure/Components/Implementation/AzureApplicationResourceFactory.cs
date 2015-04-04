﻿using AccidentalFish.ApplicationSupport.Azure.TableStorage;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.NoSql;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Repository;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.Components.Implementation
{
    internal class AzureApplicationResourceFactory : IAzureApplicationResourceFactory
    {
        private readonly IApplicationResourceFactory _applicationResourceFactory;
        private readonly IApplicationResourceSettingProvider _applicationResourceSettingProvider;
        private readonly ITableStorageRepositoryFactory _tableStorageRepositoryFactory;

        public AzureApplicationResourceFactory(IApplicationResourceFactory applicationResourceFactory,
            IApplicationResourceSettingProvider applicationResourceSettingProvider,
            ITableStorageRepositoryFactory tableStorageRepositoryFactory)
        {
            _applicationResourceFactory = applicationResourceFactory;
            _applicationResourceSettingProvider = applicationResourceSettingProvider;
            _tableStorageRepositoryFactory = tableStorageRepositoryFactory;
        }

        #region Decorated methods

        public IUnitOfWorkFactory GetUnitOfWorkFactory(IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.GetUnitOfWorkFactory(componentIdentity);
        }

        public ILeaseManager<T> GetLeaseManager<T>(IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.GetLeaseManager<T>(componentIdentity);
        }

        public ILeaseManager<T> GetLeaseManager<T>(string leaseBlockName, IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.GetLeaseManager<T>(leaseBlockName, componentIdentity);
        }

        public IAsynchronousQueue<T> GetQueue<T>(IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetQueue<T>(componentIdentity);
        }

        public IAsynchronousQueue<T> GetQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetQueue<T>(queuename, componentIdentity);
        }

        public IAsynchronousQueue<T> GetBrokeredMessageQueue<T>(IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetBrokeredMessageQueue<T>(componentIdentity);
        }

        public IAsynchronousQueue<T> GetBrokeredMessageQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetBrokeredMessageQueue<T>(queuename, componentIdentity);
        }

        public IAsynchronousTopic<T> GetTopic<T>(IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetTopic<T>(componentIdentity);
        }

        public IAsynchronousTopic<T> GetTopic<T>(string topicName, IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetTopic<T>(topicName, componentIdentity);
        }

        public IAsynchronousSubscription<T> GetSubscription<T>(IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetSubscription<T>(componentIdentity);
        }

        public IAsynchronousSubscription<T> GetSubscription<T>(string subscriptionName, IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetSubscription<T>(subscriptionName, componentIdentity);
        }

        public IAsynchronousBlockBlobRepository GetBlockBlobRepository(IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.GetBlockBlobRepository(componentIdentity);
        }

        public IAsynchronousBlockBlobRepository GetBlockBlobRepository(string containerName, IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.GetBlockBlobRepository(containerName, componentIdentity);
        }

        public string StorageAccountConnectionString(IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.StorageAccountConnectionString(componentIdentity);
        }

        public string SqlConnectionString(IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.SqlConnectionString(componentIdentity);
        }

        public string Setting(IComponentIdentity componentIdentity, string settingName)
        {
            return _applicationResourceFactory.Setting(componentIdentity, settingName);
        }

        #endregion

        public IAsynchronousTableStorageRepository<T> GetTableStorageRepository<T>(IComponentIdentity componentIdentity) where T : ITableEntity, new()
        {
            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            string defaultTableName = _applicationResourceSettingProvider.DefaultTableName(componentIdentity);
            return _tableStorageRepositoryFactory.CreateAsynchronousNoSqlRepository<T>(storageAccountConnectionString, defaultTableName);
        }

        public IAsynchronousTableStorageRepository<T> GetTableStorageRepository<T>(string tablename, IComponentIdentity componentIdentity) where T : ITableEntity, new()
        {
            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            return _tableStorageRepositoryFactory.CreateAsynchronousNoSqlRepository<T>(storageAccountConnectionString, tablename);
        }

        public IAsynchronousTableStorageRepository<T> GetTableStorageRepository<T>(string tablename, IComponentIdentity componentIdentity,
            bool lazyCreateTable) where T : ITableEntity, new()
        {
            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            return _tableStorageRepositoryFactory.CreateAsynchronousNoSqlRepository<T>(storageAccountConnectionString, tablename, lazyCreateTable);
        }
    }
}
