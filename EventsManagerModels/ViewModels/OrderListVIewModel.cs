using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManagerModels
{
    public class OrderListVIewModel
    {
        public List<Order> Orders { get; set; }
        public List<Menu> Menus { get; set; }
        public List<FoodType> FoodTypes { get; set; }
        public List<Food> Foods { get; set; }
        public List<Hall> Halls { get; set; }

    }
}
