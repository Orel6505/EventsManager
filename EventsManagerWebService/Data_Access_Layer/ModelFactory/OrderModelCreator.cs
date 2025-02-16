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
            Order order = new Order()
            {
                OrderId = Convert.ToInt16(source["OrderId"]),
                OrderDate = Convert.ToString(source["OrderDate"]),
                MenuId = Convert.ToInt16(source["MenuId"]),
                HallId = Convert.ToInt16(source["HallId"]),
                UserId = Convert.ToInt16(source["UserId"]),
                IsPaid = Convert.ToString(source["IsPaid"]),
                NumOfPeople = Convert.ToInt16(source["NumOfPeople"]),
                EventDate = Convert.ToString(source["EventDate"]),
                EventTypeId = Convert.ToInt16(source["EventTypeId"])
            };
            return order;
        }
    }
}
