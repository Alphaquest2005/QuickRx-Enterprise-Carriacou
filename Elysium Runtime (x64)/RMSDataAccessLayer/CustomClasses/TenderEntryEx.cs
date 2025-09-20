using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
    public partial class TenderEntryEx
    {
        public TenderEntryEx()
        {
            PropertyChanged += TenderEntryEx_PropertyChanged;
        }

        void TenderEntryEx_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CashAmount" || e.PropertyName == "CheckAmount" || e.PropertyName == "CreditCardAmount" || e.PropertyName == "AccountAmount")
            {
                if (this.Transaction != null)
                {
                    this.Transaction.TotalTender = 0;
                }
            }

        }
    }
}
