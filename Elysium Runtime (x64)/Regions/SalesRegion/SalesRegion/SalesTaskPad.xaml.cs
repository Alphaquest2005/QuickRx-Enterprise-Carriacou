using RMSDataAccessLayer;
using System;
using System.Collections.Generic;
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
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Drawing.Printing;
using System.Printing;
using System.Windows.Xps;
using SUT.PrintEngine;
using SUT.PrintEngine.Utils;
using System.IO;


namespace SalesRegion
{

    /// <summary>
    /// Interaction logic for SalesTaskPad.xaml
    /// </summary>
    public partial class SalesTaskPad : UserControl
    {
        public SalesTaskPad()
        {
            InitializeComponent();
            this.DataContextChanged += SalesTaskPad_DataContextChanged;
           
          
        }

        void SalesLst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReBindItemEditor();
        }

                 
        void SalesTaskPad_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SearchBox.Focus();
           // ReBindTranStatusTxt();
        }

        private void ReBindTranStatusTxt()
        {
            Binding myBinding = new Binding("Status");
            myBinding.Source = SalesVM.TransactionData;
            ((FrameworkElement)SalesPad.FindName("TransStatusTxt")).SetBinding(TextBlock.TextProperty, myBinding);
        }


        private void ReBindItemEditor()
        {
            Binding myBinding = new Binding("SelectedItem");
            myBinding.Source = SalesView.SalesLst;
            myBinding.Mode = BindingMode.OneWay;

            ((FrameworkElement)SalesPad.FindName("ItemEditor")).SetBinding(ContentControl.ContentProperty, myBinding);
        }



        //public static DependencyProperty TransactionProperty = DependencyProperty.Register("Transaction", typeof(TransactionBase), typeof(SalesTaskPad), new PropertyMetadata(new Transaction(), SalesTaskPad.TransactionPropertyChanged));

        //private static void TransactionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    // transaction changed
        //}
        //public TransactionBase Transaction
        //{
        //    get
        //    {
        //        return (TransactionBase)GetValue(TransactionProperty);
        //    }
        //    set
        //    {
        //        SetValue(TransactionProperty, value);
        //    }
        //}

        public TransactionBase Transaction
        {
            get
            {
                return SalesVM.TransactionData;
            }
        }
    

        private void SalesPad_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {

            var uie = e.OriginalSource as Control;

           if (uie == null) uie = SearchBox as Control;
            if (uie.Name == "PART_FilterBox")
            {
                if (e.Key == Key.Enter)//pkey == Key.Enter &&
                {
                    SearchBox.RaiseFilterEvent();
                    if (SearchListCtl.Items.Count == 1)
                        SearchListCtl.SelectedIndex = 0;
                    (uie as TextBox).Text = "";
                    //e.Handled = true;
                    MoveToNextControl(uie);
                }

                return;
            }

            if (e.Key == Key.Enter)
            {
                // e.Handled = true;
                MoveToNextControl(uie);
            }
            pkey = e.Key;
        }

        public static void MoveToNextControl(object sender)
        {
            UIElement uie = sender as UIElement;
            uie.MoveFocus(
            new TraversalRequest(
            FocusNavigationDirection.Next));
        }

        private void FilterControl_Direction_1(object sender, DirectionEventArgs e)
        {
            if (e.Direction == DirectionEnum.Down)
            {
                SearchListCtl.SelectedIndex += 1;
            }
            if (e.Direction == DirectionEnum.Up && SearchListCtl.SelectedIndex > -1)
            {
                SearchListCtl.SelectedIndex -= 1;
            }
            if (e.Direction == DirectionEnum.Right)
            {

                MoveToNextControl(sender);
            }
        }
        static Key pkey;
        private void SearchBox_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
           
            if (e.Key == Key.Delete)
            {
                DeleteTransactionEntry();
            }

            if (SearchListCtl.Items.Count == 1)
                SearchListCtl.SelectedIndex = 0;

            if (e.Key == Key.Enter)
            {
                
                //select the item
                LocalProcesItem(SearchListCtl.SelectedItem);
               

                MoveToNextControl(ItemEditor);
            }
            if (pkey == Key.Enter && e.Key == Key.Enter)
            {
                SalesView.GotoNextSalesStep(e.Key);
            }

            pkey = e.Key;
           
        }

        private void DeleteTransactionEntry()
        {
            if (SalesVM.TransactionData.CurrentTransactionEntry != null)
            {
                SalesVM.DeleteTransactionEntry<TransactionEntryBase>(SalesVM.TransactionData.CurrentTransactionEntry);
            }
        }
     
        private void LocalProcesItem(object itm)
        {
            if (itm == null) return;
            if (itm.GetType() == typeof(RMSDataAccessLayer.SearchItem))
            {
                switch (((ISearchItem)itm).DisplayName)
                {
                    
                    case "Add Patient":
                        Patient p = SalesVM.CreateNewEntity<Patient>();
                        SalesVM.rms.Persons.AddObject(p);
                        ItemEditor.Content = p;
                        break;
                    case "Add Doctor":
                        Doctor d = SalesVM.CreateNewEntity<Doctor>();
                        SalesVM.rms.Persons.AddObject(d);
                        ItemEditor.Content = d;
                        break;

                    default:
                        ItemEditor.Content = ((RMSDataAccessLayer.SearchItem)itm).SearchObject;
                        break;
                }
                
            }
            else if (itm.GetType() == typeof(RMSDataAccessLayer.Pass) && ((RMSDataAccessLayer.Pass)itm).Status.Contains("Invalid") == true)
            {
                TransStatusTxt.Text = "Invalid Pass";
                TransStatusTxt.Focus();
            }
            else
            {
                SalesVM.ProcessSearchListItem(itm);
                MoveToNextControl(ItemEditor);
            }
        }


        SalesView _SalesView;
        public SalesView SalesView
        {
            get
            {
                return _SalesView;
            }
            set
            {
                _SalesView = value;
                SalesView.SalesLst.SelectionChanged += SalesLst_SelectionChanged;
            }
        }

        SalesVM _SalesVM;
        public SalesVM SalesVM
        {
            get
            {
                return _SalesVM;
            }
            set
            {
                _SalesVM = value;
                SearchListCtl.ItemsSource = SalesVM.SearchList;
               // SalesVM.TransactionData.PropertyChanged += TransactionData_PropertyChanged;
                
                
            }
        }






        private void SearchBox_Filter_1(object sender, FilterEventArgs e)
        {
            if (e.FilterText != "" && e.IsFilterApplied == false)
            {
                ShowSearchList();

            }
            if (e.FilterText == "")
            {
                HideSearchList();
               

            }
           
        }

        private void HideSearchList()
        {
            SearchListCtl.Visibility = System.Windows.Visibility.Collapsed;
            SearchListCtl.Focusable = false;
            SearchListCtl.SelectedIndex = -1;
        }

        private void ShowSearchList()
        {
            SearchListCtl.Visibility = System.Windows.Visibility.Visible;
            SearchListCtl.Focusable = true;
            if (SearchListCtl.Items.Count == 1)
                SearchListCtl.SelectedIndex = 0;
        }



        private void TextBox_GotKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void CheckTxt_TextInput_1(object sender, TextCompositionEventArgs e)
        {
            PaymentInfo.Visibility = System.Windows.Visibility.Visible;
            PaymentInfo.Text = "Enter CheckInfo Here!";

        }
        
        
        public void PrintBtn_Click_1(object sender, RoutedEventArgs e)
        {
            
            
            if (ReceiptCol.Width == new GridLength(0) )
            {
               // unhide the colums to print
                ReceiptCol.Width = new GridLength(400);
                ReceiptGrd.UpdateLayout();
 SalesVM.Print(ref ReceiptGrd);
 ReceiptCol.Width = new GridLength(0);
               //hide it back
            }
            else
            {
                SalesVM.Print(ref ReceiptGrd);
            }
         
        }




        private void CloseTick_Click_1(object sender, RoutedEventArgs e)
        {
           SalesVM.CloseTicket();
           // PrintBtn_Click_1(sender, null);
        }

    

        private void NewTransaction(object sender, RoutedEventArgs e)
        {
            SalesVM.NewTransaction();
            if (SalesVM.ApplicationMode == ApplicationMode.Ticket)
            {
                SalesView.HideTransaction();
                SalesView.ShowReceipt();
                SalesView.UpdateLayout();
                PrintBtn_Click_1(sender, null);
                SalesView.HideReceipt();
                SalesView.ShowTransaction();

            }
            //save transaction
           // SalesPadSalesView.CreateNewTransaction<TransactionBase>();
        }
        static bool focusswitch;
        private void TransStatusTxt_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (focusswitch == true)
            {
                ReBindTranStatusTxt();
            }
            
                focusswitch = !focusswitch;
           
        }

        private void ListView_SourceUpdated_1(object sender, DataTransferEventArgs e)
        {
            
        }
        int TemplateHeight = 192;
        private void PrescriptionEntriesRptLst_LayoutUpdated(object sender, EventArgs e)
        {
            if (SalesVM.TransactionData != null && SalesVM.ApplicationMode == ApplicationMode.Pharmacy)
                ((FrameworkElement)this.FindName("ReceiptGrd")).Height = TemplateHeight * SalesVM.TransactionData.TransactionEntries.Count;
        }

        private void SearchListCtl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Grid_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
LocalProcesItem(SearchListCtl.SelectedItem);
            MoveToNextControl(sender);
        }

        private void SearchListCtl_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            LocalProcesItem(SearchListCtl.SelectedItem);
            MoveToNextControl(sender);
            SearchBox.FilterText = "";
            HideSearchList();
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowSearchList();
        }

        private void NextEntry_Click(object sender, RoutedEventArgs e)
        {
            SalesView.pkey = Key.Down;
            SalesView.GoToNextTransactionEntry();
            
        }

        private void DeleteTranBtn_Click(object sender, RoutedEventArgs e)
        {
            DeleteTransactionEntry();
        }

        private void BackBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SalesView.GotoPreviousSalesStep();
        }

        private void NextBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SalesView.GotoNextSalesStep(Key.Right);
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                SalesVM.TransactionData = e.AddedItems[0] as TransactionBase;
                ReBindItemEditor();
            }
        }

        private void SavePatientBtn_Click(object sender, RoutedEventArgs e)
        {

           
           SalesVM.SaveTransaction();
            // add patient to transaction
             LocalProcesItem(ItemEditor.Content);
            ReBindItemEditor();
        }

      



        




    }


}
