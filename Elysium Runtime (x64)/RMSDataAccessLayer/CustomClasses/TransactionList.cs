using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer

{
    public class Transactionlist
    {
        RMSModel db;
        public Transactionlist()
        {
          db = new RMSModel();

        }
        public ObservableCollection<RMSDataAccessLayer.TransactionBase> TransactionList
        {
            get
            {
                return new ObservableCollection<TransactionBase>(db.TransactionBase);
            }
        }
    }
}
