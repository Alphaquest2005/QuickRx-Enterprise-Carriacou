using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
    public partial class Person : ISearchItem
    {
        public Person()
        {
            PropertyChanged +=Person_PropertyChanged;
        }

        private void Person_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FirstName" || e.PropertyName == "LastName")
            {
                OnPropertyChanged("DisplayName");
                OnPropertyChanged("SearchCriteria");
            }

        }

        public string Name
        {
            get { return FirstName + " " + LastName; }
            
        }

        public string SearchCriteria
        {
            get { return FirstName + " " + LastName; }
            set
            {

            }
        }

        public string DisplayName
        {
            get { return FirstName + " " + LastName; }
        }

        public string Key
        {
            get { return Id.ToString(); }
        }
    }
}
