using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.DataObjects
{
    interface IDataObject
    {
        bool isPersistable { get; }
        void saveObject();

        void loadObject();
        
    }

 
}
