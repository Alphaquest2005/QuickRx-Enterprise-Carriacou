using System;
using System.ComponentModel;
using System.Data.Entity;
using RMSDataAccessLayer;
using System.Collections.ObjectModel;
using System.Windows.Data;

using System.Windows;
using System.Windows.Input;
using Common;


namespace QuickSalesManager
{
	public class PassViewModel : INotifyPropertyChanged
	{
        RMSModel db = new RMSModel();
        public PassViewModel()
		{
			// Insert code required on object creation below this point.
            _Passlist.Source = new ObservableCollection<Pass>(db.Persons.OfType<Pass>());
           
		}

      

        CollectionViewSource _Passlist = new CollectionViewSource();

        public CollectionViewSource PassLst
        {
            get
            {
                return  _Passlist;
            }
        }

        RelayCommand _MoveNextPassCmd;
        public ICommand MoveNextPassCmd
        {
            get
            {
                if (_MoveNextPassCmd == null)
                {
                    _MoveNextPassCmd = new RelayCommand(MoveNextPass, canMoveNextPass);
                }
                return _MoveNextPassCmd;
            }

        }
       // public DelegateCommand MoveNextPassCmd { get; set; }
        RelayCommand _MovePreviousPassCmd;
        public ICommand MovePreviousPassCmd
        {
            get
            {
                if (_MovePreviousPassCmd == null)
                {
                    _MovePreviousPassCmd = new RelayCommand(MovePreviousPass, canMovePreviousPass);
                }
                return _MovePreviousPassCmd;
            }

        }

        RelayCommand _AddPassCmd;
        public ICommand AddPassCmd
        {
            get
            {
                if (_AddPassCmd == null)
                {
                    _AddPassCmd = new RelayCommand(AddPass, canAddPass);
                }
                return _AddPassCmd;
            }

        }

        private bool canAddPass(object obj)
        {
            return true;
        }

        private void AddPass(object obj)
        {
            Pass pass = db.Pass.CreateObject<Pass>();
            pass.StartDate = DateTime.Now;
            db.Pass.AddObject(pass);
            ((ObservableCollection<Pass>)(_Passlist.Source)).Add(pass);
            _Passlist.View.MoveCurrentToLast();
            NotifyPropertyChanged("Pass");
        }

        private void UpdateSave()
        {
            db.SaveChanges();
            NotifyPropertyChanged("Pass");
        }
            


        public void MoveNextPass(object obj)
        {
            _Passlist.View.MoveCurrentToNext();
            UpdateSave();


        }

        private bool canMoveNextPass(object obj)
        {
            return (!_Passlist.View.IsCurrentAfterLast || !_Passlist.View.IsEmpty);
           
        }

        public void MovePreviousPass(object obj)
        {
            _Passlist.View.MoveCurrentToPrevious();
            UpdateSave();
        }
        private bool canMovePreviousPass(object obj)
        {
            return !_Passlist.View.IsCurrentBeforeFirst;
            
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