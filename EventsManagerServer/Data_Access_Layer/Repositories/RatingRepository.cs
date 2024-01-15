using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class RatingRepository : Repository, IRepository<Rating>
    {
        public RatingRepository(DbContext dbContext) : base(dbContext) { }

        public bool Delete(int id)
        {
            string sql = $"DELETE FROM Ratings WHERE RatingId=@RatingId";
            this.AddParameters("RatingId", id.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(Rating model)
        {
            string sql = $"DELETE FROM Ratings WHERE RatingId=@RatingId";
            this.AddParameters("RatingId", model.RatingId.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Insert(Rating model)
        {
            string sql = $"INSERT INTO Ratings(RatingTitle,RatingDesc,RatingStars,RatingDate,UserId,HallId) VALUES(@RatingTitle,@RatingDesc,@RatingStars,@RatingDate,@UserId,@HallId)";
            this.AddParameters("RatingTitle", model.RatingTitle); //prevents SQL Injection
            this.AddParameters("RatingDesc", model.RatingDesc); //prevents SQL Injection
            this.AddParameters("RatingStars", model.RatingStars.ToString()); //prevents SQL Injection
            this.AddParameters("RatingDate", model.RatingDate.ToString()); //prevents SQL Injection
            this.AddParameters("UserId", model.UserId.ToString()); //prevents SQL Injection
            this.AddParameters("HallId", model.HallId.ToString()); //prevents SQL Injection
            return this.dbContext.Create(sql) > 0;
        }

        public Rating Read(object id)
        {
            string sql = $"SELECT FROM Ratings WHERE RatingId=@RatingId";
            this.AddParameters("RatingId", id.ToString()); //prevents SQL Injection
            using (IDataReader dataReader = this.dbContext.Read(sql))
                return this.modelFactory.RatingModelCreator.CreateModel(dataReader);
            //returns Rating
        }

        public List<Rating> ReadAll()
        {
            List<Rating> Ratings = new List<Rating>();
            string sql = "SELECT * FROM Ratings";
            using (IDataReader dataReader = this.dbContext.Read(sql))
                while (dataReader.Read() == true)
                    Ratings.Add(this.modelFactory.RatingModelCreator.CreateModel(dataReader));
            return Ratings;
        }

        public object ReadValue()
        {
            throw new NotImplementedException();
        }

        public bool Update(Rating model)
        {
            string sql = "UPDATE Ratings SET RatingTitle=@RatingTitle, RatingDesc=@RatingDesc, RatingStars=@RatingStars, RatingDate=@RatingDate, UserId=@UserId, HallId=@HallId where RatingId=@RatingId";
            this.AddParameters("RatingId", model.RatingId.ToString()); //prevents SQL Injection
            this.AddParameters("RatingTitle", model.RatingTitle); //prevents SQL Injection
            this.AddParameters("RatingDesc", model.RatingDesc); //prevents SQL Injection
            this.AddParameters("RatingStars", model.RatingStars.ToString()); //prevents SQL Injection
            this.AddParameters("RatingDate", model.RatingDate.ToString()); //prevents SQL Injection
            this.AddParameters("UserId", model.UserId.ToString()); //prevents SQL Injection
            this.AddParameters("HallId", model.HallId.ToString()); //prevents SQL Injection
            return this.dbContext.Update(sql) > 0;
        }
    }
}