using System.IO;
using System.Reflection;

namespace Specter.Data
{
    public static class EnvironmentInfo
    {
        public static readonly string ExecutingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static readonly string ConnectionString = $"Data Source={Path.Combine(ExecutingPath, "db.db")};";
    }
}