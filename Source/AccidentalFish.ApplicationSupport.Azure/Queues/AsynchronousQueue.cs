﻿using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class AsynchronousQueue<T> : IAsynchronousStorageQueue<T> where T : class
    {
        private readonly CloudQueue _queue;
        private readonly IQueueSerializer _serializer;
        private readonly ILogger _logger;

        public AsynchronousQueue(IQueueSerializer queueSerializer, string connectionString, string queueName, ILogger logger)
        {
            if (queueSerializer == null) throw new ArgumentNullException(nameof(queueSerializer));
            if (String.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            if (String.IsNullOrWhiteSpace(queueName)) throw new ArgumentNullException(nameof(queueName));

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            queueClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(120), 3);
            _queue = queueClient.GetQueueReference(queueName);
            _serializer = queueSerializer;
            _logger = logger;

            _logger?.Verbose("AsynchronousQueue: created for queue {0}", queueName);
        }

        public Task EnqueueAsync(T item)
        {
            CloudQueueMessage message = new CloudQueueMessage(_serializer.Serialize(item));
            return _queue.AddMessageAsync(message);
        }

        public Task EnqueueAsync(T item, TimeSpan initialVisibilityDelay)
        {
            CloudQueueMessage message = new CloudQueueMessage(_serializer.Serialize(item));
            return _queue.AddMessageAsync(message, null, initialVisibilityDelay, null, null);
        }

        

        public Task DequeueAsync(Func<IQueueItem<T>, Task<bool>> processor)
        {
            return DequeueAsync(processor, null);
        }

        public async Task DequeueAsync(Func<IQueueItem<T>, Task<bool>> processor, TimeSpan? visibilityTimeout)
        {
            CloudQueueMessage message = await _queue.GetMessageAsync(visibilityTimeout, null, null);
            if (message != null)
            {
                T item = _serializer.Deserialize<T>(message.AsString);
                if (await processor(new CloudQueueItem<T>(message, item, message.DequeueCount, message.PopReceipt)))
                {
                    await _queue.DeleteMessageAsync(message);
                }
            }
            else
            {
                await processor(null);
            }
        }

        public async Task ExtendLeaseAsync(IQueueItem<T> queueItem, TimeSpan visibilityTimeout)
        {
            CloudQueueItem<T> queueItemImpl = queueItem as CloudQueueItem<T>;
            if (queueItemImpl == null)
            {
                throw new InvalidOperationException("Cannot mix Azure and non-Azure queue items when extending a lease");
            }
            await _queue.UpdateMessageAsync(queueItemImpl.CloudQueueMessage, visibilityTimeout, MessageUpdateFields.Visibility);
        }

        public async Task ExtendLeaseAsync(IQueueItem<T> queueItem)
        {
            CloudQueueItem<T> queueItemImpl = queueItem as CloudQueueItem<T>;
            if (queueItemImpl == null)
            {
                throw new InvalidOperationException("Cannot mix Azure and non-Azure queue items when extending a lease");
            }
            await _queue.UpdateMessageAsync(queueItemImpl.CloudQueueMessage, TimeSpan.FromSeconds(30), MessageUpdateFields.Visibility);
        }

        internal CloudQueue UnderlyingQueue => _queue;
    }
}
