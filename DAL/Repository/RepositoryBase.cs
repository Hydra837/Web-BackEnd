using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace DAL.Repository
{
	public class RepositoryBase
  	{
		
			private readonly Connection connection;

			protected Connection Connection
			{
				get { return connection; }
			}

			public RepositoryBase(Connection connection)
			{
				this.connection = connection;
			}

	}
}
