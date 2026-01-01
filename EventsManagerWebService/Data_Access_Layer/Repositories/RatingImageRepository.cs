using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class RatingImageRepository : Repository, IRepository<RatingImage>
    {
        public RatingImageRepository(DbContext dbContext, ILogger<RatingImageRepository> logger) : base(dbContext, logger) { }

        public bool Insert(RatingImage model)
        {
            const string sql = """
                INSERT INTO RatingImages(ImageLocation,RatingId)
                VALUES(@ImageLocation,@RatingId)
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@ImageLocation", model.ImageLocation);
                AddParameter(cmd, "@RatingId", model.RatingId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "INSERT RatingImage failed {@RatingImage}", model);
                throw;
            }
        }

        public Task<bool> InsertAsync(RatingImage model) => InsertAsyncDefault(model, Insert);

        public RatingImage? Read(object id)
        {
            const string sql = "SELECT * FROM RatingImages WHERE ImageId=@ImageId";

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@ImageId", id);

                using IDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    logger.LogWarning("RatingImage with ID {Id} not found", id);
                    return null;
                }

                return modelFactory.RatingImageModelCreator.CreateModel(reader);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "READ RatingImage failed for ImageId={Id}", id);
                throw;
            }
        }

        public Task<RatingImage> ReadAsync(object id) => ReadAsyncDefault(id, Read);

        public List<RatingImage> ReadAll()
        {
            const string sql = "SELECT * FROM RatingImages";
            List<RatingImage> ratingImages = new();

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            using IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ratingImages.Add(modelFactory.RatingImageModelCreator.CreateModel(reader));
            }

            return ratingImages;
        }

        public Task<List<RatingImage>> ReadAllAsync() => ReadAllAsyncDefault(ReadAll);

        public List<RatingImage> ReadByRatingId(int id)
        {
            const string sql = "SELECT * FROM RatingImages WHERE RatingId=@RatingId";
            List<RatingImage> ratingImages = new();

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@RatingId", id);

                using IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ratingImages.Add(modelFactory.RatingImageModelCreator.CreateModel(reader));
                }

                return ratingImages;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ReadByRatingId failed for RatingId={Id}", id);
                throw;
            }
        }

        public bool Update(RatingImage model)
        {
            const string sql = """
                UPDATE RatingImages
                SET RatingId=@RatingId,
                    ImageLocation=@ImageLocation
                WHERE ImageId=@ImageId
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@ImageId", model.ImageId);
                AddParameter(cmd, "@ImageLocation", model.ImageLocation);
                AddParameter(cmd, "@RatingId", model.RatingId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "UPDATE RatingImage failed {@RatingImage}", model);
                throw;
            }
        }

        public Task<bool> UpdateAsync(RatingImage model) => UpdateAsyncDefault(model, Update);

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM RatingImages WHERE ImageId=@ImageId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@ImageId", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(int id) => DeleteAsyncDefault(id, Delete);

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(RatingImage model)
        {
            const string sql = "DELETE FROM RatingImages WHERE ImageId=@ImageId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@ImageId", model.ImageId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(RatingImage model) => DeleteAsyncDefault(model, Delete);

        public object ReadValue()
        {
            throw new NotImplementedException();
        }
    }
}