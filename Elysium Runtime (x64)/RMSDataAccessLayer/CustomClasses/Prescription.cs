using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
    public partial class Prescription: ISearchItem

    {
        public Prescription()
        {
            PropertyChanged +=Prescription_PropertyChanged;
            
        }

        private void Prescription_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DoctorId" || e.PropertyName == "PatientId" || e.PropertyName == "Repeat")
            {
                OnPropertyChanged("Status");
                OnPropertyChanged("Doctor");
                OnPropertyChanged("Patient");
            }
        }
       partial void OnDoctorIdChanging(Nullable<global::System.Int32> value)
        {
            if (value == 0)
            {
                AddError("DoctorId", "Please Select Doctor");
            }
            else
            {
                RemoveError("DoctorId");
            }
        }
       partial void OnPatientIdChanging(Nullable<global::System.Int32> value)
       {
           if (value == 0)
           {
               AddError("PatientId", "Please Select Patient");
           }
           else
           {
               RemoveError("PatientId");
           }
       }

       public new string Status
        {
            get
            {
                if (DoctorId == 0)
                    return "Please select Doctor";
                if (PatientId == 0)
                    return "Please select Patient";

                return base.Status;
            }
            set
            {
                base.Status = value;
                OnPropertyChanged("Status");
            }
        }
    }
}
