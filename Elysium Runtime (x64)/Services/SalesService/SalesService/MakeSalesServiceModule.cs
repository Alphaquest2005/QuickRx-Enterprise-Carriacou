using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using SalesServiceInterface;

namespace SalesService
{
    public class MakeSalesServiceModule : IModule
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer container;

        public MakeSalesServiceModule(IEventAggregator eventAggregator, IUnityContainer container)
        {
            this.eventAggregator = eventAggregator;
            this.container = container;
        }

        public void Initialize()
        {
            RegisterServices();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            //+ TODO: Add any events this service needs to subscribe to here
        }

        private void RegisterServices()
        {
            // Indicate to unity that we know what to do with an  IMakeSalesService interface
            container.RegisterType<IMakeSalesService, MakeSalesService>();
        }
    }
}
