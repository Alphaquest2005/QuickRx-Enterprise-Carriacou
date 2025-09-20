using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace RMSDataAccessLayer
{
    public abstract partial class TransactionBase: ISearchItem, IDataErrorInfo
    {
        public TransactionBase()
        {
            TransactionEntries.AssociationChanged += TransactionEntries_AssociationChanged;
            
            TenderEntryEx.AssociationChanged += TenderEntryEx_AssociationChanged;
            ValidationErrorMsg = "Transaction has Errors";
            PropertyChanged += TransactionBase_PropertyChanged;
      
        }

        void TransactionBase_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "StationId") OnPropertyChanged("Station");
            if (e.PropertyName == "CashierId") OnPropertyChanged("Cashier");
            if (e.PropertyName == "CustomerId") OnPropertyChanged("Customer");
            if (e.PropertyName == "BatchId") OnPropertyChanged("Batch");

        }





        void TenderEntryEx_AssociationChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            OnPropertyChanged("TotalTender");
            OnPropertyChanged("TotalChange");

        }

        void TransactionEntries_AssociationChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            if (e.Action == System.ComponentModel.CollectionChangeAction.Add)
            {
                if (Status != null)
                {
                    
                }
            }

           TotalSales = 0;
        }

    

        public Decimal TotalTender
        {
            get
            {
                Decimal tx =(Decimal) (from t in TenderEntryEx
                                       select t.CashAmount + t.CheckAmount + t.CreditCardAmount + t.AccountAmount).Sum();
                return tx;
            }
            set
            {
                OnPropertyChanged("TotalTender");
                OnPropertyChanged("TotalChange");

            }
        }

        public Decimal TotalChange
        {
            get
            {
                return TotalTender - TotalSales;
            }
        }

        public Decimal TotalSales
        {
            get
            {
                Decimal t = (from te in TransactionEntries
                             select te.Amount).Sum();
                return t;
            }
            set
            {
                OnPropertyChanged("TotalSales");
                OnPropertyChanged("TotalTax");
                OnPropertyChanged("TotalDiscount");
                OnPropertyChanged("TotalTender");
                OnPropertyChanged("TotalChange");  
            }
        }
        public Decimal TotalTax
        {
            get
            {
                Decimal t = (from te in TransactionEntries
                             where te.Taxable == true 
                             select te.SalesTax).Sum();
                return t;
            }
            
        }
        public Decimal TotalDiscount
        {
            get
            {
                Decimal t = (Decimal)(from te in TransactionEntries
                                      select te.Discount).Sum();
                return t;
               
            }
        }

        RMSDataAccessLayer.TransactionEntryBase _currentTransactionEntry;
        public RMSDataAccessLayer.TransactionEntryBase CurrentTransactionEntry
        {
            get { return _currentTransactionEntry; }
            set{
                _currentTransactionEntry = value;
                OnPropertyChanged("CurrentTransactionEntry");
            }

        }

        public string SearchCriteria
        {
            get { return TransactionNumber ; }
            set
            {
            }
        }

        public string DisplayName
        {
            get { return "TransactionList"; }
        }

        public string Key
        {
            get { return "TransactionList"; }
        }


        #region "Validation"
        Dictionary<string, string> m_validationErrors = new Dictionary<string, string>();
        public void AddError(string col, string msg)
        {
            if (!m_validationErrors.ContainsKey(col))
            {
                m_validationErrors.Add(col, msg);
            }
        }
        public void RemoveError(string col)
        {
            if (m_validationErrors.ContainsKey(col))
            {
                m_validationErrors.Remove(col);
            }
        }
        public string ValidationErrorMsg { get; set; }
        public virtual string Error
        {
            get 
            {
                if (m_validationErrors.Count > 0)
                {
                    return ValidationErrorMsg;
                }
                else
                {
                    return null;
                }
            }
        }

        public string this[string columnName]
        {
            get 
            {
                if (m_validationErrors.ContainsKey(columnName))
                {
                    return m_validationErrors[columnName];
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
    }

}
