using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class UserTypeRepository : Repository, IRepository<UserType>
    {
        public UserTypeRepository(DbContext dbContext, ILogger<UserTypeRepository> logger) : base(dbContext, logger) { }

        public bool Insert(UserType model)
        {
            const string sql = """
                INSERT INTO UserTypes(UserTypeId,UserTypeName)
                VALUES(@UserTypeId,@UserTypeName)
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@UserTypeId", model.UserTypeId);
                AddParameter(cmd, "@UserTypeName", model.UserTypeName);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "INSERT UserType failed {@UserType}", model);
                throw;
            }
        }

        public Task<bool> InsertAsync(UserType model) => InsertAsyncDefault(model, Insert);

        public UserType? Read(object id)
        {
            const string sql = "SELECT * FROM UserTypes WHERE UserTypeId=@UserTypeId";

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@UserTypeId", id);

                using IDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    logger.LogWarning("UserType with ID {Id} not found", id);
                    return null;
                }

                return modelFactory.UserTypeModelCreator.CreateModel(reader);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "READ UserType failed for UserTypeId={Id}", id);
                throw;
            }
        }

        public Task<UserType> ReadAsync(object id) => ReadAsyncDefault(id, Read);

        public List<UserType> ReadAll()
        {
            const string sql = "SELECT * FROM UserTypes";
            List<UserType> userTypes = new();

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            using IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                userTypes.Add(modelFactory.UserTypeModelCreator.CreateModel(reader));
            }

            return userTypes;
        }

        public Task<List<UserType>> ReadAllAsync() => ReadAllAsyncDefault(ReadAll);

        public bool Update(UserType model)
        {
            const string sql = """
                UPDATE UserTypes
                SET UserTypeName=@UserTypeName
                WHERE UserTypeId=@UserTypeId
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@UserTypeId", model.UserTypeId);
                AddParameter(cmd, "@UserTypeName", model.UserTypeName);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "UPDATE UserType failed {@UserType}", model);
                throw;
            }
        }

        public Task<bool> UpdateAsync(UserType model) => UpdateAsyncDefault(model, Update);

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM UserTypes WHERE UserTypeId=@UserTypeId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@UserTypeId", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(int id) => DeleteAsyncDefault(id, Delete);

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(UserType model)
        {
            const string sql = "DELETE FROM UserTypes WHERE UserTypeId=@UserTypeId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@UserTypeId", model.UserTypeId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(UserType model) => DeleteAsyncDefault(model, Delete);

        public object ReadValue()
        {
            throw new NotImplementedException();
        }
    }
}