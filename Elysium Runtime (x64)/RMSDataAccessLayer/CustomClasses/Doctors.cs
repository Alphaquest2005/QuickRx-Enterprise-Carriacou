using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
    public partial class Doctor
    {

        
        public new string SearchCriteria
        {
            get
            {
                return DisplayName + " " + Code;
            }
            set
            {
                
            }
        }

        public new string DisplayName
        {
            get { return Salutation + " " + FirstName + " " + LastName; }// base.Salutation + " " +
        }


    }
}
