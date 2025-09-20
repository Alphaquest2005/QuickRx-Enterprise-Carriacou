using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
    public partial class Customers
    {
        public Customers()
        {
            PropertyChanged += Customers_PropertyChanged;
        }

        void Customers_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CustomerType")
            {
                OnPropertyChanged("Symbol");
            }
        }
            

        public string Symbol
        {
            get
            {
                if (CustomerType == "Commercial")
                {
                    return "";
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
