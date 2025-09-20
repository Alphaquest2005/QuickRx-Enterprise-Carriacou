using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrismMVVMLibrary.DesignTime;
using RMSDataAccessLayer;

namespace Transaction.Design
{
    public class TransactionVMDesign : TransactionVM
    {
        /// <summary>
        /// Design constructor.  Design Time data can be created here.
        /// </summary>
        public TransactionVMDesign()
            : base(new DesignUnityContainer(), new DesignEventAggregator())
        {
            ////+ Set your design data here.  It will show up in expression blend and your designer.
            //RMSDataAccessLayer.Transaction testTrans = new RMSDataAccessLayer.Transaction { TransactionNumber = 1 };
            //testTrans.TransactionEntries.Add(new TransactionEntry { TransactionId = 1, TransactionEntryId = 1, ItemId = 4600, Price = 10, Quantity = 3 });
            //testTrans.TransactionEntries.Add(new TransactionEntry { TransactionId = 1, TransactionEntryId = 2, ItemId = 4601, Price = 50, Quantity = 1 });
            //TransactionData = testTrans;
        }
    }
}
