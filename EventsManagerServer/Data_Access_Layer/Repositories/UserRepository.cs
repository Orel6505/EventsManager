using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
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
            string sql = $"INSERT INTO Users(FirstName,LastName,UserName,PassWordHash,Email,Address,PhoneNum,CreationDate) VALUES(@FirstName,@LastName,@UserName,@UserName,@PassWordHash,@Email,@Address,@PhoneNum,@CreationDate)";
            this.AddParameters("FirstName", model.FirstName); //prevents SQL Injection
            this.AddParameters("LastName", model.LastName); //prevents SQL Injection
            this.AddParameters("UserName", model.UserName); //prevents SQL Injection
            this.AddParameters("PassWordHash", model.PassWordHash); //prevents SQL Injection
            this.AddParameters("Email", model.Email); //prevents SQL Injection
            this.AddParameters("Address", model.Address); //prevents SQL Injection
            this.AddParameters("PhoneNum", model.PhoneNum); //prevents SQL Injection
            this.AddParameters("CreationDate", model.CreationDate); //prevents SQL Injection
            return this.dbContext.Create(sql) > 0;
        }

        public User Read(object id)
        {
            string sql = $"SELECT FROM Users WHERE UserId=@UserId";
            this.AddParameters("UserId", id.ToString()); //prevents SQL Injection
            using (IDataReader dataReader = this.dbContext.Read(sql))
                return this.modelFactory.UserModelCreator.CreateModel(dataReader);
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

        public object ReadValue()
        {
            throw new NotImplementedException();
        }

        public bool Update(User model)
        {
            string sql = "UPDATE Users SET FirstName=@FirstName, LastName=@LastName, UserName=@UserName, PassWordHash=@PassWordHash, Email=@Email, Address=@Address, PhoneNum=@PhoneNum,CreationDate=@CreationDate where UserId=@UserId";
            this.AddParameters("UserId", model.UserId.ToString()); //prevents SQL Injection
            this.AddParameters("FirstName", model.FirstName); //prevents SQL Injection
            this.AddParameters("LastName", model.LastName); //prevents SQL Injection
            this.AddParameters("UserName", model.UserName); //prevents SQL Injection
            this.AddParameters("PassWordHash", model.PassWordHash); //prevents SQL Injection
            this.AddParameters("Email", model.Email); //prevents SQL Injection
            this.AddParameters("Address", model.Address); //prevents SQL Injection
            this.AddParameters("PhoneNum", model.PhoneNum); //prevents SQL Injection
            this.AddParameters("CreationDate", model.CreationDate); //prevents SQL Injection
            return this.dbContext.Update(sql) > 0;
        }
    }
}