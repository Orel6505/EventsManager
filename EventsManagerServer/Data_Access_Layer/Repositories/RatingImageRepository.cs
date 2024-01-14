using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class RatingImageRepository : Repository, IRepository<RatingImage>
    {
        public bool Delete(int id)
        {
            string sql = $"DELETE FROM RatingImages WHERE ImageId=@ImageId";
            this.AddParameters("ImageId", id.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(RatingImage model)
        {
            string sql = $"DELETE FROM RatingImages WHERE ImageId=@ImageId";
            this.AddParameters("ImageId", model.ImageId.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Insert(RatingImage model)
        {
            string sql = $"INSERT INTO RatingImages(ImageLocation,RatingId) VALUES(@ImageLocation,@RatingId)";
            this.AddParameters("ImageLocation", model.ImageLocation); //prevents SQL Injection
            this.AddParameters("RatingId", model.RatingId.ToString()); //prevents SQL Injection
            return this.dbContext.Create(sql) > 0;
        }

        public RatingImage Read(object id)
        {
            string sql = $"SELECT FROM RatingImages WHERE ImageId=@ImageId";
            this.AddParameters("ImageId", id.ToString()); //prevents SQL Injection
            return this.modelFactory.RatingImageModelCreator.CreateModel(this.dbContext.Read(sql));
            //returns RatingImage
        }

        public List<RatingImage> ReadAll()
        {
            List<RatingImage> RatingImages = new List<RatingImage>();
            string sql = "SELECT * FROM RatingImages";
            IDataReader dataReader = this.dbContext.Read(sql);
            while (dataReader.Read() == true)
                RatingImages.Add(this.modelFactory.RatingImageModelCreator.CreateModel(dataReader));
            return RatingImages;
        }

        public object ReadValue()
        {
            throw new NotImplementedException();
        }

        public bool Update(RatingImage model)
        {
            string sql = "UPDATE RatingImages SET RatingId=@RatingId, ImageLocation=@ImageLocation where ImageId=@ImageId";
            this.AddParameters("ImageId", model.ImageId.ToString()); //prevents SQL Injection
            this.AddParameters("ImageLocation", model.ImageLocation); //prevents SQL Injection
            this.AddParameters("RatingId", model.RatingId.ToString()); //prevents SQL Injection
            return this.dbContext.Update(sql) > 0;
        }
    }
}