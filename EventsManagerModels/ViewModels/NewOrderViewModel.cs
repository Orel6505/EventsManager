using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EventsManagerModels
{
    [Serializable]
    public class NewOrderViewModel
    {
        public Hall OrderHall { get; set; }
        [JsonProperty] 
        public List<Menu> Menus { get; set; }
    }
}
