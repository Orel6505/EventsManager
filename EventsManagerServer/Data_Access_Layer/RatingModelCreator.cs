﻿using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class RatingModelCreator : IOleDbModelCreator<Rating>
    {
        public Rating CreateModel(IDataReader source)
        {
            Rating rating = new Rating()
            {
                RatingId = Convert.ToInt16(source["RatingId"]),
                RatingTitle = Convert.ToString(source["RatingTitle"]),
                RatingDesc = Convert.ToString(source["RatingDesc"]),
                RatingStars = Convert.ToInt16(source["RatingStars"]),
                UserId = Convert.ToInt16(source["UserId"]),
                RatingImages = Convert.ToInt16(source["RatingImages"])
            };
            return rating;
        }
    }
}