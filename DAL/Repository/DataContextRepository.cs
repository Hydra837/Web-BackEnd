using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Tools; // Assurez-vous que ce namespace contient la classe Connection
using DAL.Data; // Assurez-vous que ce namespace contient EFDbContextData

namespace DAL.Repository
{
    public class DataContextRepository : RepositoryBase
    {
        private readonly string _connectionString;

        public DataContextRepository(Connection connection) : base(connection)
        {
           

            _connectionString = connection.ToString();
        }

        public EFDbContextData CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<EFDbContextData>();
            optionsBuilder.UseSqlServer(_connectionString);

            return new EFDbContextData(optionsBuilder.Options);
        }
    }
}
