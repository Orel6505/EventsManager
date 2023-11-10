using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManagerModels
{
    public class Hall
    {
        public int HallId { get; set; }
        public string HallName { get; set; }
        public string HallDesc { get; set; }
        public int MaxPeople { get; set; }
        public string HallImage { get; set; }

        public List<Rating> Ratings { get; set; }
    }
}
