using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
    public partial class Batch
    {
        public Batch()
        {
            PropertyChanged += Batch_PropertyChanged;
            
        }

        private void Batch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ClosingCasher") OnPropertyChanged("CloseCashier");
        }

        public Int32 OpenTransactionsEx
        {
            get
            {
                OpenTransactions = (Int32)(from t in TransactionBase
                        where t.OpenClose == true
                        select t).Count();
                return OpenTransactions;
            }
           
        }

        public Int32 CloseTransactionsEx
        {
            get
            {
                CloseTransactions = (Int32)(from t in CloseTransactionBase
                                            where t.OpenClose == false
                                            select t).Count();

                return CloseTransactions;
            }
        }

        public double SalesEx
        {
            get
            {
                Sales = (Double)(from b in CloseTransactionBase
                                 where b.OpenClose == false
                                 from t in b.TransactionEntries
                                 select t.Amount).Sum();
                OnPropertyChanged("EndingCashEx");
                return Sales;
            }
        }

        public double TotalTenderEx
        {
            get
            {
                TotalTender = (Double)(from b in CloseTransactionBase
                                       where b.OpenClose == false
                                       from t in b.TenderEntryEx
                                       select t.CashAmount).Sum();
                return (Double)TotalTender;
            }
        }

        public double EndingCashEx
        {
            get
            {
                EndingCash = OpeningCash + Sales;
                return (Double)EndingCash;
            }
        }

        public double TotalChangeEx
        {
            get
            {
                TotalChange = TotalTender - Sales;
                return (Double)TotalChange;
            }
        }
    }
}
