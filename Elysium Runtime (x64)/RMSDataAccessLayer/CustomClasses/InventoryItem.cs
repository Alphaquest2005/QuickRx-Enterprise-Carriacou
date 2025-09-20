using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
    public partial class Item : ISearchItem
    {
        public string SearchCriteria
        {
            get {
                return String.Format("{0}|{1}", Description, ItemLookupCode);
            }
            set
            {
            }
        }

        public string DisplayName
        {
            get { return Description; }
        }

        public string Key
        {
            get { return ItemId.ToString(); }
        }
    }
}
