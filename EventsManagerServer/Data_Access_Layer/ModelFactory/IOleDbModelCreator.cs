using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManager.Data_Access_Layer
{
    public interface IOleDbModelCreator<T>:IModelCreator<T, IDataReader> //this interface implements IModelCreator and make it working with access database
    // also, this interface will be used in all of our ModelCreators
    {
    }
}
