using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
    public partial class Ticket
    {
        public Ticket()
        {
            PassReference.AssociationChanged += PassReference_AssociationChanged;
            
        }

        void PassReference_AssociationChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            if (e.Action == System.ComponentModel.CollectionChangeAction.Add)
            {
                RMSDataAccessLayer.Pass p = (RMSDataAccessLayer.Pass)e.Element;
                if (p.StartDate <= DateTime.Now && p.EndDate  > DateTime.Now)
                {

                }
                else
                {
                    Status = "Invalid Pass";
                    OnPropertyChanged("Status");
                }
                
            }
         }
    }
}
