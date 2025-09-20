using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
    public partial class Patient
    {
        public string ContactInfo
        {
            get
            {
                return DisplayName + " - " + Address + " - " + PhoneNumber;
            }
        }
    }
}
