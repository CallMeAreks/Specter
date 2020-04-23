using Dapper.Contrib.Extensions;
using Specter.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Specter.Data
{
    internal sealed class Repository<T> : IRepository<T> where T : Entity
    {
        private static string RoothPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string ConnectionString = $"Data Source={RoothPath}\\db.db;";

        public async Task<T> GetAsync(object id)
        {
            using IDbConnection connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            return await connection.GetAsync<T>(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using IDbConnection connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            return await connection.GetAllAsync<T>();
        }

        public async Task<int> InsertAsync(T model)
        {
            using IDbConnection connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            return await connection.InsertAsync(model);
        }

        public async Task<bool> UpdateAsync(T model)
        {
            using IDbConnection connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            return await connection.UpdateAsync(model);
        }

        public async Task<bool> DeleteAsync(T model)
        {
            using IDbConnection connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            return await connection.DeleteAsync(model);
        }
    }
}
