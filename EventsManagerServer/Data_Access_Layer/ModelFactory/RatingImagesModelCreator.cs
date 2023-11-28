
﻿using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class RatingImagesModelCreator : IOleDbModelCreator<RatingImage>
    {
        public RatingImage CreateModel(IDataReader source)
        {
            RatingImage RatingImages = new RatingImage()
            {
                ImageId = Convert.ToInt16(source["RatingTitle"]),
                ImageLocation = Convert.ToString(source["RatingDesc"]),
                RatingId = Convert.ToInt16(source["RatingId"])

            };
            return RatingImages;
        }
    }
}