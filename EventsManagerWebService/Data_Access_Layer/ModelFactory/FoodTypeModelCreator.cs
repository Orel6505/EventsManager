using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class FoodTypeModelCreator : IOleDbModelCreator<FoodType>
    {
        public FoodType CreateModel(IDataReader source)
        {
            FoodType FoodType = new FoodType()
            {
                FoodTypeId = Convert.ToInt16(source["FoodTypeId"]),
                FoodTypeName = Convert.ToString(source["FoodTypeName"]),
            };
            return FoodType;
        }
    }
}