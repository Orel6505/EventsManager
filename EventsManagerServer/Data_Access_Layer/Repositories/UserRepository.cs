using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class UserRepository : Repository, IRepository<User>
    {
        public UserRepository(DbContext dbContext) : base(dbContext) { }

        public bool Delete(int id)
        {
            string sql = $"DELETE FROM Users WHERE UserId=@UserId";
            this.AddParameters("UserId", id.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(User model)
        {
            string sql = $"DELETE FROM Users WHERE UserId=@UserId";
            this.AddParameters("UserId", model.UserId.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Insert(User model)
        {
            string sql = $"INSERT INTO Users (FirstName, LastName, UserName, PassWordHash, Email, Address, PhoneNum, CreationDate, Salt) VALUES (@FirstName, @LastName, @UserName, @PassWordHash, @Email, @Address, @PhoneNum, @CreationDate, @Salt);";
            this.AddParameters("FirstName", model.FirstName); //prevents SQL Injection
            this.AddParameters("LastName", model.LastName); //prevents SQL Injection
            this.AddParameters("UserName", model.UserName); //prevents SQL Injection
            this.AddParameters("PassWordHash", model.UserPassword.HashPassword); //prevents SQL Injection
            this.AddParameters("Email", model.Email); //prevents SQL Injection
            this.AddParameters("Address", model.Address ?? DBNull.Value.ToString());
            this.AddParameters("PhoneNum", model.PhoneNum); //prevents SQL Injection
            this.AddParameters("CreationDate", model.CreationDate); //prevents SQL Injection
            this.AddParameters("Salt", model.UserPassword.Salt); //prevents SQL Injection
            return this.dbContext.Create(sql) > 0;
        }

        public User Read(object id)
        {
            string sql = $"SELECT * FROM Users WHERE UserId=@UserId";
            this.AddParameters("UserId", id.ToString()); //prevents SQL Injection
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.UserModelCreator.CreateModel(dataReader);
            }
            //returns User
        }
        public User GetLoginByUserName(string UserName)
        {
            string sql = $"SELECT Users.UserName, Users.PasswordHash, Users.Salt FROM Users WHERE Users.UserName=@UserName;";
            this.AddParameters("UserName", UserName); //prevents SQL Injection
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.LoginUserModelCreator.CreateModel(dataReader);
            }
            //returns User
        }

        public UserType UserTypeByUserId(string UserId)
        {
            string sql = $"SELECT UserTypes.UserTypeId,UserTypes.UserTypeName FROM UserTypes INNER JOIN Users ON UserTypes.UserTypeId = Users.UserTypeId WHERE Users.UserId=@UserId;";
            this.AddParameters("UserId", UserId); //prevents SQL Injection
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.UserTypeModelCreator.CreateModel(dataReader);
            }
            //returns UserType
        }

        public User GetUser2FAByUserName(string UserName)
        {
            string sql = $"SELECT Users.UserName, Users.UserId, Users.UserTypeId, Users.Email FROM Users WHERE Users.UserName=@UserName;";
            this.AddParameters("UserName", UserName); //prevents SQL Injection
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.User2FAModelCreator.CreateModel(dataReader);
            }
            //returns User
        }

        public List<User> ReadAll()
        {
            List<User> Users = new List<User>();
            string sql = "SELECT * FROM Users";
            using (IDataReader dataReader = this.dbContext.Read(sql))
                while (dataReader.Read() == true)
                    Users.Add(this.modelFactory.UserModelCreator.CreateModel(dataReader));
            return Users;
        }

        public bool IsAvailableUserName(string UserName)
        {
            string sql = "SELECT * FROM Users WHERE UserName=@UserName";
            this.AddParameters("UserName", UserName);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                return dataReader.Read();
            };
        }

        public object ReadValue()
        {
            throw new NotImplementedException();
        }

        public bool Update(User model)
        {
            string sql = "UPDATE Users SET FirstName=@FirstName, LastName=@LastName, UserName=@UserName, PassWordHash=@PassWordHash, Email=@Email, Address=@Address, PhoneNum=@PhoneNum,CreationDate=@CreationDate, Salt=@Salt where UserId=@UserId";
            this.AddParameters("UserId", model.UserId.ToString()); //prevents SQL Injection
            this.AddParameters("FirstName", model.FirstName); //prevents SQL Injection
            this.AddParameters("LastName", model.LastName); //prevents SQL Injection
            this.AddParameters("UserName", model.UserName); //prevents SQL Injection
            this.AddParameters("PassWordHash", model.UserPassword.HashPassword); //prevents SQL Injection
            this.AddParameters("Salt", model.UserPassword.Salt); //prevents SQL Injection
            this.AddParameters("Email", model.Email); //prevents SQL Injection
            this.AddParameters("Address", model.Address); //prevents SQL Injection
            this.AddParameters("PhoneNum", model.PhoneNum); //prevents SQL Injection
            this.AddParameters("CreationDate", model.CreationDate); //prevents SQL Injection
            return this.dbContext.Update(sql) > 0;
        }
    }
}