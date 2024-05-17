using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


namespace EventsManager.Data_Access_Layer
{
    public class EventTypeRepository : Repository, IRepository<EventType>
    {
        public EventTypeRepository(DbContext dbContext) : base(dbContext) { }

        public bool Delete(int id)
        {
            string sql = $"DELETE FROM EventTypes WHERE EventTypeId=@EventTypeId";
            this.AddParameters("EventTypeId", id.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(EventType model)
        {
            string sql = $"DELETE FROM EventTypes WHERE EventTypeId=@EventTypeId";
            this.AddParameters("EventTypeId", model.EventTypeId.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Insert(EventType model)
        {
            string sql = $"INSERT INTO EventTypes(EventTypeId,EventTypeName) VALUES(@EventTypeId,@EventTypeName)";
            this.AddParameters("EventTypeId", model.EventTypeId.ToString()); //prevents SQL Injection
            this.AddParameters("EventTypeName", model.EventTypeName); //prevents SQL Injection
            return this.dbContext.Create(sql) > 0;
        }

        public EventType Read(object id)
        {
            string sql = $"SELECT * FROM EventTypes WHERE EventTypeId=@EventTypeId";
            this.AddParameters("EventTypeId", id.ToString()); //prevents SQL Injection
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.EventTypeModelCreator.CreateModel(dataReader);
            }
            //returns Menu
        }

        public List<EventType> ReadAll()
        {
            List<EventType> EventTypes = new List<EventType>();
            string sql = "SELECT * FROM EventTypes";
            using (IDataReader dataReader = this.dbContext.Read(sql))
                while (dataReader.Read() == true)
                    EventTypes.Add(this.modelFactory.EventTypeModelCreator.CreateModel(dataReader));
            return EventTypes;
        }

        public object ReadValue()
        {
            throw new NotImplementedException();
        }

        public bool Update(EventType model)
        {
            string sql = "UPDATE EventTypes SET EventTypeName=@EventTypeName where EventTypeId=@EventTypeId";
            this.AddParameters("EventTypeId", model.EventTypeId.ToString()); //prevents SQL Injection
            this.AddParameters("EventTypeName", model.EventTypeName); //prevents SQL Injection
            return this.dbContext.Update(sql) > 0;
        }
    }
}