using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RMSDataAccessLayer;

namespace Transaction
{
    /// <summary>
    /// Interaction logic for TransactionView.xaml
    /// </summary>
    public partial class TransactionView : UserControl
    {
        public TransactionView(TransactionVM transactionVM)
        {
            InitializeComponent();

            // Setup the view model context
            DataContext = transactionVM;
            tvm = transactionVM;
            ObservableObject<object> viewRegionContext =
               RegionContext.GetObservableContext(this);
            viewRegionContext.PropertyChanged += this.ViewRegionContext_OnPropertyChangedEvent;

        }
        private TransactionVM tvm;
        private void ViewRegionContext_OnPropertyChangedEvent(object sender,
                   PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var context = (ObservableObject<object>)sender;
                 tvm.TransactionData =(RMSDataAccessLayer.TransactionBase) context.Value;
            }
        }

       

    }
}
