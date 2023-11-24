using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManager.Data_Access_Layer
{
    public class OrderModelCreator : IOleDbModelCreator<Order>
    {
        public Order CreateModel(IDataReader source)
        {
            throw new NotImplementedException();
        }
    }
}
