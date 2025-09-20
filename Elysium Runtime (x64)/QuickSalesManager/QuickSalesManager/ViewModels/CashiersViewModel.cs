using System;
using System.ComponentModel;
using System.Data.Entity;
using RMSDataAccessLayer;
using System.Collections.ObjectModel;
using System.Windows.Data;

using System.Windows;
using System.Windows.Input;
using Common;
using PrismMVVMLibrary;


namespace QuickSalesManager
{
    public class CashiersViewModel : ViewModelBase
	{
        RMSModel db;
        public CashiersViewModel()
        {
            // Insert code required on object creation below this point.
            if (IsInDesignMode == false)
                db = new RMSModel();
            _cashierlist.Source = new ObservableCollection<Cashier>(db.Persons.OfType<Cashier>());

        }

      

        CollectionViewSource _cashierlist = new CollectionViewSource();

        public Cashier CashierLst
        {
            get
            {
                return  (Cashier)_cashierlist.View.CurrentItem;
            }
        }

        RelayCommand _MoveNextCashierCmd;
        public ICommand MoveNextCashierCmd
        {
            get
            {
                if (_MoveNextCashierCmd == null)
                {
                    _MoveNextCashierCmd = new RelayCommand(MoveNextCashier, canMoveNextCashier);
                }
                return _MoveNextCashierCmd;
            }

        }
       // public DelegateCommand MoveNextCashierCmd { get; set; }
        RelayCommand _MovePreviousCashierCmd;
        public ICommand MovePreviousCashierCmd
        {
            get
            {
                if (_MovePreviousCashierCmd == null)
                {
                    _MovePreviousCashierCmd = new RelayCommand(MovePreviousCashier, canMovePreviousCashier);
                }
                return _MovePreviousCashierCmd;
            }

        }

        RelayCommand _AddCashierCmd;
        public ICommand AddCashierCmd
        {
            get
            {
                if (_AddCashierCmd == null)
                {
                    _AddCashierCmd = new RelayCommand(AddCashier, canAddCashier);
                }
                return _AddCashierCmd;
            }

        }

        private bool canAddCashier(object obj)
        {
            return true;
        }

        private void AddCashier(object obj)
        {
            Cashier ca = db.Persons.CreateObject<Cashier>();
            ca.FirstName = "new cashier";
            db.Persons.AddObject(ca);
            ((ObservableCollection<Cashier>)(_cashierlist.Source)).Add(ca);
            _cashierlist.View.MoveCurrentToLast();
            NotifyPropertyChanged("CashierLst");
        }

        private void UpdateSave()
        {
            db.SaveChanges();
            NotifyPropertyChanged("CashierLst");
        }
            


        public void MoveNextCashier(object obj)
        {
            _cashierlist.View.MoveCurrentToNext();
            UpdateSave();


        }

        private bool canMoveNextCashier(object obj)
        {
            return (!_cashierlist.View.IsCurrentAfterLast || !_cashierlist.View.IsEmpty);
           
        }

        public void MovePreviousCashier(object obj)
        {
            _cashierlist.View.MoveCurrentToPrevious();
            UpdateSave();
        }
        private bool canMovePreviousCashier(object obj)
        {
            return !_cashierlist.View.IsCurrentBeforeFirst;
            
        }
		
		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
		#endregion

	}
}