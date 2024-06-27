using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManagerModels
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderDate { get; set; } //timestamp
        public int MenuId { get; set; }
        public int UserId { get; set; }
        public int HallId { get; set; }
        public string IsPaid {  get; set; }
        public int NumOfPeople { get; set; }
        public string EventDate { get; set; }
        public int EventTypeId { get; set; }
        public EventType EventType { get; set; }
        public Menu ChosenMenu { get; set; }
        public Hall ChosenHall { get; set; }
        public User ChosenUser { get; set; }



    }
}
