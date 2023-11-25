using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class FoodModelCreator : IOleDbModelCreator<Food>
    {
        public Food CreateModel(IDataReader source)
        {
            Food food = new Food()
            {
                FoodId = Convert.ToInt16(source["UserId"]),
                FoodName = Convert.ToString(source["FoodName"]),
                FoodDesc = Convert.ToString(source["FoodDesc"]),
                FoodPrice = Convert.ToInt16(source["FoodPrice"]),
                FoodImage = Convert.ToString(source["FoodImage"]),
                Menus = null
            };
            return food;
        }
    }
}