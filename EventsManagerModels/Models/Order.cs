using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManagerModels
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderDate { get; set; } //timestamp
        public int MenuId { get; set; }
        public int HallId { get; set; }
        public int UserId { get; set; }
        public int NumOfPeople { get; set; }
    }
}
