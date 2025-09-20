using RMSDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using BarCodes;

namespace SalesRegion
{
    /// <summary>
    /// Interaction logic for SalesView.xaml
    /// </summary>
    public partial class SalesView : UserControl
    {
        public SalesView(SalesVM salesVM)
        {
            InitializeComponent();
            DataContext = salesVM;
            salesvm = salesVM;
            SalesPad.SalesVM = salesVM;
            SalesPad.SalesView = this;
            
            salesVM.PropertyChanged += salesVM_PropertyChanged;
         
            ((INotifyCollectionChanged)SalesLst.Items).CollectionChanged += SalesView_CollectionChanged;

                      
            SalesLst.SelectionChanged += SalesLst_SelectionChanged;
          //  SalesPad.LayoutUpdated += SalesPad_LayoutUpdated;
            //SalesLst.SizeChanged += SalesLst_SizeChanged;
            
            SalesLst.LayoutUpdated += SalesLst_LayoutUpdated;
           // salesVM.ParentCanvas = ppcan;
            HideChange();
            HideTender();
            HideReceipt();
            ShowTransaction();

            SetUpSalesPad();

        }

        private void SetUpSalesPad()
        {
            Canvas.SetTop(SalesLst, 0);
            SalesPad.Margin = SalesPadMargin;
            Canvas.SetTop(SalesPad, 0);
            SetSalesPadtoSelectedItem();
        }

        void salesVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SearchList")
            {
                SalesPad.SearchListCtl.ItemsSource = salesvm.SearchList;
            }
            if (e.PropertyName == "TransactionData")
            {
                SalesLst.SelectedIndex = 0;
                pkey = Key.Up;
            }
           
        }



        //void salesVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "TransactionData")
        //    {
        //        SalesPad.Transaction = salesvm.TransactionData;
        //    }
        //}

        //void SalesPad_TransactionChanged(object sender, EventArgs e)
        //{
        //    if (salesvm.TransactionData != SalesPad.Transaction)
        //    {
        //        salesvm.TransactionData = SalesPad.Transaction;
        //        SalesLst.SelectedIndex = 0;
        //    }
        //}


        void SalesLst_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas.SetTop(SalesPad, SalesLst.ActualHeight + 8);
        }

     
        

        void SalesView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                SalesLst.ScrollIntoView(e.NewItems[0]);
                SalesLst.SelectedItem = e.NewItems[0];
                salesvm.TransactionData.CurrentTransactionEntry =(RMSDataAccessLayer.TransactionEntryBase) SalesLst.SelectedItem;
                padPos = PadPosition.Middle;

            }
        }

        enum PadPosition
        {
            Above,
            Middle,
            Below,
        }
        PadPosition padPos = PadPosition.Middle;
        void SalesLst_LayoutUpdated(object sender, EventArgs e)
        {
            SalesPad_LayoutUpdated(null, null);
        }
        Thickness SalesPadMargin = new Thickness(0, 0, 0, 0);
        void SalesPad_LayoutUpdated(object sender, EventArgs e)
        {
            if (pkey != Key.Down && pkey != Key.Up)
            {
                if (SalesLst.SelectedIndex != -1)
                {
                    Canvas.SetTop(SalesLst, 0);
                    Canvas.SetTop(SalesPad, 0);
                    SetSalesPadtoSelectedItem();
                    return;
                }
            }
            if (pkey == Key.Up)
            {
                SalesPad.Margin = SalesPadMargin;
                SalesLst.UpdateLayout();

                if (padPos == PadPosition.Middle && SalesLst.SelectedIndex == -1)
                {

                    Canvas.SetTop(SalesPad, 0);
                    Canvas.SetTop(SalesLst, SalesPad.ActualHeight + 8);
                    //SetSalesPadtoSelectedItem();
                    if(SalesLst.SelectedIndex == -1) padPos = PadPosition.Above;
                    //return;
                }
                if (padPos == PadPosition.Middle && SalesLst.SelectedIndex != -1)
                {
                    Canvas.SetTop(SalesLst, 0);
                    Canvas.SetTop(SalesPad, 0);
                    SetSalesPadtoSelectedItem();
                    
                }

                if (SalesLst.SelectedIndex != -1 && padPos == PadPosition.Above)
                {
                    // set the index to the last one and goto that one
                    Canvas.SetTop(SalesLst, 0);
                    //SalesLst.SelectedIndex = SalesLst.Items.Count - 1;
                    SetSalesPadtoSelectedItem();
                    padPos = PadPosition.Middle;
                }

                if (padPos == PadPosition.Below)
                {
                    // set the index to the last one and goto that one

                    Canvas.SetTop(SalesLst, 0);
                    Canvas.SetTop(SalesPad, 0);
                    SetSalesPadtoSelectedItem();
                    padPos = PadPosition.Middle;
                    SalesLst.SelectedIndex = SalesLst.Items.Count - 1;
                    
                    
                   
                }
                pkey = Key.None;
            }
            if (pkey == Key.Down)
            {
                MoveSalesPadDown();
                pkey = Key.None;
            }
            

           // pkey = Key.None;
            //}


        }

        public void MoveSalesPadDown()
        {
            
            SalesLst.UpdateLayout();
            SalesPad.Margin = SalesPadMargin;


            if (padPos == PadPosition.Above && SalesLst.SelectedIndex != -1)
            {
                Canvas.SetTop(SalesLst, 0);
                SetSalesPadtoSelectedItem();
                padPos = PadPosition.Middle;
            }

            if (padPos == PadPosition.Middle)
            {
                if (SalesLst.SelectedIndex == -1)
                {
                    Canvas.SetTop(SalesLst, 0);
                    // SalesPad.Margin = SalesPadMargin;
                    Canvas.SetTop(SalesPad, SalesLst.ActualHeight + 8);
                    SetSalesPadtoSelectedItem();
                    if (SalesLst.SelectedIndex == -1) padPos = PadPosition.Below;
                }
                else
                {
                    Canvas.SetTop(SalesLst, 0);
                    SetSalesPadtoSelectedItem();
                }
              
            }

            if (SalesLst.SelectedIndex != -1 && padPos == PadPosition.Below)
            {
                Canvas.SetTop(SalesLst, 0);
                Canvas.SetTop(SalesPad, 0);
                SalesPad.Margin = SalesPadMargin;
                SetSalesPadtoSelectedItem();
                if (SalesLst.SelectedIndex == 0) padPos = PadPosition.Middle;
               // padPos = PadPosition.Below;
            }
        }

   
        SalesVM salesvm;
        FrameworkElement f;
        void SalesLst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Rect r = ((FrameworkElement)SalesLst.ItemContainerGenerator.ContainerFromItem(SalesLst.SelectedItem)).TransformToAncestor(ppcan).TransformBounds(new Rect(0,0,0,0));
            //SalesPad.Margin = new Thickness(r.Left, r.Top, r.Right, r.Bottom);
            if (salesvm.TransactionData == null) return;
            if (SalesLst.Items.Count > 0 && SalesLst.SelectedItem == null )
            {
                if(salesvm.ApplicationMode == ApplicationMode.Ticket) SalesLst.SelectedIndex = 0;//
            }
            salesvm.TransactionData.CurrentTransactionEntry = (RMSDataAccessLayer.TransactionEntryBase)SalesLst.SelectedItem;
           // f = (FrameworkElement)SalesLst.ItemContainerGenerator.ContainerFromItem(SalesLst.SelectedItem);
           
                SalesPad_LayoutUpdated(null, null);
              //  SetSalesPadtoSelectedItem();
                //if (f != null)
                //{
                //    f.LayoutUpdated += f_LayoutUpdated;
                //}
            //SalesRegion.SalesTaskPad.MoveToNextControl(SalesPad.TransactionGrd);
            SalesPad.SearchBox.Focus();
            
        }

        void f_LayoutUpdated(object sender, EventArgs e)
        {
           
                SetSalesPadtoSelectedItem();
          
        }

        private void SetSalesPadtoSelectedItem()
        {
            SalesLst.UpdateLayout();
            f = (FrameworkElement)SalesLst.ItemContainerGenerator.ContainerFromItem(SalesLst.SelectedItem);
            if (f != null )//&& f.Parent != null
            {
                Rect r = ((FrameworkElement)f).TransformToAncestor(ppcan).TransformBounds(new Rect(0, 0, 0, 0));
                SalesPad.Margin = new Thickness(r.Left, r.Top, r.Right, r.Bottom);
               
            }
            
        }

        static SalesRegion.SalesPadTransState SalesPadState = SalesPadTransState.Transaction;
       public static Key pkey;

        public void ppcan_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            
            var uie = e.OriginalSource as Control;

            if (uie == null) uie = SalesPad.SearchBox as Control;

            if ( uie.Name == "PART_FilterBox" && ((uie as TextBox).Text != "") && (e.Key == Key.Up || e.Key == Key.Down))
            {
               
                return;
            }

            if (uie.Name == "PrintBtn" && e.Key == Key.Enter)
            {
                if (pkey == Key.Enter) GotoNextSalesStep(e.Key);
                e.Handled = true;
                pkey = e.Key;
                return;
            }

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.C)
            {
                if (salesvm.ApplicationMode == ApplicationMode.Ticket)
                {
                    salesvm.CloseTicket();
                    e.Handled = true;
                }
            }
             if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.P)
            {
                if (SalesPadState != SalesPadTransState.Receipt)
                {
                    HideCurrentSalesPadState();
                    // unhide the colums to print
                    ShowReceipt();
                    salesvm.Print(ref SalesPad.ReceiptGrd);
                    e.Handled = true;
                    //hide it back
                }
                else
                {
                    salesvm.Print(ref SalesPad.ReceiptGrd);
                    e.Handled = true;
                }
               
            }

           
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                GoToNextTransactionEntry();
            }
            else if(e.Key == Key.Up)
            {
                GoToPreviousTransactionEntry(e);
            }
            else if (e.Key == Key.Right)
            {
                GotoNextSalesStep(e.Key);       

            }
            else if (e.Key == Key.Left)
            {
                e.Handled = true;
                GotoPreviousSalesStep();

            }
            //else if (e.Key == Key.P && (SalesPadState == SalesPadTransState.Change || SalesPadState == SalesPadTransState.Receipt))
            //{
            //    SalesPad.PrintBtn_Click_1(sender, null);

            //}

            else
                if (e.Key == Key.Enter)
                {
                  
                    if (pkey == Key.Enter)
                    {
                        e.Handled = true;
                        

                        if (SalesLst.SelectedItem != null && salesvm.ApplicationMode!= ApplicationMode.Ticket)
                        {
                            GotoBlankTransactionEntry();
                            
                        }
                        else
                        {
                           GotoNextSalesStep(e.Key);
                        }

                       
                        pkey = Key.None;
                        return;
                    }
                    
                }
            pkey = e.Key;
        }

        private void HideCurrentSalesPadState()
        {
            if (SalesPadState == SalesPadTransState.Transaction) HideTransaction();
            if (SalesPadState == SalesPadTransState.Tender) HideTender();
            if (SalesPadState == SalesPadTransState.Change) HideChange();
            if (SalesPadState == SalesPadTransState.Receipt) HideReceipt();
        }

        private void StartSalesStep()
        {
            SalesLst.SelectedIndex = 0;
            HideTransaction();// SalesPad.EntryCol.Width = new GridLength(0);
            ShowTender();
            
        }

        private void GotoBlankTransactionEntry()
        {
            if (SalesLst.SelectedIndex == SalesLst.Items.Count - 1)
            {

                SalesLst.SelectedIndex = -1;
                SalesLst.UpdateLayout();
                MoveSalesPadDown();



            }
            else
            {
                SalesLst.SelectedIndex += 1;
            }
        }

        public void GotoPreviousSalesStep()
        {
           
            switch (salesvm.ApplicationMode)
            {
                case ApplicationMode.Ticket:
                    PreviousTicketSaleStep();
                    break;
                case ApplicationMode.Pharmacy:

                    PreviousPharmacySaleSteps();
                    break;
                case ApplicationMode.POS:
                    PreviousPOSSaleSteps();
                    break;
                default:
                    break;
            }

        }

        private void PreviousPharmacySaleSteps()
        {
            SalesLst.SelectedIndex = 0;
            if (SalesPadState == SalesPadTransState.Receipt)//SalesPad.TotalsCol.Width == new GridLength(0) && SalesPad.PaymentCol.Width == new GridLength(0)
            {
                HideReceipt();
                ShowTransaction();
                return;
            }
            if (SalesPadState == SalesPadTransState.Transaction)
            {
                salesvm.GoToPreviousTransaction();
            }
        }

        private void PreviousTicketSaleStep()
        {
            SalesLst.SelectedIndex = 0;
            if (SalesPadState == SalesPadTransState.Tender)//SalesPad.TotalsCol.Width == new GridLength(200) && SalesPad.EntryCol.Width == new GridLength(0)
            {
                HideTender();
                ShowTransaction();
                return;
            }

            if (SalesPadState == SalesPadTransState.Change)//SalesPad.TotalsCol.Width == new GridLength(0) && SalesPad.PaymentCol.Width == new GridLength(0)
            {
                HideChange();
                ShowTender();
                return;
            }
            if (SalesPadState == SalesPadTransState.Receipt)//SalesPad.TotalsCol.Width == new GridLength(0) && SalesPad.PaymentCol.Width == new GridLength(0)
            {
                HideReceipt();
                ShowChange();
                return;
            }
        }

        private void PreviousPOSSaleSteps()
        {
            SalesLst.SelectedIndex = 0;
            if (SalesPadState == SalesPadTransState.Tender)//SalesPad.TotalsCol.Width == new GridLength(200) && SalesPad.EntryCol.Width == new GridLength(0)
            {
                HideTender();
                ShowTransaction();
                return;
            }

            if (SalesPadState == SalesPadTransState.Change)//SalesPad.TotalsCol.Width == new GridLength(0) && SalesPad.PaymentCol.Width == new GridLength(0)
            {
                HideChange();
                ShowTender();
                return;
            }
            if (SalesPadState == SalesPadTransState.Receipt)//SalesPad.TotalsCol.Width == new GridLength(0) && SalesPad.PaymentCol.Width == new GridLength(0)
            {
                HideReceipt();
                ShowChange();
                return;
            }
        }

        public void HideReceipt()
        {
            SalesPad.ReceiptCol.Width = new GridLength(0);
            SalesPad.ReceiptGrd.Visibility = System.Windows.Visibility.Hidden;
            SalesPad.PrintCol.Width = new GridLength(0);
            SalesPad.PrintGrd.Visibility = System.Windows.Visibility.Hidden;
        }
        public void ShowReceipt()
        {
            SalesPad.ReceiptCol.Width = new GridLength(400);
            SalesPad.ReceiptGrd.Visibility = System.Windows.Visibility.Visible;
            SalesPad.PrintCol.Width = new GridLength(200);
            SalesPad.PrintGrd.Visibility = System.Windows.Visibility.Visible;
            SalesTaskPad.MoveToNextControl(SalesPad.PrintGrd);
            SalesPadState = SalesPadTransState.Receipt;
        }

        public void HideChange()
        {
            SalesPad.ChangeCol.Width = new GridLength(0);
            SalesPad.ChangeGrd.Visibility = System.Windows.Visibility.Hidden;
            //SalesPad.PrintCol.Width = new GridLength(0);
            //SalesPad.PrintGrd.Visibility = System.Windows.Visibility.Hidden;
        }
        public void ShowChange()
        {
            SalesPad.ChangeCol.Width = new GridLength(400);
            SalesPad.ChangeGrd.Visibility = System.Windows.Visibility.Visible;
            //SalesPad.PrintCol.Width = new GridLength(200);
            //SalesPad.PrintGrd.Visibility = System.Windows.Visibility.Visible;
            SalesTaskPad.MoveToNextControl(SalesPad.PrintGrd);
            SalesPadState = SalesPadTransState.Change;
        }

        public void ShowTender()
        {
            SalesPad.TotalsCol.Width = new GridLength(200);
            SalesPad.TotalsGrd.Visibility = System.Windows.Visibility.Visible;
            SalesPad.PaymentCol.Width = new GridLength(400);
            SalesPad.TenderOptionsGrd.Visibility = System.Windows.Visibility.Visible;
            SalesTaskPad.MoveToNextControl(SalesPad.TenderOptionsGrd);
            SalesPadState = SalesPadTransState.Tender;
        }

        public void ShowTransaction()
        {
            SalesPad.EntryCol.Width = new GridLength(408);
            SalesPad.TransactionGrd.Visibility = System.Windows.Visibility.Visible;
            SalesPad.TotalsCol.Width = new GridLength(200);
            SalesPad.TotalsGrd.Visibility = System.Windows.Visibility.Visible;
            SalesTaskPad.MoveToNextControl(SalesPad.TransactionGrd);
            SalesPadState = SalesPadTransState.Transaction;

        }

        public void GotoNextSalesStep(Key e)
        {
            SalesLst.SelectedIndex = 0;
            

            switch (salesvm.ApplicationMode)
            {
                case ApplicationMode.Ticket:
                    NextTicketSalesSteps(e);
                    break;
                case ApplicationMode.Pharmacy:
                    NextPhamacySalesSteps(e);

                    break;
                case ApplicationMode.POS:
                    NextPOSSaleSteps(e);                   
                    break;
                default:
                    break;
            }



        }

        private void NextTicketSalesSteps(Key e)
        {
            if (SalesPadState == SalesPadTransState.Change && e == Key.Right)
            {
                HideChange();
                ShowReceipt();
                return;
            }
            if ((SalesPadState == SalesPadTransState.Change) && e == Key.Enter)
            {
                HideChange();
                salesvm.CloseTransaction<Ticket>();
                salesvm.SaveTransaction();
                salesvm.CreateNewTransaction<Ticket>();
                ShowTransaction();
                return;
            }

            if ((SalesPadState == SalesPadTransState.Receipt) && e == Key.Enter)
            {
                HideReceipt();
                salesvm.CloseTransaction<Ticket>();
                salesvm.SaveTransaction();
                salesvm.CreateNewTransaction<Ticket>();
                ShowTransaction();
                return;
            }

            if (SalesPadState == SalesPadTransState.Transaction)
            {
                HideTransaction();
                ShowTender();

                return;
            }

            if (SalesPadState == SalesPadTransState.Tender)//SalesPad.EntryCol.Width == new GridLength(0) && SalesPad.TotalsCol.Width != new GridLength(0)
            {
                HideTender();
                ShowChange();

                return;
            }
        }

        private void NextPhamacySalesSteps(Key e)
        {
            if (SalesPadState == SalesPadTransState.Receipt && (e == Key.Right || e == Key.Enter))
            {
                HideReceipt();
                if (salesvm.TransactionData != null && salesvm.TransactionData.Status == null)
                {
                    salesvm.TransactionData.OpenClose = false;
                    salesvm.SaveTransaction();
                    salesvm.CreateNewTransaction<QuickPrescription>();
                    
                }
                 ShowTransaction();
                return;
            }


            if (SalesPadState == SalesPadTransState.Transaction)
            {
                HideTransaction();
                if (salesvm.TransactionData != null && salesvm.TransactionData.Status == null)
                {
                    salesvm.SaveTransaction();
                    ShowReceipt();
                }
                ShowReceipt();

                return;
            }

        }

        private void NextPOSSaleSteps(Key e)
        {
            if (SalesPadState == SalesPadTransState.Change && e == Key.Right)
            {
                HideChange();
                ShowReceipt();
                return;
            }
            if ((SalesPadState == SalesPadTransState.Change || SalesPadState == SalesPadTransState.Change) && e == Key.Enter)
            {
                HideChange();
                salesvm.TransactionData.OpenClose = false;
                salesvm.SaveTransaction();
                salesvm.CreateNewTransaction<TransactionBase>();
                ShowTransaction();
                return;
            }

            if (SalesPadState == SalesPadTransState.Transaction)
            {
                HideTransaction();
                ShowTender();

                return;
            }

            if (SalesPadState == SalesPadTransState.Tender)//SalesPad.EntryCol.Width == new GridLength(0) && SalesPad.TotalsCol.Width != new GridLength(0)
            {
                HideTender();
                ShowChange();

                return;
            }
        }

        public void HideTransaction()
        {
            SalesPad.EntryCol.Width = new GridLength(0);
            SalesPad.TransactionGrd.Visibility = System.Windows.Visibility.Hidden;
            SalesPad.TotalsCol.Width = new GridLength(0);
            SalesPad.TotalsGrd.Visibility = System.Windows.Visibility.Hidden;
            SalesRegion.SalesTaskPad.MoveToNextControl(SalesPad.TenderOptionsGrd);
        }

        public void HideTender()
        {
            SalesPad.TotalsCol.Width = new GridLength(0);
            SalesPad.TotalsGrd.Visibility = System.Windows.Visibility.Hidden;
            SalesPad.PaymentCol.Width = new GridLength(0);
            SalesPad.TenderOptionsGrd.Visibility = System.Windows.Visibility.Hidden;
        }

        public void GoToNextTransactionEntry()
        {
           

            if (SalesLst.SelectedIndex == SalesLst.Items.Count - 1)
            {
                SalesLst.SelectedIndex = -1;
            }
            else
            {
                SalesLst.SelectedIndex += 1; // selected index don't change when more than list
            }
        }

        private void GoToPreviousTransactionEntry(KeyEventArgs e)
        {
            e.Handled = true;
            if (SalesLst.SelectedIndex != -1)
            {
                SalesLst.SelectedIndex -= 1;
            }
            else
            {
                SalesLst.SelectedIndex += 1;
            }
        }

        private void UserControl_Unloaded_1(object sender, RoutedEventArgs e)
        {
            salesvm.SaveTransaction();
        }



        
    }
}