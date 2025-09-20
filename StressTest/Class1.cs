using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS2QBPost
{

    class Class1
    {
        private volatile Type _dependency;

        public Class1()
        {
            _dependency = typeof (System.Data.Entity.SqlServer.SqlProviderServices);
        }
    }
}
