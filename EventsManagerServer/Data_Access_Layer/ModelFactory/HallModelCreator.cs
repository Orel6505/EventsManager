using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace EventsManager.Data_Access_Layer
{
    public class HallModelCreator : IOleDbModelCreator<Hall>
    {
        public Hall CreateModel(IDataReader source)
        {
            Hall hall = new Hall()
            {
                HallId = Convert.ToInt16(source["HallId"]),
                HallName = Convert.ToString(source["HallName"]),
                HallDesc = Convert.ToString(source["HallDesc"]),
                MaxPeople = Convert.ToInt16(source["MaxPeople"]),
                HallImage = Convert.ToString(source["HallImage"]),
                Ratings = null
            };
            return hall;
        }
    }
}