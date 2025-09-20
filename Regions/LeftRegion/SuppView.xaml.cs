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
using System.Threading;
using System.Threading.Tasks;

namespace LeftRegion
{
    /// <summary>
    /// Interaction logic for TransactionView.xaml
    /// </summary>
    public partial class SuppView : UserControl
    {
        public SuppView()
        {
            InitializeComponent();

            // Setup the view model context
            DataContext = SuppVM.Instance;
            tvm = SuppVM.Instance;
           

        }
        private SuppVM tvm;
        private void SearchPatient(object sender, RoutedEventArgs e)
        {

        }

        private void SearchMedication(object sender, RoutedEventArgs e)
        {

        }

        private void SearchDoctor(object sender, RoutedEventArgs e)
        {

        }

        private void SearchPrescriptions(object sender, RoutedEventArgs e)
        {

        }

        private async void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e == null) return;
            if (e.Key == Key.Enter)
            {
                if (tvm != null)
                {
                    tvm.SearchResults = new System.Collections.ObjectModel.ObservableCollection<Prescription>(){};
                
                    
                    Status.Text = "Searching";

                   
                        if (Application.Current != null)
                            Application.Current.Dispatcher.Invoke(
                                () => { if (SearchBox != null) tvm.SearchText = SearchBox.Text; });

                        await Task.Run(() => tvm.SearchPrescriptions()).ConfigureAwait(false);

                        if (Application.Current != null)
                            Application.Current.Dispatcher.Invoke(
                                () => { Status.Text = ""; });
                    
                   
                }
            }
        }

        private Prescription GetTransactionData(Prescription searchView)
        {
            using (var ctx = new RMSModel())
            {
                return ctx.TransactionBase.OfType<Prescription>().FirstOrDefault(x => x.TransactionId == searchView.TransactionId);
            }
        }


        private void AutoRepeatBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tvm == null) return;
            var i = sender as FrameworkElement;
            if (i != null)
                tvm.TransactionData = GetTransactionData((Prescription)i.DataContext);
            tvm.AutoRepeat();
        }


        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (tvm != null)
            {
                var frameworkElement = sender as FrameworkElement;
                if (frameworkElement != null)
                    tvm.TransactionData = GetTransactionData((Prescription)frameworkElement.DataContext);
            }
        }

        private void NewPrescription(object sender, RoutedEventArgs e)
        {
            if (tvm != null)
            {
                var frameworkElement = sender as FrameworkElement;
                if (frameworkElement != null)
                    tvm.TransactionData = GetTransactionData((Prescription)frameworkElement.DataContext);
                tvm.CopyPrescription();
            }
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SalesRegion.SalesVM.Instance.ShowInactiveItems = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SalesRegion.SalesVM.Instance.ShowInactiveItems = false;
        }
       


       

       

    }
}
