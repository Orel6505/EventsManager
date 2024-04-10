using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManagerModels
{
    public class Food
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public string FoodDesc { get; set; }
        public double FoodPrice { get; set; }
        public string FoodImage { get; set; }
        public int FoodTypeId { get; set; }

        public List<Menu> Menus { get; set; }
    }
}
