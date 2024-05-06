using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torc.BuildingBlocks.EventBus.Events;

namespace Torc.BuildingBlocks.EventBus.Absractions
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> 
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}
