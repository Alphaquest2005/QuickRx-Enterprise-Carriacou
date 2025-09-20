using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;


namespace RMSDataAccessLayer
{
    public partial class TicketEntry: ISearchItem
    {
      public static DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
      public TicketEntry()
      {
          timer.Interval = new TimeSpan(0, 0, 1);
          timer.Tick += timer_Tick;
          TransactionReference.AssociationChanged += TransactionReference_AssociationChanged;
          StartDateTime = DateTime.Now;
          if (EndDateTime == null)
          {
              timer.Start();
          }
          else
          {
              timer.Stop();
          }

      }

      void TransactionReference_AssociationChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e)
      {
          if (e.Action == System.ComponentModel.CollectionChangeAction.Remove)
          {
              timer.Stop();
          }

      }

      void timer_Tick(object sender, EventArgs e)
      {
          OnPropertyChanged("Quantity");
          OnPropertyChanged("Amount");
          Transaction.TotalSales = 0;
          
      }
   

        

        public string Status
        {
            get
            {
                if (EndDateTime == null)
                {
                    return "Open";
                }
                else
                {
                    return "Closed";
                }
            }
           
        }

        public override Decimal Amount
        {
            get
            {
                if (Item == null) return 0;
                
                TicketItem  ticitm = ((TicketItem)Item);
                
                if (Transaction.GetType().IsInstanceOfType(typeof(Ticket)))
                {
                    Ticket tic = (Ticket)Transaction;

                    if (tic.Pass != null && tic.Pass.FreePass == true)
                    {
                        if (Price != ticitm.Price1)
                            Price = ticitm.Price1;
                        return 0;
                    }
                }
                if (Item != null && ticitm.TicketSetup != null && Quantity.TotalMinutes * -1 <= ticitm.TicketSetup.FreeMinutes)
                {
                    if (Price != ticitm.Price1)
                    Price = ticitm.Price1;
                    
                    return 0;
                }
                
                    
                    double hours = Quantity.TotalHours * -1;
                    if (hours <= 1.05)
                    {
                        if (Price != ticitm.Price1)
                            Price = ticitm.Price1;
                       
                       
                        return ticitm.Price1;
                    }
                    else
                    {
                        if (Price != ticitm.Price2)
                            Price = ticitm.Price2;
                    
                        
                        return (ticitm.Price1) + ((ticitm.Price2) * Math.Floor((System.Convert.ToDecimal(hours - 0.05))));
                    }
               
            }
        }


        public  Nullable<global::System.DateTime>  EndDateTimeEx
        {
            get
            {
                return EndDateTime;
            }
            set
            {
                EndDateTime = DateTime.Now;
                timer.Stop();

                // set the discount
                
                
                OnPropertyChanged("EndDateTimeEx");
                OnPropertyChanged("Status");
            }
        }

        public new TimeSpan Quantity
        {
            get
            {
                if (EndDateTime == null)
                {
                    return (StartDateTime - DateTime.Now);
                }
                else
                {
                    return (StartDateTime - (DateTime)EndDateTime); 
                }
            }

        }
        public string SearchCriteria
        {
            get { return TransactionEntryId.ToString(); }
            set
            {
            }
        }

        public string DisplayName
        {
            get { throw new NotImplementedException(); }
        }

        public string Key
        {
            get { throw new NotImplementedException(); }
        }
    }
}
