using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class MenuModelCreator : IOleDbModelCreator<Menu>
    {
        public Menu CreateModel(IDataReader source)
        {
            Menu menu = new Menu()
            {
                MenuId = Convert.ToInt16(source["MenuId"]),
                MenuName = Convert.ToString(source["MenuName"]),
                MenuDesc = Convert.ToString(source["MenuDesc"]),
                MenuPrice = Convert.ToInt16(source["MenuPrice"]),
                MenuImage = Convert.ToString(source["MenuImage"]),
                Foods = null
            };
            return menu;
        }
    }
}