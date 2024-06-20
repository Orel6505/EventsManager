using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace EventsManager.Data_Access_Layer
{
    public class OleDbContext : DbContext
    {
        static OleDbContext oleDbContext;
        private static readonly object blocker = new object();
        private OleDbContext()
        {
            this.connection = new OleDbConnection();
            this.connection.ConnectionString = CommonParameters.Location.Db; // tells which type of sql we are connecting to and tell its directory
        }

        public static OleDbContext GetInstance()
        {
            lock (blocker) { 
                if (oleDbContext == null)               
                    oleDbContext = new OleDbContext(); //singleton design pattern
                return oleDbContext;
            }
        }
    }
}
