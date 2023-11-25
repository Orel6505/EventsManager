﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManagerModels
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuDesc { get; set; }
        public int MenuPrice { get; set; }
        public string MenuImage { get; set; }

        public List<Food> Foods { get; set; }
    }
}
