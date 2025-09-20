using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using SalesServiceInterface;
using SalesServiceEvents;

namespace SalesService
{
    public class MakeSalesService : IMakeSalesService
    {
        private readonly IEventAggregator eventAggregator;

        public MakeSalesService(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public bool SaveTransaction(string parameter)
        {
            // Your WCF Service Call or other logic goes here

            //+ Call an example event to say that this service action happened
            var prismEvent = eventAggregator.GetEvent<SaleCompleteEvent>();
            prismEvent.Publish(new SalesCompleteEventData { SomeData = parameter });

            return true;
        }

    }

    //Payload for an event
    public class SalesCompleteEventData : ISalesCompleteEventData
    {
        //+ Your fields go here
        public string SomeData { get; set; }
    }

    //+ ToDo: Add this in another project to use this service
    //- This can be a parameter to a constructor as well:  MyClass(IMakeSalesService makeSalesService)
    //IMakeSalesService makeSalesService = container.Resolve<IMakeSalesService>();

    //- This is an example of how to call this Prism Service.
    //makeSalesService.ExampleServiceCall(ExampleData);

}
