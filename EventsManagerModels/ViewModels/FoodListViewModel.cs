using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManagerModels
{
    public class FoodListViewModel
    {
        public List<Food> Foods { get; set; }
        public List<Hall> Halls { get; set; }
        public int PageNumber { get; set; }
        public int ItemsPerpage { get; set; }
    }
}
