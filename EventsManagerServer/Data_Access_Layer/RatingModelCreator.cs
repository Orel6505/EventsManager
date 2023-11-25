using EventsManagerModels;
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
            throw new NotImplementedException();
        }
    }
}