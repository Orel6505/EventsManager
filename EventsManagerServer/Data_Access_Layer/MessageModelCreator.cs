using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class MessageModelCreator : IOleDbModelCreator<Message>
    {
        public Message CreateModel(IDataReader source)
        {
            Message message = new Message()
            {
                MessageId = Convert.ToInt16(source["MessageId"]),
                MessageContent = Convert.ToString(source["MessageContent"]),
                MessageDate = Convert.ToString(source["MessageDate"]),
            };
            return message;
        }
    }
}