using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
    public partial class Pass: ISearchItem
    {
        public Pass()
        {
            //PropertyChanged += Pass_PropertyChanged;
        }

    
        public string Status
        {
            get
            {
                if (StartDate <= DateTime.Now && EndDate > DateTime.Now)
                {
                    return "Pass Valid";
                }
                else
                {
                    return "Invalid Pass";

                }

            }
           
        }

        //void Pass_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "PassId")
        //    {
        //        BarCodes.UPCA.cUPCA barcode = new BarCodes.UPCA.cUPCA();
        //        string txn = PassId.ToString().PadLeft(11, '0');
        //        PassNumber = txn + barcode.GetCheckSum(txn).ToString();
        //    }
        //}

        public string SearchCriteria
        {
            get
            {
                return PassNumber;
            }
            set
            {
               
            }
        }

        public string DisplayName
        {
            get { return PassNumber; }
        }

        public string Key
        {
            get { return PassNumber;  }
        }
    }
}
