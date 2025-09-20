using PrismMVVMLibrary;
using RMSDataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSalesManager
{
    public class SalesViewModel : ViewModelBase
    {

        RMSDataAccessLayer.RMSModel db;

        public SalesViewModel()
        {
            if (IsInDesignMode == false)
            db = new RMSDataAccessLayer.RMSModel();
        }

        public object MonthTicketSales
        {
            get
            {
                var ms = from s in db.TransactionBase.AsEnumerable()
                         where s.Time.Month == DateTime.Now.Month
                         group s by  s.Time.ToShortDateString() into g
                         select new
                            {
                                Date = g.Key,
                                Sales = g.Sum(s => s.TotalSales)
                            };
                return ms;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
