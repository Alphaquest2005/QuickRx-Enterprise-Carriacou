using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using PrismMVVMLibrary;
using Microsoft.Practices.Prism.Regions;
using RMSDataAccessLayer;
using Microsoft.Practices.Prism;
using System.Data.Entity;

namespace Transaction
{
    public class TransactionVM : ViewModelBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer container;

        public TransactionVM(IUnityContainer container, IEventAggregator eventAggregator)
        {
            //+ Example of run time data vs. design time data (Design data goes in in TransactionVMDesign.cs)
           // TransactionData =  regionManager..Regions["HeaderRegion"].Context as RMSDataAccessLayer.Transaction;
            
        }

        //RMSModel db = new RMSModel();

        //public RMSDataAccessLayer.Company Company
        //{
        //    get
        //    {
        //        db.Company.Load();
        //        if (db.Company.Count()!= 0)
        //        {
        //            return db.Company.ToList<RMSDataAccessLayer.Company>()[0];
        //        }
        //        return null;
        //    }
        //}



        //+ ToDo: Replace this with your own data fields
        private RMSDataAccessLayer.TransactionBase transactionData;
        public RMSDataAccessLayer.TransactionBase TransactionData
        {
            get { return transactionData; }
            set
            {
                if (!object.Equals(transactionData, value))
                {
                    transactionData = value;
                    RaisePropertyChanged(() => TransactionData);
                }
            }
        }
    }
}
