using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
   public class SearchItem : ISearchItem 
    {
        public object SearchObject { get; set; }
        public virtual string SearchCriteria
        {
            get;
            set;
        }

        string _DisplayName;
        public virtual string DisplayName
        {
            get
            {
                return _DisplayName;
            }
            set
            {
                _DisplayName = value;
            }
        }
        string _Key;
        public virtual string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value;
            }
        }

    }
}
