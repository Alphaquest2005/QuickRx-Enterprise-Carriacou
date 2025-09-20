using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
    public interface ISearchItem
    {
        string SearchCriteria
        {
            get;
            set;

        }
        string DisplayName
        {
            get;
           
        }
        string Key
        {
            get;
           
        }
    }
}
