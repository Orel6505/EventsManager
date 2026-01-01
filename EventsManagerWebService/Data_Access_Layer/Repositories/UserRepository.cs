using EventsManagerModels;
using PasswordManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class UserRepository : Repository, IRepository<User>
    {
        public UserRepository(DbContext dbContext, ILogger<UserRepository> logger) : base(dbContext, logger) { }

        public bool Insert(User model)
        {
            const string sql = """
                INSERT INTO Users (FirstName, LastName, UserName, PassWordHash, Email, Address, PhoneNum, CreationDate, Salt)
                VALUES (@FirstName, @LastName, @UserName, @PassWordHash, @Email, @Address, @PhoneNum, @CreationDate, @Salt)
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@FirstName", model.FirstName);
                AddParameter(cmd, "@LastName", model.LastName);
                AddParameter(cmd, "@UserName", model.UserName);
                AddParameter(cmd, "@PassWordHash", model.UserPassword.HashPassword);
                AddParameter(cmd, "@Email", model.Email);
                AddParameter(cmd, "@Address", model.Address);
                AddParameter(cmd, "@PhoneNum", model.PhoneNum);
                AddParameter(cmd, "@CreationDate", model.CreationDate);
                AddParameter(cmd, "@Salt", model.UserPassword.Salt);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "INSERT User failed {@User}", model);
                throw;
            }
        }

        public Task<bool> InsertAsync(User model) => InsertAsyncDefault(model, Insert);

        public User? Read(object id)
        {
            const string sql = "SELECT * FROM Users WHERE UserId=@UserId";

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@UserId", id);

                using IDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    logger.LogWarning("User with ID {Id} not found", id);
                    return null;
                }

                return modelFactory.UserModelCreator.CreateModel(reader);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "READ User failed for UserId={Id}", id);
                throw;
            }
        }

        public Task<User> ReadAsync(object id) => ReadAsyncDefault(id, Read);

        public User? GetLoginByUserName(string UserName)
        {
            const string sql = """
                SELECT Users.UserName, Users.PasswordHash, Users.Salt
                FROM Users
                WHERE Users.UserName=@UserName
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@UserName", UserName);

                using IDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    logger.LogWarning("User with UserName {UserName} not found", UserName);
                    return null;
                }

                return modelFactory.LoginUserModelCreator.CreateModel(reader);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetLoginByUserName failed for UserName={UserName}", UserName);
                throw;
            }
        }

        public Password? GetPasswordByUserId(string UserId)
        {
            const string sql = """
                SELECT Users.PasswordHash, Users.Salt
                FROM Users
                WHERE UserId=@UserId
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@UserId", UserId);

                using IDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    logger.LogWarning("User with UserId {UserId} not found", UserId);
                    return null;
                }

                return new Password(Convert.ToString(reader["PasswordHash"]), Convert.ToString(reader["Salt"]));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetPasswordByUserId failed for UserId={UserId}", UserId);
                throw;
            }
        }

        public UserType? UserTypeByUserId(string UserId)
        {
            const string sql = """
                SELECT UserTypes.UserTypeId, UserTypes.UserTypeName
                FROM UserTypes
                INNER JOIN Users ON UserTypes.UserTypeId = Users.UserTypeId
                WHERE Users.UserId=@UserId
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@UserId", UserId);

                using IDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    logger.LogWarning("UserType for UserId {UserId} not found", UserId);
                    return null;
                }

                return modelFactory.UserTypeModelCreator.CreateModel(reader);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "UserTypeByUserId failed for UserId={UserId}", UserId);
                throw;
            }
        }

        public User? GetUser2FAByUserName(string UserName)
        {
            const string sql = """
                SELECT Users.UserName, Users.UserId, Users.UserTypeId, Users.Email
                FROM Users
                WHERE Users.UserName=@UserName
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@UserName", UserName);

                using IDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    logger.LogWarning("User with UserName {UserName} not found", UserName);
                    return null;
                }

                return modelFactory.User2FAModelCreator.CreateModel(reader);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetUser2FAByUserName failed for UserName={UserName}", UserName);
                throw;
            }
        }

        public List<User> ReadAll()
        {
            const string sql = "SELECT * FROM Users ORDER BY Users.UserId ASC";
            List<User> users = new();

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            using IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(modelFactory.UserModelCreator.CreateModel(reader));
            }

            return users;
        }

        public Task<List<User>> ReadAllAsync() => ReadAllAsyncDefault(ReadAll);

        public bool IsAvailableUserName(string UserName)
        {
            const string sql = "SELECT * FROM Users WHERE UserName=@UserName";

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@UserName", UserName);

                using IDataReader reader = cmd.ExecuteReader();

                return reader.Read();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "IsAvailableUserName failed for UserName={UserName}", UserName);
                throw;
            }
        }

        public bool Update(User model)
        {
            const string sql = """
                UPDATE Users
                SET FirstName=@FirstName,
                    LastName=@LastName,
                    UserName=@UserName,
                    PassWordHash=@PassWordHash,
                    Email=@Email,
                    Address=@Address,
                    PhoneNum=@PhoneNum,
                    CreationDate=@CreationDate,
                    Salt=@Salt
                WHERE UserId=@UserId
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@UserId", model.UserId);
                AddParameter(cmd, "@FirstName", model.FirstName);
                AddParameter(cmd, "@LastName", model.LastName);
                AddParameter(cmd, "@UserName", model.UserName);
                AddParameter(cmd, "@PassWordHash", model.UserPassword.HashPassword);
                AddParameter(cmd, "@Salt", model.UserPassword.Salt);
                AddParameter(cmd, "@Email", model.Email);
                AddParameter(cmd, "@Address", model.Address);
                AddParameter(cmd, "@PhoneNum", model.PhoneNum);
                AddParameter(cmd, "@CreationDate", model.CreationDate);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "UPDATE User failed {@User}", model);
                throw;
            }
        }

        public Task<bool> UpdateAsync(User model) => UpdateAsyncDefault(model, Update);

        public bool UpdateInfo(User model)
        {
            const string sql = """
                UPDATE Users
                SET FirstName=@FirstName,
                    LastName=@LastName,
                    UserName=@UserName,
                    Email=@Email,
                    Address=@Address,
                    PhoneNum=@PhoneNum
                WHERE UserId=@UserId
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@FirstName", model.FirstName);
                AddParameter(cmd, "@LastName", model.LastName);
                AddParameter(cmd, "@UserName", model.UserName);
                AddParameter(cmd, "@Email", model.Email);
                AddParameter(cmd, "@Address", model.Address);
                AddParameter(cmd, "@PhoneNum", model.PhoneNum);
                AddParameter(cmd, "@UserId", model.UserId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "UpdateInfo User failed {@User}", model);
                throw;
            }
        }

        public bool UpdatePassword(User model)
        {
            const string sql = """
                UPDATE Users
                SET PassWordHash=@PassWordHash,
                    Salt=@Salt
                WHERE UserId=@UserId
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@PassWordHash", model.UserPassword.HashPassword);
                AddParameter(cmd, "@Salt", model.UserPassword.Salt);
                AddParameter(cmd, "@UserId", model.UserId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "UpdatePassword User failed for UserId={UserId}", model.UserId);
                throw;
            }
        }

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Users WHERE UserId=@UserId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@UserId", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(int id) => DeleteAsyncDefault(id, Delete);

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(User model)
        {
            const string sql = "DELETE FROM Users WHERE UserId=@UserId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@UserId", model.UserId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(User model) => DeleteAsyncDefault(model, Delete);

        public object ReadValue()
        {
            throw new NotImplementedException();
        }
    }
}