using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManagerModels
{
    public class Rating
    {
        public int RatingId { get; set; }
        public string RatingTitle { get; set; }
        public string RatingDesc { get; set; }
        public int RatingStars { get; set; }
        public int RatingDate { get; set; }
        public int UserId { get; set; }
        public int HallId { get; set; }
        public List<RatingImage> RatingImages { get; set; }
    }
}
