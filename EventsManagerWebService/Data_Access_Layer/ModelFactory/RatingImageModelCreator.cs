using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using EventsManagerModels;

namespace EventsManager.Data_Access_Layer
{
    public class RatingImageModelCreator : IOleDbModelCreator<RatingImage>
    {
        public RatingImage CreateModel(IDataReader source)
        {
            RatingImage ratingImage = new RatingImage()
            {
                ImageId = Convert.ToInt16(source["ImageId"]),
                ImageLocation = Convert.ToString(source["ImageLocation"]),
                RatingId = Convert.ToInt16(source["RatingId"])

            };
            return ratingImage;
        }
    }
}