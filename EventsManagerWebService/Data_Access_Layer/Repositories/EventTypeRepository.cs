using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class EventTypeRepository : Repository, IRepository<EventType>
    {
        public EventTypeRepository(DbContext dbContext, ILogger<EventTypeRepository> logger) : base(dbContext, logger) { }

        public bool Insert(EventType model)
        {
            const string sql = """
                INSERT INTO EventTypes(EventTypeId,EventTypeName)
                VALUES(@EventTypeId,@EventTypeName)
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@EventTypeId", model.EventTypeId);
                AddParameter(cmd, "@EventTypeName", model.EventTypeName);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "INSERT EventType failed {@EventType}", model);
                throw;
            }
        }

        public Task<bool> InsertAsync(EventType model) => InsertAsyncDefault(model, Insert);

        public EventType? Read(object id)
        {
            const string sql = "SELECT * FROM EventTypes WHERE EventTypeId=@EventTypeId";

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@EventTypeId", id);

                using IDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    logger.LogWarning("EventType with ID {Id} not found", id);
                    return null;
                }

                return modelFactory.EventTypeModelCreator.CreateModel(reader);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "READ EventType failed for EventTypeId={Id}", id);
                throw;
            }
        }

        public Task<EventType> ReadAsync(object id) => ReadAsyncDefault(id, Read);

        public List<EventType> ReadAll()
        {
            const string sql = "SELECT * FROM EventTypes";
            List<EventType> eventTypes = new();

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            using IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                eventTypes.Add(modelFactory.EventTypeModelCreator.CreateModel(reader));
            }

            return eventTypes;
        }

        public Task<List<EventType>> ReadAllAsync() => ReadAllAsyncDefault(ReadAll);

        public List<EventType> HallAvailability(string EventDate, string HallId)
        {
            const string sql = """
                SELECT EventTypes.*
                FROM EventTypes
                WHERE EventTypes.EventTypeId NOT IN (
                    SELECT EventTypeId
                    FROM Orders
                    WHERE EventDate = @EventDate AND IsPaid = True AND HallId = @HallId
                )
                """;

            List<EventType> eventTypes = new();

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@EventDate", EventDate);
                AddParameter(cmd, "@HallId", HallId);

                using IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    eventTypes.Add(modelFactory.EventTypeModelCreator.CreateModel(reader));
                }

                return eventTypes;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "HallAvailability failed for EventDate={EventDate}, HallId={HallId}", EventDate, HallId);
                throw;
            }
        }

        public bool Update(EventType model)
        {
            const string sql = """
                UPDATE EventTypes
                SET EventTypeName=@EventTypeName
                WHERE EventTypeId=@EventTypeId
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@EventTypeId", model.EventTypeId);
                AddParameter(cmd, "@EventTypeName", model.EventTypeName);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "UPDATE EventType failed {@EventType}", model);
                throw;
            }
        }

        public Task<bool> UpdateAsync(EventType model) => UpdateAsyncDefault(model, Update);

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM EventTypes WHERE EventTypeId=@EventTypeId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@EventTypeId", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(int id) => DeleteAsyncDefault(id, Delete);

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(EventType model)
        {
            const string sql = "DELETE FROM EventTypes WHERE EventTypeId=@EventTypeId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@EventTypeId", model.EventTypeId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(EventType model) => DeleteAsyncDefault(model, Delete);

        public object ReadValue()
        {
            throw new NotImplementedException();
        }
    }
}