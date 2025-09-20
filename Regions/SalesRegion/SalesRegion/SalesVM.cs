using System.Linq;

using RMSDataAccessLayer;

using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Windows.Data;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Transactions;
using SUT.PrintEngine.Utils;
using System.Windows.Media;
using Common.Core.Logging;
using log4netWrapper;
using QuickBooks;
using SalesRegion.Messages;
using SimpleMvvmToolkit;
using TrackableEntities;
using TrackableEntities.Common;
using TrackableEntities.EF6;


namespace SalesRegion
{
    public class SalesVM : ViewModelBase<SalesVM>
    {


        private static readonly SalesVM _instance;

        static SalesVM()
        {
            _instance = new SalesVM();
        }

        public static SalesVM Instance
        {
            get { return _instance; }
        }


        private static Cashier _cashier;

        public Cashier CashierEx
        {
            get { return _cashier; }
            set
            {
                if (_cashier != value)
                {
                    _cashier = value;
                    NotifyPropertyChanged(x => x.CashierEx);
                }
            }
        }

        public SalesVM()
        {

        }


        public void CloseTransaction()
        {
            try
            {
                Logger.Log(LoggingLevel.Info, "Close Transaction");
                if (batch == null)
                {
                    Logger.Log(LoggingLevel.Warning, "Batch is null");
                    MessageBox.Show("Batch is null");
                    return;
                }
                if (TransactionData != null)
                {
                    TransactionData.CloseBatchId = Batch.BatchId;
                    TransactionData.OpenClose = false;

                    SaveTransaction();
                    TransactionData = null;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }


        //public void CreateNewPrescription()
        //{
        //    try
        //    {

        //        Logger.Log(LoggingLevel.Info, "Create New Prescription");
        //        if (doctor == null)
        //        {
        //            Logger.Log(LoggingLevel.Warning, "Doctor is Missing");
        //            this.Status = "Doctor is Missing";
        //            return;
        //        }

        //        if (patient == null)
        //        {
        //            Logger.Log(LoggingLevel.Warning, "Patient is Missing");
        //            this.Status = "Patient is Missing";
        //            return;
        //        }

        //        if (Store == null)
        //        {
        //            Logger.Log(LoggingLevel.Warning, "Store is Missing");
        //            this.Status = "Store is Missing";
        //            return;
        //        }

        //        if (Batch == null)
        //        {
        //            Logger.Log(LoggingLevel.Warning, "Batch is Missing");
        //            this.Status = "Batch is Missing";
        //            return;
        //        }

        //        if (CashierEx == null)
        //        {
        //            Logger.Log(LoggingLevel.Warning, "Cashier is Missing");
        //            this.Status = "CashierEx is Missing";
        //            return;
        //        }

        //        if (Station == null)
        //        {
        //            Logger.Log(LoggingLevel.Warning, "Station is Missing");
        //            this.Status = "Station is Missing";
        //            return;
        //        }
        //        Prescription txn = new Prescription()
        //        {
        //            BatchId = Batch.BatchId,
        //            StationId = Station.StationId,
        //            Time = DateTime.Now,
        //            CashierId = CashierEx.Id,
        //            PharmacistId = (CashierEx.Role == "Pharmacist" ? CashierEx.Id : null as int?),
        //            StoreCode = Store.StoreCode,
        //            OpenClose = true,
        //            DoctorId = doctor.Id,
        //            PatientId = patient.Id,
        //            Patient = patient,
        //            Doctor = doctor,
        //            Cashier = CashierEx,
        //            Pharmacist = CashierEx.Role == "Pharmacist" ? CashierEx : null,
        //            TrackingState = TrackingState.Added
        //        };
        //        txn.StartTracking();
        //        Logger.Log(LoggingLevel.Info, "Prescription Created");
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
        //        throw ex;
        //    }
        //}


        //+ ToDo: Replace this with your own data fields

        private Doctor doctor = null;

        public Doctor Doctor
        {
            get { return doctor; }
            set
            {
                if (doctor != value)
                {
                    doctor = value;
                    NotifyPropertyChanged(x => x.Doctor);
                }
            }
        }

        private Patient patient = null;

        public Patient Patient
        {
            get { return patient; }
            set
            {
                if (patient != value)
                {
                    patient = value;
                    NotifyPropertyChanged(x => x.Patient);
                }
            }
        }

        private Cashier transactionCashier = null;
        public Cashier TransactionCashier
        {
            get { return transactionCashier; }
            set
            {
                if (transactionCashier != value)
                {
                    transactionCashier = value;
                    NotifyPropertyChanged(x => x.TransactionCashier);
                }
            }
        }

        private Cashier _transactionPharmacist = null;
        public Cashier TransactionPharmacist
        {
            get { return _transactionPharmacist; }
            set
            {
                if (_transactionPharmacist != value)
                {
                    _transactionPharmacist = value;
                    if (TransactionData != null)
                    {
                        if (value != null)
                        {
                            TransactionData.PharmacistId = value.Id;
                            TransactionData.Pharmacist = value;
                        }
                    }
                    NotifyPropertyChanged(x => x.TransactionPharmacist);
                }
            }
        }

        private string status = null;

        public string Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    NotifyPropertyChanged(x => x.Status);
                }
            }
        }


        public TransactionBase transactionData;

        public TransactionBase TransactionData
        {
            get { return transactionData; }
            set
            {
                if (!object.Equals(transactionData, value))
                {
                    Set_TransactionData(value);

                }
            }
        }

        private void Set_TransactionData(TransactionBase value)
        {
            transactionData = value;
            
           SendMessage(MessageToken.TransactionDataChanged,
                new NotificationEventArgs<TransactionBase>(MessageToken.TransactionDataChanged, transactionData));
            if (transactionData != null) transactionData.PropertyChanged += TransactionData_PropertyChanged;

            NotifyPropertyChanged(x => x.TransactionData);
        }

        private void TransactionData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentTransactionEntry")
            {
                if (transactionData != null)
                    if (transactionData.CurrentTransactionEntry != null)
                        if (transactionData.CurrentTransactionEntry.TransactionEntryItem != null)
                            SetCurrentItemDosage(transactionData.CurrentTransactionEntry.TransactionEntryItem);
            }
        }

        private ObservableCollection<object> _csv;

        public ObservableCollection<object> SearchList
        {
            get { return _csv; }

        }

        private ObservableCollection<Cashier> _pharmacists = null;

        public ObservableCollection<Cashier> Pharmacists
        {
            get
            {
                if (_pharmacists == null)
                {
                    using (var ctx = new RMSModel())
                    {
                        _pharmacists =
                            new ObservableCollection<Cashier>(
                                ctx.Persons.OfType<Cashier>().Where(x => x.Role == "Pharmacist"));
                        _pharmacists.ToList().ForEach(x => x.StartTracking());
                    }
                }
                return _pharmacists;
            }
        }


        private Cashier _currentPharmacist = null;

        public Cashier CurrentPharmacist
        {
            get
            {
                return _currentPharmacist;
            }
            set
            {
                if (_currentPharmacist != value)
                {
                    _currentPharmacist = value;
                    NotifyPropertyChanged(x => CurrentPharmacist);
                }
            }
        }


        public void UpdateSearchList(string filterText)
        {
            try
            {
                Logger.Log(LoggingLevel.Info,
                    string.Format("Update SearchList -filter Text [{0}] - StartTime:{1}", filterText, DateTime.Now));
                CompositeCollection cc = CreateSearchList(filterText);


                _csv = new ObservableCollection<Object>();
                foreach (var item in cc)
                {
                    _csv.Add(item);
                }
                NotifyPropertyChanged(x => x.SearchList);
                Logger.Log(LoggingLevel.Info,
                    string.Format("Finish Update SearchList - filter Text [{0}] - EndTime:{1}", filterText, DateTime.Now));
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        public void GetSearchResults(string filterText)
        {
            UpdateSearchList(filterText);
        }



        private CompositeCollection CreateSearchList(string filterText)
        {
            try
            {
                //todo: make parallel
                Logger.Log(LoggingLevel.Info,
                    string.Format("Start Create SearchList -filter Text [{0}] - StartTime:{1}", filterText, DateTime.Now));
                CompositeCollection cc = new CompositeCollection();


                foreach (var itm in AddSearchItems())
                {
                    cc.Add(itm);
                }


                GetPatients(cc, filterText);
                GetDoctors(cc, filterText);

                AddInventory(cc, filterText);

                double t = 0;
                if (double.TryParse(filterText, out t))
                {
                    AddTransaction(cc, filterText);
                }

                Logger.Log(LoggingLevel.Info,
                    string.Format("Finish Create SearchList -filter Text [{0}] - StartTime:{1}", filterText,
                        DateTime.Now));
                return cc;
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }


        private CompositeCollection AddSearchItems()
        {
            try
            {
                CompositeCollection cc = new CompositeCollection();
                //SearchItem b = new SearchItem();
                //b.SearchObject = new RMSDataAccessLayer.Transactionlist();
                //b.SearchCriteria = "Transaction History";
                //b.DisplayName = "Transaction History";
                //cc.Add(b);

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


                return cc;
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }





        private void AddTransaction(CompositeCollection cc, string filterText)
        {
            if (cc == null) return;
            try
            {
                using (var ctx = new RMSModel())
                {
                    // right now any prescriptions
                    foreach (
                        var trns in
                            ctx.TransactionBase.OfType<Prescription>()
                                .Where(x => x.TransactionId.ToString().Contains(filterText))
                                .OrderBy(t => t.Time)
                                .Take(100))
                    {
                        cc.Add(trns);
                    }
                }
                using (var ctx = new RMSModel())
                {
                    foreach (
                        var trns in
                            ctx.TransactionBase.OfType<QuickPrescription>()
                                .Where(x => x.TransactionId.ToString().Contains(filterText))
                                .OrderBy(t => t.Time)
                                .Take(100))
                    {
                        cc.Add(trns);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }






        private void GetDoctors(CompositeCollection cc, string filterText)
        {
            try
            {
                using (var ctx = new RMSModel())
                {
                    foreach (
                        var cus in
                            ctx.Persons.OfType<Doctor>()
                                .Where(
                                    x =>
                                        ("Dr. " + " " + x.FirstName.Trim().Replace(".", "").Replace(" ", "").Replace("Dr", "Dr. ") + " " +
                                         x.LastName.Trim() +
                                         " " + x.Code).Contains(filterText))
                                .Take(listCount))
                    {
                        cc.Add(cus);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private void GetPatients(CompositeCollection cc, string filterText)
        {
            try
            {
                using (var ctx = new RMSModel())
                {
                    foreach (
                        var cus in
                            ctx.Persons.OfType<Patient>()
                                .Where(x => (x.FirstName.Trim() + " " + x.LastName.Trim()).Contains(filterText))
                                .Take(listCount)) //
                    {
                        cc.Add(cus);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }


        private bool _showInactiveItems = false;
        public bool ShowInactiveItems
        {
            get
            {
                return _showInactiveItems;
            }
            set
            {
                _showInactiveItems = value;
                NotifyPropertyChanged(x => x.ShowInactiveItems);
            }

        }
        private int listCount = 25;


        private void AddInventory(CompositeCollection cc, string filterText)
        {
            try
            {
                //todo: make parallel
                using (var ctx = new RMSModel())
                {

                    var itms = ctx.Item.OfType<Medicine>().Where(x => ((x.ItemName ?? x.Description).Contains(filterText) || (x.ItemNumber.ToString().Contains(filterText)))
                                                                       && x.QBItemListID != null
                        // && x.Quantity > 0                           && 
                                                                       && x.QBActive == true
                                                                       && (x.Inactive == null ||
                                                                          (x.Inactive != null && x.Inactive == _showInactiveItems)))
                                                                         
                         .Take(listCount)
                         .AsEnumerable()
                         .OrderBy(x => x.DisplayName).ToList();

                    foreach (var itm in itms)
                    {
                        cc.Add(itm);
                    }
                }

                using (var ctx = new RMSModel())
                {
                    foreach (
                        var itm in
                            ctx.Item.OfType<StockItem>()
                                .Where(x => (x.ItemName ?? x.Description).Contains(filterText))
                                .Take(listCount))
                    {
                        cc.Add(itm);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }


        }




        public void ProcessSearchListItem(object SearchItem)
        {
            try
            {
                if (SearchItem == null) return;
                if (TransactionData != null && TransactionData.ChangeTracker == null) TransactionData.StartTracking();
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

                var searchItem = SearchItem as Item;
                if (searchItem != null)
                {

                    var itm = searchItem;
                    //  if (CheckDuplicateItem(itm)) return;
                    if (itm.Quantity < 0)
                    {
                        var res = MessageBox.Show("Item may not be in stock! Do you want to continue?", "Negative Stock",
                            MessageBoxButton.YesNo);
                        if (res == MessageBoxResult.No) return;
                    }
                    SetCurrentItemDosage(itm);

                    if (TransactionData != null)
                    {
                        InsertItemTransactionEntry(itm);
                    }
                    else
                    {
                        NewItemTransaction(itm);
                    }

                }
                var trn = SearchItem as TransactionBase;
                if (trn != null)
                {
                    GoToTransaction(trn);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        public void SetCurrentItemDosage(TransactionEntryItem itm)
        {
            if (itm.Item == null)
            {
                using (var ctx = new RMSModel())
                {
                    itm.Item = ctx.Item.FirstOrDefault(x => x.ItemId == itm.ItemId);
                    itm.TrackingState = TrackingState.Unchanged;
                }
            }
            SetCurrentItemDosage(itm.Item);
        }

        public void SetCurrentItemDosage(Item itm)
        {
            if (itm == null) return;
            using (var ctx = new RMSModel())
            {
                itm.DosageList =
                    ctx.ItemDosages.Where(x => x.ItemId == itm.ItemId)
                        .OrderByDescending(x => x.Count)
                        .Take(5)
                        .Select(x => x.Dosage)
                        .ToList();
                itm.TrackingState = TrackingState.Unchanged;
            }
        }

        //private bool CheckDuplicateItem(Item itm)
        //{
        //    if (TransactionData != null &&
        //        TransactionData.TransactionEntries.FirstOrDefault(x => x.TransactionEntryItem.ItemId == itm.ItemId) != null)
        //    {
        //        MessageBox.Show("Can't add same item twice!");
        //        return true;
        //    }
        //    return false;
        //}

        private void DoSearchItem(SearchItem searchItem)
        {
            throw new NotImplementedException();
        }


        private void AddPatientToTransaction(Patient patient)
        {
            if (patient == null) return;
            Patient = patient;
            if (TransactionData is Prescription == false)
            {
                var t = NewPrescription();
                CopyTransactionDetails(t, TransactionData);
               DeleteTransactionData();
                TransactionData = t;
            }
            var prescription = (Prescription)TransactionData;
            if (prescription != null)
            {
                prescription.PatientId = patient.Id;
                prescription.Patient = patient;
                prescription.Patient.StartTracking();
            }

        }



        private void AddDoctorToTransaction(Doctor doctor)
        {
            if (doctor == null) return;
            Doctor = doctor;
            if (TransactionData is Prescription == false)
            {
                var t = NewPrescription();
                CopyTransactionDetails(t, TransactionData);
                DeleteTransactionData();
                TransactionData = t;
            }

            var prescription = TransactionData as Prescription;
            if (prescription != null)
            {
                prescription.DoctorId = doctor.Id;
                prescription.Doctor = doctor;
                prescription.Doctor.StartTracking();
            }

        }


        private void GoToTransaction(TransactionBase trn)
        {
            GoToTransaction(trn.TransactionId);
        }


        public void GoToTransaction(int TransactionId)
        {
            try
            {
                Status = "";
                using (var ctx = new RMSModel())
                {
                    TransactionBase ntrn;
                    ntrn = (from t in ctx.TransactionBase
                        .Include(x => x.TransactionEntries)
                        .Include(x => x.Cashier)
                       //.Include(x => x.OldPrescription)
                       // .Include("OldPrescription.TransactionEntries")
                       // .Include(x => x.Repeats)
                       // .Include("Repeats.TransactionEntries")
                        .Include("TransactionEntries.TransactionEntryItem")
                        .Include("TransactionEntries.TransactionEntryItem.Item")

                                //.Include("TransactionEntries.Item.ItemDosages")
                            where t.TransactionId == TransactionId
                            orderby t.Time descending
                            select t).FirstOrDefault();
                    if (ntrn != null)
                    {
                        IncludePrecriptionProperties(ctx, ntrn);
                        Item = null;
                        NotifyPropertyChanged(x => x.Item.DosageList);
                        TransactionData = ntrn;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }


        public void GoToPreviousTransaction()
        {
            try
            {
                using (var ctx = new RMSModel())
                {
                    TransactionBase ptrn;

                    if (TransactionData == null || TransactionData.TransactionId == 0)
                    {
                        ptrn = GetDBTransaction(ctx).FirstOrDefault();
                    }
                    else
                    {
                        ptrn = GetDBTransaction(ctx).FirstOrDefault(t => t.TransactionId < TransactionData.TransactionId);
                    }
                   
                    if (ptrn != null)
                    {
                        IncludePrecriptionProperties(ctx, ptrn);
                        Item = null;
                        NotifyPropertyChanged(x => x.Item.DosageList);

                      //  IncludeInventoryProperties(ctx, ptrn);
                        TransactionData = ptrn;
                        this.Item = null;
                    }
                    else
                    {
                        MessageBox.Show("No previous transaction");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }


        private IOrderedQueryable<TransactionBase> GetDBTransaction(RMSModel ctx)
        {
            try
            {
                TransactionBase ptrn;
                return (from t in ctx.TransactionBase
                    .Include(x => x.TransactionEntries)
                    .Include(x => x.Cashier)
                  //  .Include(x => x.OldPrescription)
                  //  .Include("OldPrescription.TransactionEntries")
                  // .Include(x => x.Repeats)
                  //  .Include("Repeats.TransactionEntries")
                    .Include("TransactionEntries.TransactionEntryItem")
                    .Include("TransactionEntries.TransactionEntryItem.Item")
                            //.Include("TransactionEntries.Item.ItemDosages")
                        orderby t.Time descending
                        select t);

                //return ptrn;
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        public void IncludePrecriptionProperties(TransactionBase ptrn)
        {
            try
            {
                using (var ctx = new RMSModel())
                {
                    IncludePrecriptionProperties(ctx,ptrn);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        public void IncludePrecriptionProperties(RMSModel ctx, TransactionBase ptrn)
        {
            try
            {
                if (ptrn is Prescription)
                {
                    var pc = (ptrn as Prescription);
                    pc.Doctor = ctx.Persons.OfType<Doctor>().FirstOrDefault(x => x.Id == pc.DoctorId);
                    pc.Doctor.StartTracking();
                    pc.Patient = ctx.Persons.OfType<Patient>().FirstOrDefault(x => x.Id == pc.PatientId);
                    pc.Patient.StartTracking();
                 
                }
                this.TransactionCashier = ctx.Persons.OfType<Cashier>().FirstOrDefault(x => x.Id == ptrn.CashierId);
               
                this.TransactionPharmacist = ctx.Persons.OfType<Cashier>().FirstOrDefault(x => x.Id == ptrn.PharmacistId);
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }


        private void InsertItemTransactionEntry(RMSDataAccessLayer.Item itm)
        {
            try
            {
                var medicine = itm as Medicine;
                if (TransactionData.CurrentTransactionEntry == null)
                {
                   
                        PrescriptionEntry p = new PrescriptionEntry()
                        {
                            StoreID = Store.StoreId,
                            TransactionId = TransactionData.TransactionId,
                            TransactionEntryItem = CreateTransactionEntryItem(itm),
                            
                            Price = itm.Price,
                            Dosage = medicine == null?"":medicine.SuggestedDosage,
                            Taxable = itm.SalesTax != 0,
                            SalesTaxPercent = itm.SalesTax.GetValueOrDefault(),
                            TransactionTime = DateTime.Now,
                            EntryNumber =
                                TransactionData.TransactionEntries == null
                                    ? 1
                                    : (short?)TransactionData.TransactionEntries.Count,
                            // Transaction = TransactionData,
                            
                            TrackingState = TrackingState.Added
                        };
                        p.TransactionEntryItem.TransactionEntryBase = p;
                        p.StartTracking();
                        

                        TransactionData.TransactionEntries.Add(p);
                        NotifyPropertyChanged(x => TransactionData.TransactionEntries);
                        this.TransactionData.CurrentTransactionEntry = p;
                    
                }
                else
                {
                    var item = this.TransactionData.CurrentTransactionEntry;
                    if (item != null)
                    {
                        SetTransactionEntryItem(itm, item);

                        item.Price = itm.Price;
                       
                        //if (medicine != null) item.Dosage = medicine.SuggestedDosage;
                        
                        this.TransactionData.UpdatePrices();
                    }
                    
                    
                    this.Item = itm;
                }





                NotifyPropertyChanged(x => x.TransactionData);
                //NotifyPropertyChanged(x => x.CurrentTransactionEntry);
                NotifyPropertyChanged(x => x.Item);
                return;
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private TransactionEntryItem CreateTransactionEntryItem(Item itm)
        {
            if (itm == null) return null;
            return new TransactionEntryItem()
            {
                ItemId = itm.ItemId,
                ItemName = itm.ItemName ?? itm.Description,
                ItemNumber = itm.ItemNumber,
            QBItemListID = itm.QBItemListID,
                Item = itm,
                TrackingState = TrackingState.Added
            };
        }

        private TransactionEntryItem CreateTransactionEntryItem(TransactionEntryItem itm)
        {
            var newitm = GetCurrentQBInventoryItem(itm);
            if (newitm == null)
            {
                MessageBox.Show(
                    $"Item --'{itm.ItemName}' Not Found In current Inventory. Please create this Prescription Entry Manually!");
                return null;
            }
            var ti = new TransactionEntryItem()
            {
                ItemId = newitm.ItemId,
                ItemName = newitm.ItemName,
                ItemNumber = newitm.ItemNumber,
                QBItemListID = newitm.QBItemListID,
                Item = newitm.Item,
                TrackingState = TrackingState.Added
            };
            
            return ti;
        }

        private TransactionEntryItem GetCurrentQBInventoryItem(TransactionEntryItem oldEntryItem)
        {
            using (var ctx = new RMSModel())
            {
                //if item exist and is qbactive return it.
               var eitm = ctx.QBInventoryItems
                    .FirstOrDefault(
                        x => x.ListID.ToString() == oldEntryItem.QBItemListID && x.Items.Any(z => z.ItemNumber == oldEntryItem.ItemNumber && z.QBActive.Value == true));
                if (eitm != null) return oldEntryItem;

                var res =
                    ctx.QBInventoryItems
                    .Include(x => x.Items)
                    .FirstOrDefault(
                        x => x.ItemNumber.ToString() == oldEntryItem.ItemNumber && x.Items.Any(z =>z.ItemNumber == oldEntryItem.ItemNumber && z.QBActive.Value == true));
               
                if (res != null)
                {
                    var itm = res.Items.OrderByDescending(x => x.Inactive).FirstOrDefault();
                    MessageBox.Show(
                        $"Existing Item {oldEntryItem.ItemName} don't exist in QBInventory, it will be replaced with {itm.ItemName}");
                    return new TransactionEntryItem() {Item = itm, ItemNumber = itm.ItemNumber, ItemName = itm.ItemName, QBItemListID = res.ListID, ItemId = itm.ItemId, TrackingState = oldEntryItem.TrackingState, TransactionEntryId = oldEntryItem.TransactionEntryId, TransactionEntryBase = oldEntryItem.TransactionEntryBase};  

                }
                return null;
            }
        }

        private static void SetTransactionEntryItem(Item itm, PrescriptionEntry item)
        {
            if (itm == null) return;
            if (item?.TransactionEntryItem == null) return;
            item.TransactionEntryItem.TransactionEntryId = item.TransactionEntryId;
            item.TransactionEntryItem.TrackingState = TrackingState.Modified;
            item.TransactionEntryItem.ItemId = itm.ItemId;
            item.TransactionEntryItem.ItemName = itm.ItemName ?? itm.Description;
            item.TransactionEntryItem.ItemNumber = itm.ItemNumber;
            item.TransactionEntryItem.QBItemListID = itm.QBItemListID;
            item.TransactionEntryItem.Item = itm;
        }


        private bool AutoCreateOldTransactions()
        {
            try
            {
                if (TransactionData == null) return false;
                if (TransactionData.Time.Date != DateTime.Now.Date)
                {
                      MessageBox.Show(
                            "Modifying old transactions is not allowed! Do you want to create a New Transaction?",
                            "Can't Modify Old Transaction", MessageBoxButton.OK);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }


        public void DeleteTransactionEntry<T>(TransactionEntryBase dtrn) where T : TransactionEntryBase
        {
            try
            {
                if (dtrn == null)
                {

                    return;
                }
                if (AutoCreateOldTransactions() == false) return;

                using (var ctx = new RMSModel())
                {
                    var d = ctx.TransactionEntryBase.FirstOrDefault(x => x.TransactionEntryId == dtrn.TransactionEntryId);
                    if (d != null)
                    {
                        d.TrackingState = TrackingState.Deleted;
                        ctx.ApplyChanges(d);
                        ctx.SaveChanges();
                        d.AcceptChanges();
                    }
                    //else
                    //{
                    //    TransactionData.TransactionEntries.Remove(dtrn);
                    //}

                    //NotifyPropertyChanged(x => TransactionData.TransactionEntries);
                    //NotifyPropertyChanged(x => TransactionData);
                    //TransactionData.UpdatePrices();

                }
                GoToTransaction(TransactionData.TransactionId);
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }


        public void DeleteCurrentTransaction()
        {
            try
            {


                Logger.Log(LoggingLevel.Info,
                    string.Format("Start DeleteCurrentTransaction: StartTime:{0}", DateTime.Now));
                if (
                    MessageBox.Show("Are you sure you want to delete?", "Delete Current Transaction",
                        MessageBoxButton.YesNo) ==
                    MessageBoxResult.Yes)
                {
                    if (TransactionData != null && TransactionData.Time.Date != DateTime.Now.Date)
                    {
                        MessageBox.Show("Modifying old transactions is not allowed!",
                            "Can't Modify Old Transaction");
                        return;
                    }

                    //if (TransactionData.Repeats.Any())
                    //{
                    //    MessageBox.Show("This Prescription has been repeated! ");
                    //    return;
                    //}
                    DeleteTransactionData();
                    GoToPreviousTransaction();

                }
                Logger.Log(LoggingLevel.Info,
                    string.Format("Finish DeleteCurrentTransaction: EndTime:{0}", DateTime.Now));
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private void DeleteTransactionData()
        {
            if (TransactionData != null && TransactionData.TrackingState != TrackingState.Added)
            {
                using (var ctx = new RMSModel())
                {
                    var t = ctx.TransactionBase.FirstOrDefault(x => x.TransactionId == TransactionData.TransactionId);
                    if (TransactionData != null)
                    {
                        t.TrackingState = TrackingState.Deleted;
                        ctx.ApplyChanges(t);
                        ctx.SaveChanges();
                    }
                    TransactionData.TrackingState = TrackingState.Deleted;
                    // TransactionData.AcceptChanges();
                }
            }
            TransactionData = null;
        }


        public TransactionBase CopyCurrentTransaction(bool copydetails = true)
        {
            try
            {
                using (var t = new TransactionScope())
                {
                    dynamic newt = null;
                    if (TransactionData is Prescription)
                    {
                        var p = NewPrescription();
                        p.StartTracking();
                        var doc = ((Prescription) TransactionData).Doctor;
                        if (doc != null)
                        {
                            p.Doctor = doc;
                            p.DoctorId = p.Doctor.Id;
                            p.Doctor.StartTracking();
                        }
                        var pat = ((Prescription) TransactionData).Patient;
                        if (pat != null)
                        {
                            p.Patient = pat;
                            p.Patient.StartTracking();
                            p.PatientId = p.Patient.Id;
                        }
                        newt = p;
                    }
                    if (TransactionData is QuickPrescription)
                        newt = CreateNewQuickPrescription();
                        newt.StartTracking();

                    if (copydetails)
                    {
                        CopyTransactionDetails(newt, TransactionData);
                    }
                    newt.UpdatePrices();
                    t.Complete();
                    return newt;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, ex.Message + " | " + ex.StackTrace);
                throw ex;
            }
        }

        private void CopyTransactionDetails(dynamic newt, TransactionBase t)
        {
            if (newt == null || t == null) return;

            var entries = t.TransactionEntries.OfType<PrescriptionEntry>();
            //VerifyInventory(entries);

            foreach (var itm in entries)
            {
                 var ti = CreateTransactionEntryItem(itm.TransactionEntryItem);
                if (ti == null) continue;
                var te = new PrescriptionEntry()
                {
                    Dosage = itm.Dosage,
                    TransactionEntryItem = ti,
                    Repeat = itm.Repeat,
                    Quantity = itm.Quantity,
                    SalesTaxPercent = itm.SalesTaxPercent,
                    Price = itm.Price,
                    ExpiryDate = itm.ExpiryDate,
                    Comment = itm.Comment,
                    TrackingState = TrackingState.Added
                };
               

                te.StartTracking();
                
                newt.TransactionEntries.Add(te);
            }
        }

        private void VerifyInventory(IEnumerable<PrescriptionEntry> entries)
        {
            using (var ctx = new RMSModel())
            {
                foreach (var itm in entries)
                {
                    var inv = ctx.Item.FirstOrDefault(x => x.QBItemListID == itm.TransactionEntryItem.QBItemListID && x.QBActive == true);
                    if (inv == null)
                        MessageBox.Show(
                            $"{itm.TransactionEntryItem.ItemName}-{itm.TransactionEntryItem.ItemNumber} is not Availible in QuickBooks! please Re-Create item.");
                }
            }
        }


        public void AutoRepeat()
        {
            try
            {
                
                TransactionBase newt = CopyCurrentTransaction();
                foreach (PrescriptionEntry item in newt.TransactionEntries.ToList())
                {
                    if (item.Repeat == 0)
                    {
                        //newt.TransactionEntries.Remove(item);
                        // rms.Detach(item);
                    }
                    else
                    {
                        item.Repeat -= 1;
                    }

                }

                var oldTrans = TransactionData;
                // rms.TransactionBase.AddObject(newt);
                TransactionData = newt;
               // SaveTransaction();

                //if (oldTrans is Prescription)
                //{
                    
                //    ((Prescription)TransactionData).OldPrescription.Add(oldTrans as Prescription);
                   
                //}
                if(!SaveTransaction()) return;
                SalesVM.Instance.GoToTransaction(newt.TransactionId);
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ;
            }

        }


        private void NewItemTransaction(Item SearchItem)
        {
            try
            {
              //  if (CheckDuplicateItem(SearchItem)) return;
                if (TransactionData == null)
                {
                    TransactionData = CreateNewQuickPrescription();
                    TransactionData.StartTracking();
                }
                InsertItemTransactionEntry(SearchItem as Item);
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        public QuickPrescription CreateNewQuickPrescription()
        {
            try
            {
                return new QuickPrescription()
                {
                    BatchId = Batch.BatchId,
                    StationId = Station.StationId,
                    Time = DateTime.Now,
                    CashierId = CashierEx.Id,
                    PharmacistId = (CashierEx.Role == "Pharmacist" ? CashierEx.Id : null as int?),
                    StoreCode = Store.StoreCode,
                    OpenClose = true,
                    TrackingState = TrackingState.Added
                };
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }


       

        public void Print(ref FrameworkElement fwe, PrescriptionEntry prescriptionEntry)
        {
            PrintServer printserver = null;
            PrintDialog pd = null;
            DrawingVisual visual = null;
            SUT.PrintEngine.Paginators.VisualPaginator page = null;
            
            try
            {
                printserver = Station.PrintServer.StartsWith("\\")
                                              ? new PrintServer(Station.PrintServer)
                                              : new LocalPrintServer();
                
                Size visualSize = new Size(288, 2 * 96); // paper size

                visual = PrintControlFactory.CreateDrawingVisual(fwe, fwe.ActualWidth, fwe.ActualHeight);

                page = new SUT.PrintEngine.Paginators.VisualPaginator(
                    visual, visualSize, new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, 0));
                page.Initialize(false);

                pd = new PrintDialog();
                pd.PrintQueue = printserver.GetPrintQueue(Station.ReceiptPrinterName);

                pd.PrintDocument(page, "");
            }
            catch (Exception ex)
            {
                Instance.UpdateTransactionEntry(ex, prescriptionEntry);
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            }
            finally
            {
                // Dispose resources properly
                pd?.PrintQueue?.Dispose();
                printserver?.Dispose();
                
                // Note: DrawingVisual and VisualPaginator don't implement IDisposable
                // but we null them to help GC
                visual = null;
                page = null;
                pd = null;
            }
        }

        


        public void PostQBSale()
        {

            try
            {

                if (TransactionData == null || string.IsNullOrEmpty(TransactionData.TransactionNumber))
                {
                    MessageBox.Show("Invalid Transaction Please Try again");
                    return;
                }
                if (TransactionData.ChangeTracker == null) TransactionData.StartTracking();
                    TransactionData.Status = "ToBePosted";
                if (!SaveTransaction())
                {
                    MessageBox.Show("Post failed to Save! Please Check that all fields are entered and try again.");
                    return;
                } 
                if (ServerMode != true)
                {
                    Post(TransactionData);
                }

            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

       

        private void Post(TransactionBase TransactionData)
        {
            try
            {

                IncludePrecriptionProperties(TransactionData);

                SalesReceipt s = new SalesReceipt();
                s.TxnDate = TransactionData.Time;
                s.TxnState = "1";
                s.Workstation = "02";
                s.StoreNumber = "1";
                s.SalesReceiptNumber = "123";
                s.Discount = "0";

                
                if (TransactionData is Prescription)
                {
                    Prescription p = TransactionData as Prescription;
                    string doctor = "";
                    string patient = "";
                    if (p.Doctor != null)
                    {
                        doctor = p.Doctor.DisplayName;
                    }
                    if (p.Patient != null)
                    {
                        patient = p.Patient.ContactInfo;
                        s.Discount = p.Patient.Discount == null ? "" : p.Patient.Discount.ToString();
                    }
                    s.Comments = String.Format("{0} \n RX#:{1} \n Doctor:{2}", patient,
                        p.TransactionNumber, doctor);
                }
                else
                {
                    s.Comments = "RX#:" + TransactionData.TransactionNumber;
                }

                if (TransactionData != null)
                {
                    s.TrackingNumber = TransactionData.TransactionNumber;
                }
                s.Associate = "Dispensary";
                s.SalesReceiptType = "0";



                foreach (var item in TransactionData.TransactionEntries)
                {
                    if (item.TransactionEntryItem!= null)
                    {

                        s.SalesReceiptDetails.Add(new SalesReceiptDetail
                        {
                            ItemListID = item.TransactionEntryItem.QBItemListID,
                            QtySold = (Decimal)item.Quantity
                        }); //340 
                    }
                    else
                    {
                        ////MessageBox.Show("Please Link Quickbooks item to dispensary");
                        //TransactionData.Status = "Please Link Quickbooks item to dispensary";
                        //rms.SaveChanges();
                        return;
                    }
                }


               // qb = new QBPOS(Settings);
                var result = QBPOS.AddSalesReceipt(s,QBCompanyFile);
                if (result != null)
                {
                    TransactionData.ReferenceNumber = "QB#" + result.SalesReceiptNumber;
                    TransactionData.Status = "Posted";
                    SaveTransaction(TransactionData);
                    //using (var ctx = new RMSModel())
                    //{
                    //    TransactionData.ReferenceNumber = "QB#" + result.SalesReceiptNumber;
                    //    TransactionData.Status = "Posted";
                       
                    //    //ctx.TransactionBase.AddOrUpdate(TransactionData);
                    //    //ctx.SaveChanges();
                    //}
                }
                else
                {
                    // problem
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private static string _qbCompanyFile;

        public string QBCompanyFile
        {
            get { return _qbCompanyFile; }
            set { _qbCompanyFile = value; }
        }

        public async Task DownloadAllQBItems()
        {
            try
            {
                Status = "Start Downloading";

                var t = await QBPOS.GetAllInventoryQuery(QBCompanyFile).ConfigureAwait(false);
                //set qbactive for all inventory items first
                using (var ctx = new RMSModel())
                {
                    ctx.Database.ExecuteSqlCommand("update item set QBActive = 0");
                }
                
                await Task.Run(() => ProcessQBItems(t)).ConfigureAwait(false);

                Status = "Download complete";
                MessageBox.Show("Download Complete");
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private async Task ProcessQBItems(List<ItemInventoryRet> itms)
        {
            try
            {
                if (itms != null)
                {
                    var itmcnt = 0;
                    List<Medicine> clst = null;
                    using (var ctx = new RMSModel())
                    {
                        clst = ctx.Item.OfType<Medicine>()
                            .Where(x => x.QBItemListID != null)
                            // .Where(x => x.ItemNumber == "6315")
                            .ToList();
                    }
                    Parallel.ForEach(itms, (item) => //.Where(x => x.ItemNumber == 6315)
                    {
                        //if (itmcnt%100 == 0)
                        //{
                        //    ctx.SaveChanges(); //SaveDatabase();
                        //}
                        using (var ctx = new RMSModel())
                        {
                            var i = ctx.QBInventoryItems.FirstOrDefault(x => x.ListID == item.ListID);
                            if (i == null)
                            {
                                i = new QBInventoryItem()
                                {
                                    ListID = item.ListID,
                                    ItemName = item.Desc1,
                                    ItemDesc2 = item.Desc2,
                                    Size = item.Size,
                                    DepartmentCode = "MISC",
                                    ItemNumber = System.Convert.ToInt16(item.ItemNumber),
                                    TaxCode = item.TaxCode,
                                    Price = System.Convert.ToDouble(item.Price1),
                                    Quantity = System.Convert.ToDouble(item.QuantityOnHand),
                                    UnitOfMeasure = item.UnitOfMeasure
                                };

                                ctx.QBInventoryItems.Add(i);
                            }

                            i.ItemName = item.Desc1;
                            i.ItemDesc2 = item.Desc2;
                            i.ListID = item.ListID;
                            i.Size = item.Size;
                            i.UnitOfMeasure = item.UnitOfMeasure;
                            i.TaxCode = item.TaxCode;
                            i.ItemNumber = System.Convert.ToInt16(item.ItemNumber);
                            i.Price = System.Convert.ToDouble(item.Price1);
                            i.Quantity = System.Convert.ToDouble(item.QuantityOnHand);

                            ctx.QBInventoryItems.AddOrUpdate(i);

                            Medicine itm = ctx.Item.OfType<Medicine>().FirstOrDefault(x => x.QBItemListID == i.ListID);
                            if (itm == null)
                            {
                                itm = new Medicine()
                                {
                                    DateCreated = DateTime.Now,
                                    SuggestedDosage = "Take as Directed by your Doctor"
                                };

                                ctx.Item.Add(itm);
                            }

                            if (itm != null)
                            {
                                itm.Description = i.ItemDesc2;
                                itm.Price = i.Price.GetValueOrDefault();
                                itm.Quantity = Convert.ToDouble(i.Quantity);
                                itm.SalesTax = (i.TaxCode != null && i.TaxCode.ToUpper() == "VAT"
                                    ? .15
                                    : 0);
                                itm.QBItemListID = i.ListID;
                                itm.UnitOfMeasure = i.UnitOfMeasure;
                                itm.ItemName = i.ItemName;
                                itm.ItemNumber = i.ItemNumber.ToString();
                                itm.Size = i.Size;
                                itm.QBActive = true;
                                ctx.Item.AddOrUpdate(itm);
                            }
                            ctx.SaveChanges();
                        }
                        // itmcnt += 1;
                        Status = $"Downloading {item.ItemNumber}";
                    });
                    //SaveDatabase();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        public void Notify(string token, object sender, NotificationEventArgs e)
        {
            MessageBus.Default.Notify(token, sender, e);
        }






        private Item item = null;

        public Item Item
        {
            get { return item; }
            set
            {
                if (item != null)
                {
                    item = value;
                    NotifyPropertyChanged(x => x.Item);
                }
            }
        }

        private ObservableCollection<TransactionsView> transactionList = null;

        public ObservableCollection<TransactionsView> TransactionList
        {
            get { return transactionList; }
            set
            {
                if (transactionList != value)
                {
                    transactionList = value;
                    NotifyPropertyChanged(x => x.TransactionList);
                }
            }
        }

        public Patient CreateNewPatient(string searchtxt)
        {
            var p = CreateNewPatient();
            p.StartTracking();
            SetNames(searchtxt, p);
            return p;
        }

        private void SetNames(string searchtxt, Person p)
        {
            var strs = searchtxt.Split(' ');
            p.FirstName = strs.FirstOrDefault();
            p.LastName = strs.LastOrDefault();
        }

        public Patient CreateNewPatient()
        {
            return new Patient(){TrackingState = TrackingState.Added};
        }

        public bool SavePerson(Person patient)
        {
            var res = false;
            try
            {
                
                using (var ctx = new RMSModel())
                {
                    ctx.ApplyChanges(patient);
                    ctx.SaveChanges();
                    patient.AcceptChanges();
                    //ctx.Persons.AddOrUpdate(patient);
                    //ctx.SaveChanges();
                }
                res = true;
                return res;
            }
            catch (DbEntityValidationException vex)
            {
                var str = vex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Aggregate("", (current, er) => current + (er.PropertyName + ","));
                MessageBox.Show("Please Check the following fields before saving! - " + str);
                return res;
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        public List<TransactionBase> GetPatientTransactionList(Patient p)
        {
            using (var ctx = new RMSModel())
            {
                return
                    new List<TransactionBase>(
                        ctx.TransactionBase.OfType<Prescription>().Where(x => x.PatientId == p.Id).ToList());
            }
        }

        public List<TransactionBase> GetDoctorTransactionList(Doctor d)
        {
            using (var ctx = new RMSModel())
            {
                return
                    new List<TransactionBase>(
                        ctx.TransactionBase.OfType<Prescription>().Where(x => x.DoctorId == d.Id).ToList());
            }
        }


        public Doctor CreateNewDoctor(string searchtxt)
        {
            var d = CreateNewDoctor();
            d.StartTracking();
            SetNames(searchtxt, d);
            return d;
        }
        public Doctor CreateNewDoctor()
        {
            return new Doctor() { TrackingState = TrackingState.Added }; 
        }

        public bool SaveTransaction()
        {
            var res = SaveTransaction(TransactionData);
            NotifyPropertyChanged(x => x.TransactionData);
            return res;

        }

        public bool SaveTransaction(TransactionBase trans)
        {
            try
            {
                if (trans == null || trans.TransactionEntries == null) return false;

                if (trans != null && trans.GetType() == typeof(Prescription))
                {
                    var p = trans as Prescription;
                    if (p.Doctor == null)
                    {
                        MessageBox.Show("Please Select a doctor");
                        return false;
                    }
                    else
                    {
                        p.Doctor.TrackingState = TrackingState.Unchanged;
                    }
                    if (p.Patient == null)
                    {
                        MessageBox.Show("Please Select a Patient");
                        return false;
                    }
                    {
                        p.Patient.TrackingState = TrackingState.Unchanged;
                    }
                }
                if (trans.ChangeTracker == null) return true;
              using (var ctx = new RMSModel())
              {
                 
                    try
                    {
                        var t = trans.ChangeTracker.GetChanges().FirstOrDefault();//trans.ChangeTracker.GetChanges();
                        t.TransactionEntries.ToList().ForEach(x =>
                        {
                           
                            x.TransactionEntryItem.Item = null;
                            if (x.TransactionEntryItem.TrackingState == TrackingState.Unchanged)
                                x.TransactionEntryItem = null;
                            if (x.ModifiedProperties == null) x.ModifiedProperties = new List<string>();
                            if (x.ModifiedProperties.Contains("Amount")) x.ModifiedProperties.Remove("Amount");
                            if (x.ModifiedProperties.Contains("SalesTax")) x.ModifiedProperties.Remove("SalesTax");
                        });
                        if(t.ModifiedProperties == null) t.ModifiedProperties = new List<string>();
                        if(t.ModifiedProperties.Contains("TotalSales")) t.ModifiedProperties.Remove("TotalSales");
                        if (t.ModifiedProperties.Contains("TotalTax")) t.ModifiedProperties.Remove("TotalTax");
                        if (t.ModifiedProperties.Contains("Amount")) t.ModifiedProperties.Remove("Amount");
                        if (t.ModifiedProperties.Contains("TotalDiscount")) t.ModifiedProperties.Remove("TotalDiscount");
                        if (t.ModifiedProperties.Contains("Pharmacist")) t.ModifiedProperties.Remove("Pharmacist");
                        if (t.ModifiedProperties.Contains("Cashier")) t.ModifiedProperties.Remove("Cashier");
                        if (t.ModifiedProperties.Contains("Patient")) t.ModifiedProperties.Remove("Patient");
                        if (t.ModifiedProperties.Contains("CurrentTransactionEntry")) t.ModifiedProperties.Remove("CurrentTransactionEntry");

                        t.Pharmacist = null;
                        t.Cashier = null;
                        

                        ctx.ApplyChanges(t);
                        ctx.SaveChanges();
                       //// trans.ChangeTracker.MergeChanges(ref trans,t);

                       // trans.AcceptChanges();
                       // //.TransactionNumber = trans.TransactionNumber;
                       // ForceTransactionEntryNumberUpdate(TransactionData);
                       // TransactionData.AcceptChanges();
                        GoToTransaction(t.TransactionId);
                        return true;
                    }
                    catch (DbEntityValidationException vex)
                    {
                        var str = vex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Aggregate("", (current, er) => current + (er.PropertyName + ","));
                        MessageBox.Show("Please Check the following fields before saving! - " + str);
                        return false;
                    }
                    catch (DbUpdateConcurrencyException dce)
                    {
                        // Get failed entry
                        foreach (var itm in dce.Entries)
                        {
                            if (itm.State != EntityState.Added)
                            {
                                var dv = itm.GetDatabaseValues();
                                if (dv != null) itm.OriginalValues.SetValues(dv);
                            }
                        }
                        return true;
                    }
                    catch (Exception ex1)
                    {
                        if (!ex1.Message.Contains("Object reference not set to an instance of an object")) throw; 
                    }

                   // trans.TransactionId = trans.TransactionId;
                       
                    if (trans != null)
                    {
                        var dbEntry = ctx.Entry(trans);

                        if (dbEntry != null && dbEntry.State != EntityState.Deleted)
                        {
                            if (trans.TransactionEntries != null)
                               ForceTransactionEntryNumberUpdate(trans);
                        }
                    }
                    return false;
                }
                
            }
           
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw;
            }
        }

        private void ForceTransactionEntryNumberUpdate(TransactionBase transactionBase)
        {
            if (transactionBase == null) return;
            foreach (var te in transactionBase.TransactionEntries)
            {
                te.TransactionEntryNumber = "0";
            }
        }

        private void CleanTransactionNavProperties(TransactionBase titm, RMSModel ctx)
        {
            try
            {
                var itm = titm as Prescription;
                if (itm != null)
                {
                    var dbEntityEntry = ctx.Entry(itm.Doctor);
                    if (dbEntityEntry != null &&
                        (dbEntityEntry.State != EntityState.Unchanged && dbEntityEntry.State != EntityState.Detached))
                    {
                        dbEntityEntry.State = EntityState.Unchanged;
                    }
                    var p = ctx.Entry(itm.Patient);
                    if (p != null && (p.State != EntityState.Unchanged && p.State != EntityState.Detached))
                    {
                        p.State = EntityState.Unchanged;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }
        
        private static Batch batch;

        public Batch Batch
        {
            get { return batch; }
            set
            {
                if (batch != value)
                {
                    batch = value;
                    NotifyPropertyChanged(x => x.Batch);
                }
            }

        }

        private static Station station;

        public Station Station
        {
            get { return station; }
            set
            {
                if (station != value)
                {
                    station = value;
                    NotifyPropertyChanged(x => x.Station);
                }
            }

        }

        private static Store store;

        public Store Store
        {
            get { return store; }
            set
            {
                if (store != value)
                {
                    store = value;
                    NotifyPropertyChanged(x => x.Store);
                }
            }

        }



        internal Prescription NewPrescription()
        {
            try
            {
                var trn = new Prescription()
                {
                    StationId = Station.StationId,
                    BatchId = Batch.BatchId,
                    Time = DateTime.Now,
                    CashierId = _cashier.Id,
                    StoreCode = Store.StoreCode,
                    TrackingState = TrackingState.Added
                };
                trn.StartTracking();

                return trn;
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        public bool ServerMode { get; set; }

        internal void SaveMedicine(Medicine medicine)
        {
            using (var ctx = new RMSModel())
            {
                ctx.ApplyChanges(medicine);
                ctx.SaveChanges();
                medicine.AcceptChanges();
            }
       
        }


        public void UpdateTransactionEntry(Exception exception, PrescriptionEntry prescriptionEntry)
        {
            var d = TransactionData.TransactionEntries.IndexOf(prescriptionEntry) + 1;
            MessageBox.Show(($"Could Not Print No:{d} Item-'{prescriptionEntry.TransactionEntryItem.ItemName}'"));
           
        }
    }
}
