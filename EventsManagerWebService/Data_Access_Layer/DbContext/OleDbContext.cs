using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using Microsoft.Extensions.Configuration;

namespace EventsManager.Data_Access_Layer
{
	public class OleDbContext : DbContext
	{
		public OleDbContext(IConfiguration configuration)
		{
			string connectionString = configuration.GetConnectionString("DefaultConnection");
			
			if (string.IsNullOrEmpty(connectionString))
				throw new InvalidOperationException("Connection string 'DefaultConnection' not found in configuration");

			connectionString = connectionString.Replace("{AppPath}", Directory.GetCurrentDirectory());

			this.connection = new OleDbConnection(connectionString);
		}
	}
}
