using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class MessageRepository : Repository, IRepository<Message>
    {
        public bool Delete(int id)
        {
            string sql = $"DELETE FROM Messages WHERE MessageId=@MessageId";
            this.AddParameters("MessageId", id.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(Message model)
        {
            string sql = $"DELETE FROM Messages WHERE MessageId=@MessageId";
            this.AddParameters("MessageId", model.MessageId.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Insert(Message model)
        {
            string sql = $"INSERT INTO Menus(MessageContent,MessageDate) VALUES('@MessageContent','@MessageDate')";
            this.AddParameters("MessageContent", model.MessageContent); //prevents SQL Injection
            this.AddParameters("MessageDate", model.MessageDate); //prevents SQL Injection
            return this.dbContext.Create(sql) > 0;
        }

        public Message Read(object id)
        {
            string sql = $"SELECT FROM Messages WHERE MessageId=@MessageId";
            this.AddParameters("MessageId", id.ToString()); //prevents SQL Injection
            return this.modelFactory.MessageModelCreator.CreateModel(this.dbContext.Read(sql));
            //returns Message
        }

        public List<Message> ReadAll()
        {
            List<Message> Messages = new List<Message>();
            string sql = "SELECT * FROM Messages";
            IDataReader dataReader = this.dbContext.Read(sql);
            while (dataReader.Read() == true)
                Messages.Add(this.modelFactory.MessageModelCreator.CreateModel(dataReader));
            return Messages;
        }

        public object ReadValue()
        {
            throw new NotImplementedException();
        }

        public bool Update(Message model)
        {
            string sql = "UPDATE Messages SET MessageContent=@MessageContent, MessageDate=@MessageDate where MessageId=@MessageId";
            this.AddParameters("MessageId", model.MessageId.ToString()); //prevents SQL Injection
            this.AddParameters("MessageContent", model.MessageContent); //prevents SQL Injection
            this.AddParameters("MessageDate", model.MessageDate); //prevents SQL Injection
            return this.dbContext.Update(sql) > 0;
        }
    }
}