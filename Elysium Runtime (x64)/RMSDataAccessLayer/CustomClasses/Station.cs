using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
    public partial class Station
    {
        public Station()
        {
            StoreReference.AssociationChanged += StoreReference_AssociationChanged;
        }

        void StoreReference_AssociationChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            OnPropertyChanged("Store");
        }
    }
}
