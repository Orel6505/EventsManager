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
            try
            {
                Food food = new Food()
                {
                    FoodId = Convert.ToInt16(source["FoodId"]),
                    FoodName = Convert.ToString(source["FoodName"]),
                    FoodDesc = Convert.ToString(source["FoodDesc"]),
                    FoodPrice = Convert.ToDouble(source["FoodPrice"]),
                    FoodImage = Convert.ToString(source["FoodImage"]),
                    FoodTypeId = Convert.ToInt16(source["FoodTypeId"]),
                    Menus = null
                };
                return food;
            }
            catch (Exception)
            {
                Food food = new Food()
                {
                    FoodId = Convert.ToInt16(source["FoodMenu.FoodId"]),
                    FoodName = Convert.ToString(source["FoodName"]),
                    FoodDesc = Convert.ToString(source["FoodDesc"]),
                    FoodPrice = Convert.ToDouble(source["FoodPrice"]),
                    FoodImage = Convert.ToString(source["FoodImage"]),
                    Menus = null
                };
                return food;
            }
        }
    }
}