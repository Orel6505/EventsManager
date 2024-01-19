using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class EventTypeModelCreator : IOleDbModelCreator<EventType>
    {
        public EventType CreateModel(IDataReader source)
        {
            EventType eventType = new EventType()
            {
                EventTypeId = Convert.ToInt16(source["EventTypeId"]),
                EventTypeName = Convert.ToString(source["EventTypeName"]),
            };
            return eventType;
        }
    }
}