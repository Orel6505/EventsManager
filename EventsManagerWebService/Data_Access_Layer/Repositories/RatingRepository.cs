using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class RatingRepository : Repository, IRepository<Rating>
    {
        public RatingRepository(DbContext dbContext, ILogger<RatingRepository> logger) : base(dbContext, logger) { }

        public bool Insert(Rating model)
        {
            const string sql = """
                INSERT INTO Ratings(RatingTitle,RatingDesc,RatingStars,RatingDate,UserId,HallId)
                VALUES(@RatingTitle,@RatingDesc,@RatingStars,@RatingDate,@UserId,@HallId)
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@RatingTitle", model.RatingTitle);
                AddParameter(cmd, "@RatingDesc", model.RatingDesc);
                AddParameter(cmd, "@RatingStars", model.RatingStars);
                AddParameter(cmd, "@RatingDate", model.RatingDate);
                AddParameter(cmd, "@UserId", model.UserId);
                AddParameter(cmd, "@HallId", model.HallId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "INSERT Rating failed {@Rating}", model);
                throw;
            }
        }

        public Task<bool> InsertAsync(Rating model) => InsertAsyncDefault(model, Insert);

        public Rating? Read(object id)
        {
            const string sql = "SELECT * FROM Ratings WHERE RatingId=@RatingId";

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@RatingId", id);

                using IDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    logger.LogWarning("Rating with ID {Id} not found", id);
                    return null;
                }

                return modelFactory.RatingModelCreator.CreateModel(reader);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "READ Rating failed for RatingId={Id}", id);
                throw;
            }
        }

        public Task<Rating> ReadAsync(object id) => ReadAsyncDefault(id, Read);

        public int ReadByRatingDate(string RatingDate)
        {
            const string sql = "SELECT Ratings.RatingId FROM Ratings ORDER BY Ratings.RatingId DESC";

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@RatingDate", RatingDate);

                using IDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    logger.LogWarning("No ratings found for RatingDate={RatingDate}", RatingDate);
                    return 0;
                }

                return Convert.ToInt32(reader["RatingId"]);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ReadByRatingDate failed for RatingDate={RatingDate}", RatingDate);
                throw;
            }
        }

        public List<Rating> ReadAll()
        {
            const string sql = "SELECT * FROM Ratings";
            List<Rating> ratings = new();

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            using IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ratings.Add(modelFactory.RatingModelCreator.CreateModel(reader));
            }

            return ratings;
        }

        public Task<List<Rating>> ReadAllAsync() => ReadAllAsyncDefault(ReadAll);

        public List<Rating> ReadRatingsByHallIdId(int HallId)
        {
            const string sql = "SELECT * FROM Ratings WHERE HallId=@HallId ORDER BY Ratings.RatingId DESC";
            List<Rating> ratings = new();

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@HallId", HallId);

                using IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ratings.Add(modelFactory.RatingModelCreator.CreateModel(reader));
                }

                return ratings;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ReadRatingsByHallIdId failed for HallId={HallId}", HallId);
                throw;
            }
        }

        public bool Update(Rating model)
        {
            const string sql = """
                UPDATE Ratings
                SET RatingTitle=@RatingTitle,
                    RatingDesc=@RatingDesc,
                    RatingStars=@RatingStars,
                    RatingDate=@RatingDate,
                    UserId=@UserId,
                    HallId=@HallId
                WHERE RatingId=@RatingId
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@RatingId", model.RatingId);
                AddParameter(cmd, "@RatingTitle", model.RatingTitle);
                AddParameter(cmd, "@RatingDesc", model.RatingDesc);
                AddParameter(cmd, "@RatingStars", model.RatingStars);
                AddParameter(cmd, "@RatingDate", model.RatingDate);
                AddParameter(cmd, "@UserId", model.UserId);
                AddParameter(cmd, "@HallId", model.HallId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "UPDATE Rating failed {@Rating}", model);
                throw;
            }
        }

        public Task<bool> UpdateAsync(Rating model) => UpdateAsyncDefault(model, Update);

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Ratings WHERE RatingId=@RatingId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@RatingId", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(int id) => DeleteAsyncDefault(id, Delete);

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(Rating model)
        {
            const string sql = "DELETE FROM Ratings WHERE RatingId=@RatingId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@RatingId", model.RatingId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(Rating model) => DeleteAsyncDefault(model, Delete);

        public object ReadValue()
        {
            throw new NotImplementedException();
        }
    }
}