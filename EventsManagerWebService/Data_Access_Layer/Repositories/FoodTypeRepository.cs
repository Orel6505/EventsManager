using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


namespace EventsManager.Data_Access_Layer
{
    public class FoodTypeRepository : Repository, IRepository<FoodType>
    {
        public FoodTypeRepository(DbContext dbContext) : base(dbContext) { }

        public bool Delete(int id)
        {
            string sql = $"DELETE FROM FoodTypes WHERE FoodTypeId=@FoodTypeId";
            this.AddParameters("FoodTypeId", id.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(FoodType model)
        {
            string sql = $"DELETE FROM FoodTypes WHERE FoodTypeId=@FoodTypeId";
            this.AddParameters("FoodTypeId", model.FoodTypeId.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Insert(FoodType model)
        {
            string sql = $"INSERT INTO FoodTypes(FoodTypeId,FoodTypeName) VALUES(@FoodTypeId,@FoodTypeName)";
            this.AddParameters("FoodTypeId", model.FoodTypeId.ToString()); //prevents SQL Injection
            this.AddParameters("FoodTypeName", model.FoodTypeName); //prevents SQL Injection
            return this.dbContext.Create(sql) > 0;
        }

        public FoodType Read(object id)
        {
            string sql = $"SELECT FROM FoodTypes WHERE FoodTypeId=@FoodTypeId";
            this.AddParameters("FoodTypeId", id.ToString()); //prevents SQL Injection
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.FoodTypeModelCreator.CreateModel(dataReader);
            }
            //returns Menu
        }

        public List<FoodType> ReadAll()
        {
            List<FoodType> FoodTypes = new List<FoodType>();
            string sql = "SELECT * FROM FoodTypes";
            using (IDataReader dataReader = this.dbContext.Read(sql))
                while (dataReader.Read() == true)
                    FoodTypes.Add(this.modelFactory.FoodTypeModelCreator.CreateModel(dataReader));
            return FoodTypes;
        }

        public object ReadValue()
        {
            throw new NotImplementedException();
        }

        public bool Update(FoodType model)
        {
            string sql = "UPDATE FoodTypes SET FoodTypeName=@FoodTypeName where FoodTypeId=@FoodTypeId";
            this.AddParameters("FoodTypeId", model.FoodTypeId.ToString()); //prevents SQL Injection
            this.AddParameters("FoodTypeName", model.FoodTypeName); //prevents SQL Injection
            return this.dbContext.Update(sql) > 0;
        }
    }
}