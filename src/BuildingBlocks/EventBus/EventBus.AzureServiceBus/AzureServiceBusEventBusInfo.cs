using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torc.BuildingBlocks.EventBus.AzureServiceBus
{
    public class AzureServiceBusEventBusInfo
    {
        public string TopicName { get; private set; }
        public string SubscriptionName { get; private set; }
        
        public AzureServiceBusEventBusInfo(string topicName, string subscriptionName)
        {
            TopicName = topicName;
            SubscriptionName = subscriptionName;
        }
    }
}
