using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;

namespace SalesServiceEvents
{
    public class SaleCompleteEvent : CompositePresentationEvent<ISalesCompleteEventData>
    {
    }

    //+ ToDo: This could be defined in another project (to allow for loose coupling).
    public interface ISalesCompleteEventData
    {
        string SomeData { get; set; }
    }

    //+ Use this in a different project to subscribe to this event 
    //var prismEvent = eventAggregator.GetEvent<SaleCompleteEvent>();
    //prismEvent.Subscribe(OnSaleComplete, true);

    //+ Use this in a different project to publish this event 
    //var prismEvent = eventAggregator.GetEvent<SaleCompleteEvent>();
    //prismEvent.Publish(new SalesCompleteEventData());
}
