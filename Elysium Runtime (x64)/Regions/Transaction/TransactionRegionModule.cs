using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Transaction
{
    public class TransactionRegionModule : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;

        public TransactionRegionModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            RegisterViews();
        }

        private void RegisterViews()
        {
            // Register the view we know of
            regionManager.RegisterViewWithRegion("HeaderRegion", typeof(TransactionView));
        }

        /*
        <!--ToDo: Copy this to the your ModuleCatalog.xml-->        
        <Modularity:ModuleInfo Ref="file://Transaction.dll"
                    ModuleName="TransactionRegionModule"
                    ModuleType="Transaction.TransactionRegionModule, Transaction, Version=1.0.0.0">
        <!--ToDo: Update with the service module that your region depends on (if you have one).-->
        </Modularity:ModuleInfo>
        */
    }
}
