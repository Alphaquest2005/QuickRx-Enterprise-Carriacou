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
	public class CustomersViewModel : INotifyPropertyChanged
	{
       

       static RMSModel db = new RMSDataAccessLayer.RMSModel();
        public CustomersViewModel()
		{
			// Insert code required on object creation below this point.

            
                
           // _Customerslist.Source = new ObservableCollection<Customers>(db.Persons.OfType<Customers>());
            _Customerslist.CurrentChanged += View_CurrentChanged;
      Customer.PropertyChanged +=Customer_PropertyChanged;
           
		}

        private void Customer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Customer");
        }



       

        void View_CurrentChanged(object sender, EventArgs e)
        {
           // _Customerslist.View.MoveCurrentTo((sender as ListCollectionView).CurrentItem);
           
            //_Customerslist.View.MoveCurrentToPosition((sender as ListCollectionView).CurrentPosition);
         //   Customer = (Customers) (sender as ListCollectionView).CurrentItem;
            NotifyPropertyChanged("Customer");
            NotifyPropertyChanged("CustomerList");
           
        }



        private static ListCollectionView _Customerslist = new ListCollectionView(new ObservableCollection<Customers>(db.Persons.OfType<Customers>()));

        public ListCollectionView CustomerList
        {
            get
            {
                return _Customerslist;
            }
        }

        public  Customers Customer
        {
            get
            {
               return (Customers) _Customerslist.CurrentItem;
            }
            set
            {
                _Customerslist.MoveCurrentTo(value);

                NotifyPropertyChanged("Customer");
                NotifyPropertyChanged("CustomerList");

               
            }
        }

        RelayCommand _MoveNextCustomersCmd;
        public ICommand MoveNextCustomersCmd
        {
            get
            {
                if (_MoveNextCustomersCmd == null)
                {
                    _MoveNextCustomersCmd = new RelayCommand(MoveNextCustomers, canMoveNextCustomers);
                }
                return _MoveNextCustomersCmd;
            }

        }
       // public DelegateCommand MoveNextCustomersCmd { get; set; }
        RelayCommand _MovePreviousCustomersCmd;
        public ICommand MovePreviousCustomersCmd
        {
            get
            {
                if (_MovePreviousCustomersCmd == null)
                {
                    _MovePreviousCustomersCmd = new RelayCommand(MovePreviousCustomers, canMovePreviousCustomers);
                }
                return _MovePreviousCustomersCmd;
            }

        }

        RelayCommand _Save;
        public ICommand SaveCmd
        {
            get
            {
                if (_Save == null)
                {
                    _Save = new RelayCommand(Save);
                }
                return _Save;
            }

        }

        private void Save(object obj)
        {
            UpdateSave();
        }

        RelayCommand _AddCustomersCmd;
        public ICommand AddCustomersCmd
        {
            get
            {
                if (_AddCustomersCmd == null)
                {
                    _AddCustomersCmd = new RelayCommand(AddCustomers, canAddCustomers);
                }
                return _AddCustomersCmd;
            }

        }

        private bool canAddCustomers(object obj)
        {
            return true;
        }

        private void AddCustomers(object obj)
        {
            Customers Customers = db.Persons.CreateObject<Customers>();
            Customers.FirstName = "New Customer";
            db.Persons.AddObject(Customers);
            _Customerslist.AddNewItem(Customers);
            _Customerslist.MoveCurrentToLast();
            NotifyPropertyChanged("Customer");
        }

        public void UpdateSave()
        {
            db.SaveChanges();
            NotifyPropertyChanged("CustomerList");
            NotifyPropertyChanged("Customer");
        }
            


        public void MoveNextCustomers(object obj)
        {
            _Customerslist.MoveCurrentToNext();
            UpdateSave();


        }

        private bool canMoveNextCustomers(object obj)
        {
            return (!_Customerslist.IsCurrentAfterLast || !_Customerslist.IsEmpty);
           
        }

        public void MovePreviousCustomers(object obj)
        {
            _Customerslist.MoveCurrentToPrevious();
            UpdateSave();
        }
        private bool canMovePreviousCustomers(object obj)
        {
            return !_Customerslist.IsCurrentBeforeFirst;
            
        }
		
		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String info)
		{
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
          // RaisePropertyChanged(info);
		}
		#endregion

	}
}