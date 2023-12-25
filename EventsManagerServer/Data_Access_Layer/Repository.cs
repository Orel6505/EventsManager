using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;

namespace EventsManager.Data_Access_Layer
{
    public class Repository
    {
        protected DbContext dbContext;
        protected ModelFactory modelFactory;

        public Repository()
        {
            this.dbContext = OleDbContext.GetInstance();
            this.modelFactory = new ModelFactory();
        }

        protected void AddParameters(string paramName, int paramValue)
        {
            OleDbParameter oleDbParameter = new OleDbParameter(paramName, paramValue);
            this.dbContext.AddParameters(oleDbParameter);
        }
    }
}