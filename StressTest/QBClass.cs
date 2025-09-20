using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using log4netWrapper;
using QS2QBPost.Properties;
using QuickBooks;
using RMSDataAccessLayer;
using SalesRegion;
using Timer = System.Timers.Timer;

namespace QS2QBPost
{
    public class QBClass
    {
        private static volatile QBClass instance;
        private static object syncRoot = new Object();
        private Timer postingTimer;
        private Timer downloadTimer;

        static QBClass()
        {
            Instance.postingTimer = new System.Timers.Timer(3000);
            Instance.postingTimer.Elapsed += Instance.OnTimeToPost;
            Instance.postingTimer.Enabled = true;

            Instance.downloadTimer = new System.Timers.Timer(45 * 60 *1000);
            //60 minutes * 1000 milliseconds
            Instance.downloadTimer.Elapsed += Instance.OnTimeToDownload;
            Instance.downloadTimer.Enabled = true;

        }

        public static QBClass Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new QBClass();
                    }
                }

                return instance;
            }
        }

        private async void OnTimeToDownload(object sender, ElapsedEventArgs e)
        {
            if (Instance.downloadTimer.Enabled == true)
            {
                Instance.downloadTimer.Enabled = false;
                await DownloadFromQB().ConfigureAwait(false);
                Instance.downloadTimer.Enabled = true;
            }
        }

        private async Task DownloadFromQB()
        {
            try
            {
                await Task.Run(() => DownloadQBItems()).ConfigureAwait(false);
            }
            catch (Exception)
            {
                
                throw;
            }
          
        }

        private async void OnTimeToPost(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (postingTimer.Enabled == true) await PostToQB().ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public async Task PostToQB()
        {
            try
            {
                Instance.postingTimer.Enabled = false;
                var lst = new List<PostTransaction>();
                using (var ctx = new RMSModel())
                {
                    int i = 0;
                    lst = ctx.TransactionBase.Where(x => x.Status == "ToBePosted")
                        //.Include(x => x.TransactionEntries)
                       // .Include("TransactionEntries.Item")
                        //.Include(x => x.Cashier)
                        .Select(x => new PostTransaction()
                                 { TransactionData = x,
                                   PostEntries = x.TransactionEntries.Select(z => new PostEntry()
                                   {
                                       QBListId = z.TransactionEntryItem.QBItemListID,
                                       Quantity = (int) z.Quantity
                                   }).ToList()})
                        .ToList();
                }
                //while (keeprunning)
                foreach (var itm in lst)
                {
                    await Task.Run(() => Post(itm)).ConfigureAwait(false);
                }
                Instance.postingTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, ex.Message + ex.StackTrace);
                throw ;
            }
        }

        private void Post(PostTransaction pt)
        {
            try
            {

                IncludePrecriptionProperties(pt.TransactionData);

                SalesReceipt s = new SalesReceipt();
                s.TxnDate = pt.TransactionData.Time;
                s.TxnState = "1";
                s.Workstation = "02";
                s.StoreNumber = "1";
                s.SalesReceiptNumber = "123";
                s.Discount = "0";

                if (pt.TransactionData == null || string.IsNullOrEmpty(pt.TransactionData.TransactionNumber))
                {

                    //MessageBox.Show("Invalid Transaction Please Try again");
                    //TransactionData.Status = "Invalid Transaction Please Try again";
                    //rms.SaveChanges();
                    //return;
                }

                //TransPreZeroConverter tz = new TransPreZeroConverter();

                if (pt.TransactionData is Prescription)
                {
                    Prescription p = pt.TransactionData as Prescription;
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
                    s.Comments = "RX#:" + pt.TransactionData.TransactionNumber;
                }


                if (pt.TransactionData != null)
                {
                    s.TrackingNumber = pt.TransactionData.TransactionNumber;
                }
                s.Associate = "Dispensary";
                s.SalesReceiptType = "0";



                foreach (var item in pt.PostEntries)
                {
                    if (item.QBListId != null)
                    {

                        s.SalesReceiptDetails.Add(new SalesReceiptDetail
                        {
                            ItemListID = item.QBListId,
                            QtySold = item.Quantity
                        }); //340 
                    }
                    else
                    {
                        pt.TransactionData.Status = "Can't Post Because ListId is null";
                        UpdateTransactionDataFromSalesRet(pt, new SalesReceiptRet() {Comments = "No ListID for item"});
                        return;
                    }
                }


                // QBPOS qb = new QBPOS();
                
                  var result = QBPOS.Instance.AddSalesReceipt(s);
                if (result != null)
                {
                    UpdateTransactionDataFromSalesRet(pt, result);
                }




            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private void UpdateTransactionDataFromSalesRet(PostTransaction pt, SalesReceiptRet result)
        {
            using (var ctx = new RMSModel())
            {
                if (string.IsNullOrEmpty(result.SalesReceiptNumber))
                {
                    pt.TransactionData.Status = result.Comments.Substring(0,49);
                }
                else
                {
                    pt.TransactionData.ReferenceNumber = "QB#" + result.SalesReceiptNumber;
                    pt.TransactionData.Status = "Posted";
                }
                ctx.TransactionBase.AddOrUpdate(pt.TransactionData);
                ctx.SaveChanges();
            }
        }

        private class PostTransaction
        {
            public TransactionBase TransactionData { get; set; }
            public List<PostEntry> PostEntries { get; set; }

           
        }

        private class PostEntry
        {
            public string QBListId { get; set; }
            public int Quantity { get; set; }
        }

        private void IncludePrecriptionProperties(TransactionBase ptrn)
        {
            try
            {

                if (ptrn is Prescription)
                {

                    var pc = (ptrn as Prescription);
                    using (var ctx = new RMSModel())
                    {
                        pc.Doctor = ctx.Persons.OfType<Doctor>().FirstOrDefault(x => x.Id == pc.DoctorId);
                        pc.Patient = ctx.Persons.OfType<Patient>().FirstOrDefault(x => x.Id == pc.PatientId);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        public async Task DownloadQBItems(int days = 1)
        {
            try
            {

                

                // QBPOS pos = new QBPOS();
                List<ItemInventoryRet> itms = QBPOS.Instance.GetInventoryItemQuery(days);

               await ProcessQBItems(itms).ConfigureAwait(false);

               
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
                    //List<Medicine> clst = null;
                    //using (var ctx = new RMSModel())
                    //{
                    //    clst = ctx.Item.OfType<Medicine>()
                    //        .Where(x => x.QBItemListID != null)
                    //        // .Where(x => x.ItemNumber == "6315")
                    //        .ToList();
                    //}
                    Parallel.ForEach(itms, (item) => //.Where(x => x.ItemNumber == 6315)
                    {
                        //if (itmcnt%100 == 0)
                        //{
                        //    ctx.SaveChanges(); //SaveDatabase();
                        //}
                        using (var ctx = new RMSModel())
                        {
                            QBInventoryItem i = ctx.QBInventoryItems.FirstOrDefault(x => x.ListID == item.ListID );
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
                                    ?  .15
                                    : 0);
                                itm.QBItemListID = i.ListID;
                                itm.UnitOfMeasure = i.UnitOfMeasure;
                                itm.ItemName = i.ItemName;
                                itm.ItemNumber = i.ItemNumber.ToString();
                                itm.Size = i.Size;
                                ctx.Item.AddOrUpdate(itm);
                            }
                            ctx.SaveChanges();
                        }
                        // itmcnt += 1;
                    });
                    //SaveDatabase();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        internal async Task DownloadAllQBItems()
        {
            try
            {
                await Task.Run(() =>
                {
                    var t = QBPOS.Instance.GetAllInventoryQuery().Result;
                    ProcessQBItems(t);
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, ex.Message + ex.StackTrace);
                throw ex;
            }
        }
    }
}