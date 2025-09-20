using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Data;

namespace RMSDataAccessLayer
{
    public partial class PrescriptionEntry : ISearchItem
    {

        //TODO: implement this in the view when item is selected. cache items
        //IEnumerable<string> doseList = new List<string>();
        //public IEnumerable<string> DosageList
        //{
        //    get
        //    {
        //        //if (Item != null)
        //        //{
        //        //    if (Item != null)
        //        //    {
        //        //        var list = (from p in Item.TransactionEntryBase.OfType<PrescriptionEntry>()
        //        //                    where p.ItemId == Item.ItemId
        //        //                    group p by p.Dosage into g
        //        //                    orderby g.Count() descending
        //        //                    select g.Key).Take(5);

        //        //        BindingOperations.EnableCollectionSynchronization(list, _syncLock);

        //        //        return list;
        //        //    }
        //        //}

        //        return null;
        //    }
        //}



        //void PrescriptionEntry_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //if (e.PropertyName == "Price")
        //{
        //    // auto change price of items
        //    if (Item != null)
        //    {
        //        Item.Price = Price;
        //    }
        //}

        //if (e.PropertyName == "ExpiryDate")
        //{
        //    // auto change price of items
        //    if (Item != null && typeof(Medicine).IsInstanceOfType(Item))
        //    {
        //       ((Medicine)Item).ExpiryDate = ExpiryDate;
        //    }
        //}


        //if (Transaction != null)
        //{
        //    if (Dosage == null && Transaction.Status != "Please Enter Dosage")
        //    {

        //        Transaction.Status = "Please Enter Dosage";
        //    }
        //    else
        //    {
        //        Transaction.Status = null;
        //    }
        //}
        //}





        #region ISearchItem Members
        [NotMapped]
        [IgnoreDataMember]
        public string SearchCriteria
        {
            get
            {
                return DisplayName + "|";
            }
            set
            {
               
            }
        }
        [NotMapped]
        [IgnoreDataMember]
        public string DisplayName
        {
            get { return ""; } //this.Item.DisplayName; 
            }
        [NotMapped]
        [IgnoreDataMember]
        public string Key
        {
            get { return TransactionEntryNumber; }
        }

        #endregion
    }
}
