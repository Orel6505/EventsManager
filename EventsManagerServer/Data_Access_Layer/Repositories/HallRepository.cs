using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class HallRepository : Repository, IRepository<Hall>
    {
        public HallRepository(DbContext dbContext) : base(dbContext) { }

        public bool Delete(int id)
        {
            string sql = $"DELETE FROM Halls WHERE HallId=@HallId";
            this.AddParameters("HallId", id.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(Hall model)
        {
            string sql = $"DELETE FROM Halls WHERE HallId=@HallId";
            this.AddParameters("HallId", model.HallId.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Insert(Hall model)
        {
            string sql = $"INSERT INTO Halls(HallName,HallDesc,HallImage,MaxPeople) VALUES(@HallName,@HallDesc,@HallImage,@MaxPeople)";
            this.AddParameters("HallName", model.HallName); //prevents SQL Injection
            this.AddParameters("HallDesc", model.HallDesc); //prevents SQL Injection
            this.AddParameters("HallImage", model.HallImage); //prevents SQL Injection
            this.AddParameters("HallImage", model.MaxPeople.ToString()); //prevents SQL Injection
            return this.dbContext.Create(sql) > 0;
        }

        public Hall Read(object id)
        {
            string sql = $"SELECT FROM Halls WHERE HallId=@HallId";
            this.AddParameters("HallId", id.ToString()); //prevents SQL Injection
            using (IDataReader dataReader = this.dbContext.Read(sql))
                return this.modelFactory.HallModelCreator.CreateModel(dataReader);
            //returns Hall
        }

        public List<Hall> ReadAll()
        {
            List<Hall> Halls = new List<Hall>();
            string sql = "SELECT * FROM Halls";
            using (IDataReader dataReader = this.dbContext.Read(sql))
                while (dataReader.Read() == true)
                    Halls.Add(this.modelFactory.HallModelCreator.CreateModel(dataReader));
            return Halls;
        }

        public object ReadValue()
        {
            throw new NotImplementedException();
        }

        public bool Update(Hall model)
        {
            string sql = "UPDATE Halls SET HallName=@HallName, HallDesc=@HallDesc, HallImage=@HallImage, MaxPeople=@MaxPeople where HallId=@HallId";
            this.AddParameters("HallId", model.HallId.ToString()); //prevents SQL Injection
            this.AddParameters("HallName", model.HallName); //prevents SQL Injection
            this.AddParameters("HallDesc", model.HallDesc); //prevents SQL Injection
            this.AddParameters("HallImage", model.HallImage); //prevents SQL Injection
            this.AddParameters("HallImage", model.MaxPeople.ToString()); //prevents SQL Injection
            return this.dbContext.Update(sql) > 0;
        }
    }
}