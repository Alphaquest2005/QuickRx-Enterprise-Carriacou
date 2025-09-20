using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
    public partial class PrescriptionEntry
    {
        public PrescriptionEntry()
        {
            PropertyChanged += PrescriptionEntry_PropertyChanged;
            Dosage = "";
            ItemReference.AssociationChanged += ItemReference_AssociationChanged;
       
        }

        private void ItemReference_AssociationChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            if (Item != null && typeof(Medicine).IsInstanceOfType(Item))
            {
                Dosage = ((Medicine)Item).SuggestedDosage;
            }
        }

        partial void OnDosageChanging(global::System.String value)
        {
            //if (value == "")
            //{
            //    AddError("Dosage", "Please Enter Dosage");
            //}
            //else
            //{
            //    RemoveError("Dosage");
            //}
        }

        void PrescriptionEntry_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
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
        }
    }
}
