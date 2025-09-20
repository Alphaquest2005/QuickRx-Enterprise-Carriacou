using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using PrismMVVMLibrary;
using SalesServiceInterface;
using SalesServiceEvents;
using RMSDataAccessLayer;
using System.ComponentModel;
using Microsoft.Practices.Prism.Regions;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using System.Data.Entity;
using System.Windows.Data;
using System.Printing;
using SUT.PrintEngine.Utils;
using System.Windows.Media;



namespace SalesRegion
{
    public class SalesVM : ViewModelBase
    {
        private readonly IUnityContainer container;
        private readonly IEventAggregator eventAggregator;

        
       
    
        //+ Example Command
        public DelegateCommand ExampleCommand { get; set; }

        IRegionManager regionManager;


   public RMSModel rms = new RMSModel();
   Cashier ca; Batch batch;  Station station;

   public ApplicationMode ApplicationMode { get; set; }


        public SalesVM(IUnityContainer container, IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            this.container = container;
            this.eventAggregator = eventAggregator;

            ApplicationMode = SalesRegion.ApplicationMode.Pharmacy;

            //+ Example of run time data vs. design time data (Design data goes in in SalesVMDesign.cs)
            //set runtime to new transaction
            //if (IsInDesignMode == false)
            //{
           
      
              
                // get the current cashier to add it
               
                      ca = (from c in rms.CashierLogs
                               where c.MachineName == Environment.MachineName && c.Status == "LogIn"
                               select c.Cashier).FirstOrDefault();

                station =  (from s in rms.Stations
                               where s.MachineName == Environment.MachineName
                               select s).FirstOrDefault();

                batch = (from b in rms.Batches
                         where b.StationId == station.StationId && b.Status == "Open"
                         select b).FirstOrDefault();
                
                UpdateSearchList();
                //CreateNewTransaction<TransactionBase>();
                //transactionData.TransactionEntries.Add(new TicketEntry());
                
            //}

           

            ExampleCommand = new DelegateCommand(CallExampleService);

            //+ Example of event subscription
            var prismEvent = eventAggregator.GetEvent<SaleCompleteEvent>();
            
            prismEvent.Subscribe(OnSaleComplete, true);
            
        }



      

    

        public void CloseTransaction<T>() where T : TransactionBase
        {
            TransactionData.CloseBatchId = batch.BatchId;
               TransactionData.OpenClose = false;
        }

        public void CreateNewTransaction<T>() where T : TransactionBase
        {
            TransactionBase txn ;
            if (typeof(T) == typeof(TransactionBase))
            {
                 txn = rms.CreateObject<Transaction>();
            }
            else
            {
                
                txn = rms.CreateObject<T>();
            }

            //create a dummy and save it then deep copy to new type and don't save
            
           
            //if (TransactionNumber != finaltxn) TransactionNumber = finaltxn;
            //

            if (batch == null)
            {
                MessageBox.Show("Please Open Draw");
                txn = null;
                return;
            }

            txn.BatchId = batch.BatchId;
            txn.StationId = station.StationId;
            txn.CashierId = ca.Id;
            txn.StoreCode = station.Store.StoreCode;
            txn.Time = DateTime.Now;
            txn.Station = station;
            txn.Cashier = ca;
            txn.OpenClose = true;

            rms.TransactionBase.AddObject(txn);
            txn.TenderEntryEx.Add(new TenderEntryEx());
           
            //get last number used in transaction to create the txn number

            TransactionData = txn;
            

            //################# Can't always save new transaction because of required fields

            txn.TransactionNumber = CreateTxnNumber(txn);
           // SaveTransaction();
            // txn.TransactionNumber = CreateTxnNumber(txn);
            
           // SaveTransaction();
            //TransactionData.Cashier = ca;
        }

        public T CreateNewEntity<T>() where T : class
        {
            T obj = rms.CreateObject<T>();
           
            return obj;
        }

        private string CreateTxnNumber(TransactionBase txn)
        {
            BarCodes.UPCA.cUPCA barcode = new BarCodes.UPCA.cUPCA();
            string txnnumber = (station.Store.TransactionSeed + (txn.TransactionId - station.Store.SeedTransaction)).ToString().PadLeft(11, '0');
            string finaltxn = txnnumber + barcode.GetCheckSum(txnnumber).ToString();
            return finaltxn;
        }

        public void SaveTransaction()
        {
            try
            {
                


                rms.SaveChanges();

                var txnlist = (from t in rms.TransactionBase
                               where t.TransactionNumber == "000000000000"
                               select t);
                foreach (TransactionBase txn in txnlist)
                {
                    txn.TransactionNumber = CreateTxnNumber(txn);
                }

                rms.SaveChanges();

                UpdateSearchList();
            }
            catch (Exception e)
            {

                throw;
            }

        }
      

        //+ Example of how to call a service.
        private void CallExampleService()
        {
            //+ Call an example service
            IMakeSalesService makeSalesService = container.Resolve<IMakeSalesService>();
           // makeSalesService.ExampleServiceCall("Service call To be Implemented");
        }

        //+ Example of event subscription
        private void OnSaleComplete(ISalesCompleteEventData salesCompleteEventData)
        {
            //+ Example action for the event call by the service call
           // TransactionData;
        }

      
        //+ ToDo: Replace this with your own data fields
        TransactionBase ptran;
        private RMSDataAccessLayer.TransactionBase transactionData;
        public RMSDataAccessLayer.TransactionBase TransactionData
        {
            get { return transactionData; }
            set
            {
                if (!object.Equals(transactionData, value))
                {
                    ptran = transactionData;
                    transactionData = value;
                  
                    this.regionManager.Regions["HeaderRegion"].Context = transactionData;
                    RaisePropertyChanged(() => TransactionData);
                }
            }
        }


        private ListCollectionView _csv;
        public ListCollectionView SearchList
        {
            get { return _csv; }
           
        }
        public void UpdateSearchList()
        {
            CompositeCollection cc = CreateSearchList();

            _csv = new ListCollectionView(cc);
            RaisePropertyChanged(() => SearchList);
            
        }

        private CompositeCollection CreateSearchList()
        {
            CompositeCollection cc = new CompositeCollection();
         
            AddSearchItems(cc);
            AddCustomers(cc);
            if(ApplicationMode == SalesRegion.ApplicationMode.Ticket) AddPass(cc);
            AddTransaction(cc);
            AddInventory(cc);
           

            return cc;
        }

        private void AddSearchItems(CompositeCollection cc)
        {
            SearchItem b = new SearchItem();
            b.SearchObject = new RMSDataAccessLayer.Transactionlist();
            b.SearchCriteria = "Transaction History";
            b.DisplayName = "Transaction History";
            cc.Add(b);

            if (ApplicationMode == SalesRegion.ApplicationMode.Pharmacy)
            {
                SearchItem p = new SearchItem();
                p.SearchObject = null;
                p.SearchCriteria = "Add Patient";
                p.DisplayName = "Add Patient";
                cc.Add(p);

                SearchItem d = new SearchItem();
                d.SearchObject = null;
                d.SearchCriteria = "Add Doctor";
                d.DisplayName = "Add Doctor";
                cc.Add(d);
            }

        }

        private void AddPass(CompositeCollection cc)
        {
            foreach (var pass in rms.Pass)
            {
                cc.Add(pass);
            }
        }

        private void AddTransaction(CompositeCollection cc)
        {
            switch (ApplicationMode)
            {
                case ApplicationMode.Ticket:
                    var opnlst = (from trn in rms.TransactionEntryBase.OfType<TicketEntry>()
                                  //where trn.EndDateTime == null
                                  orderby trn.TransactionTime

                                  select trn.Transaction);


                    foreach (var trns in opnlst)
                    {
                        cc.Add(trns);
                    }
                    break;
                case ApplicationMode.Pharmacy:
                    // right now any prescriptions
                    foreach (var trns in rms.TransactionBase.OfType<Prescription>().OrderBy(t => t.Time))
                    {
                        cc.Add(trns);
                    }
                    foreach (var trns in rms.TransactionBase.OfType<QuickPrescription>().OrderBy(t => t.Time))
                    {
                        cc.Add(trns);
                    }

                    break;
                case ApplicationMode.POS:
                    foreach (var trns in rms.TransactionBase.OfType<Transaction>().OrderBy(t => t.Time))
                    {
                        cc.Add(trns);
                    }

                    break;
                default:
                    break;
            }

        }

        private void AddCustomers(CompositeCollection cc)
        {
            switch (ApplicationMode)
            {
                case ApplicationMode.Ticket:
                    break;
                case ApplicationMode.Pharmacy:
                    foreach (var cus in rms.Persons.OfType<Patient>())
                    {
                        cc.Add(cus);
                    }

                    foreach (var cus in rms.Persons.OfType<Doctor>())
                    {
                        cc.Add(cus);
                    }
                    break;
                case ApplicationMode.POS:
                    foreach (var cus in rms.Persons.OfType<Customers>())
                    {
                        cc.Add(cus);
                    }
                    break;
                default:
                    break;
            }

        }
        private void AddInventory(CompositeCollection cc)
        {
            switch (ApplicationMode)
            {
                case ApplicationMode.Ticket:
      foreach (var itm in rms.Item.OfType<TicketItem>())
            {
                cc.Add(itm);
            }
                    break;
                case ApplicationMode.Pharmacy:
                    foreach (var itm in rms.Item.OfType<Medicine>())
                    {
                        cc.Add(itm);
                    }

                    foreach (var itm in rms.Item.OfType<StockItem>())
                    {
                        cc.Add(itm);
                    }
                    break;
                case ApplicationMode.POS: 
               foreach (var itm in rms.Item.OfType<StockItem>())
                    {
                        cc.Add(itm);
                    }
                    break;
                   
                default:
                    break;
            }
           
        }

        public void AddCustomerToTransaction(RMSDataAccessLayer.Customers cus)
        {

            TransactionData.CustomerId = cus.Id;
        }

        public void ProcessSearchListItem(object SearchItem)
        {
            if (SearchItem != null)
            {
                if (typeof(RMSDataAccessLayer.SearchItem) == SearchItem.GetType())
                {
                    DoSearchItem(SearchItem as RMSDataAccessLayer.SearchItem);
                }

                if (typeof(RMSDataAccessLayer.Doctor) == SearchItem.GetType())
                {
                    AddDoctorToTransaction(SearchItem as Doctor);
                }

                if (typeof(RMSDataAccessLayer.Patient) == SearchItem.GetType())
                {
                    AddPatientToTransaction(SearchItem as Patient);
                }




                if (typeof(RMSDataAccessLayer.Customers) == SearchItem.GetType())
                {
                    AddCustomerToTransaction(SearchItem as Customers);
                }
                if (typeof(RMSDataAccessLayer.Pass) == SearchItem.GetType())
                {
                    //save transaction
                   // SaveTransaction();
                    
                        //create new transaction
                        CreateNewTransaction<Ticket>();
                        // get ticket item from inventory
                        var itm = (from i in rms.Item
                                   where i.Description == "Ticket"
                                   select i).FirstOrDefault();
                        //create new ticket

                        AddPassToTransaction(SearchItem as Pass);

                        if (TransactionData.Status == null)
                        {
                            InsertItemTransactionEntry((Item)itm);
                        }
                        //save transaction
                        SaveTransaction();
                    
                    

                }
                if (typeof(RMSDataAccessLayer.Item).IsInstanceOfType(SearchItem))
                {
                    NewItemTransaction(SearchItem as Item);
                }
                if (typeof(TransactionBase).IsAssignableFrom(SearchItem.GetType()))
                {
                    GoToTransaction((TransactionBase)SearchItem);
                }

            }
        }

        private void AddPatientToTransaction(Patient patient)
        {
             if (!typeof(Prescription).IsInstanceOfType(transactionData))
            {
                SaveTransaction();
                CreateNewTransaction<Prescription>();
               
            }
             ((Prescription)TransactionData).PatientId = patient.Id;
             //((Prescription)TransactionData).Patient = patient;
        }

        private void AddDoctorToTransaction(Doctor doctor)
        {
            // only prescriptions can have doctors


            if (!typeof(Prescription).IsInstanceOfType(transactionData))
            {
                SaveTransaction();
                CreateNewTransaction<Prescription>();
               
            }
             ((Prescription)TransactionData).DoctorId = doctor.Id;
            //((Prescription)TransactionData).Doctor = doctor;
           
           
        }



        private void DoSearchItem(SearchItem searchItem)
        {
            if (rms.TransactionBase.GetType() == searchItem.GetType())
            {

            }
        }

        private void GoToTransaction(TransactionBase trn)
        {
            TransactionData = trn;

        }

        public void GoToPreviousTransaction()
        {
            if (TransactionData == null) return;
            TransactionBase ptrn = (from t in rms.TransactionBase
                                    where t.TransactionId < TransactionData.TransactionId
                                    orderby t.Time descending
                                    select t).FirstOrDefault();
            if (ptrn != null)
                rms.Attach(ptrn);    
            TransactionData = ptrn;


        }

        private void AddPassToTransaction(RMSDataAccessLayer.Pass pass)
        {          

            foreach (var trn in pass.Ticket)
            {
                foreach (TicketEntry tic in trn.TransactionEntries)
                {
                    if (tic.EndDateTime == null)
                    {
                        GoToTransaction(trn);
                        return;
                    }
                }
            }

            ((Ticket)TransactionData).PassId = pass.PassId;
        }

        private void InsertItemTransactionEntry(RMSDataAccessLayer.Item itm)
        {

            if (TransactionData.CurrentTransactionEntry == null)
            {
                if (ApplicationMode == SalesRegion.ApplicationMode.Ticket )//typeof(TicketItem).IsInstanceOfType(itm)
                {
                    CreateTransactionEntry<TicketEntry>();
                }
                else if (ApplicationMode == SalesRegion.ApplicationMode.Pharmacy)//typeof(Medicine).IsInstanceOfType(itm)
                {
                    CreateTransactionEntry<PrescriptionEntry>();
                }
                else
                {
                    CreateTransactionEntry<TransactionEntry>();
                }
            }

            if (itm.ItemId == 0) throw new Exception("This is a new created item, should be existing item");
            TransactionData.CurrentTransactionEntry.ItemId = itm.ItemId;
            TransactionData.CurrentTransactionEntry.Price = itm.Price;
            TransactionData.CurrentTransactionEntry.SalesTaxPercent = itm.SalesTax;
            TransactionData.CurrentTransactionEntry.Item = itm;
           // SaveTransaction();
        }

        private void CreateTransactionEntry<T>() where T: TransactionEntryBase
        {
            T tkt = rms.TransactionEntryBase.CreateObject<T>();
            rms.TransactionEntryBase.AddObject(tkt);
            TransactionData.TransactionEntries.Add(tkt);
            
            //ItemEditor.ItemsSource = new List<RMSDataAccessLayer.TransactionEntryBase> {Transaction.CurrentTransactionEntry};
            TransactionData.CurrentTransactionEntry = tkt;
        }

        public void DeleteTransactionEntry<T>(TransactionEntryBase dtrn) where T : TransactionEntryBase
        {
            TransactionData.TransactionEntries.Remove(dtrn);
            rms.TransactionEntryBase.DeleteObject(dtrn);

        }


        public void CloseTicket()
        {
            ((TicketEntry)TransactionData.CurrentTransactionEntry).EndDateTimeEx = DateTime.Now;
            //Print Ticket
            SaveTransaction();
        }

        public void NewTransaction()
        {
            //save transaction
           // SaveTransaction();

            //create new transaction
            if (TransactionData == null || TransactionData.TransactionEntries.Count != 0)
            {
                switch (ApplicationMode)
                {
                    case ApplicationMode.Ticket:
                        CreateNewTransaction<Ticket>();
                        break;
                    case ApplicationMode.Pharmacy:
                        CreateNewTransaction<Prescription>();
                        break;
                    case ApplicationMode.POS:
                        CreateNewTransaction<Transaction>();
                        break;
                    default:
                        CreateNewTransaction<TransactionBase>();
                        break;
                }
                
            }
            // get ticket item from inventory

            if(ApplicationMode == SalesRegion.ApplicationMode.Ticket) InsertItemTransactionEntry(rms.Item.OfType<TicketItem>().First());      
            
            //save transaction
            SaveTransaction();
            //print ticket
        }

        private void NewItemTransaction(Item SearchItem)
        {
            if (TransactionData == null)
            {
                if (ApplicationMode== SalesRegion.ApplicationMode.Ticket) CreateNewTransaction<Ticket>();//typeof(TicketItem).IsInstanceOfType(SearchItem)
                if (ApplicationMode== SalesRegion.ApplicationMode.Pharmacy) CreateNewTransaction<QuickPrescription>();//typeof(Medicine).IsInstanceOfType(SearchItem)
                if (ApplicationMode == SalesRegion.ApplicationMode.POS) CreateNewTransaction<Transaction>();//typeof(StockItem).IsInstanceOfType(SearchItem)
            }
            if(TransactionData != null) InsertItemTransactionEntry(SearchItem as Item);
        }


        public void Print(ref Grid fwe)
        {
            if (TransactionData == null) return;
            LocalPrintServer printServer = new LocalPrintServer();


            Size visualSize;
                if(ApplicationMode== SalesRegion.ApplicationMode.Pharmacy)
                {
                    visualSize = new Size(288, 2*96);// paper size
                }
            else
                {
                    visualSize = new Size(fwe.ActualWidth, fwe.ActualHeight);
                }

            DrawingVisual visual = PrintControlFactory.CreateDrawingVisual(fwe, fwe.ActualWidth, fwe.ActualHeight);


            SUT.PrintEngine.Paginators.VisualPaginator page = new SUT.PrintEngine.Paginators.VisualPaginator(visual, visualSize, new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, 0));
            page.Initialize(false);

            PrintDialog pd = new PrintDialog();
           // pd.PrintQueue = printServer.GetPrintQueue(TransactionData.Station.ReceiptPrinterName);
            if (pd.ShowDialog()==true)
            {

                pd.PrintDocument(page, "");
            }
        }

    }
}
