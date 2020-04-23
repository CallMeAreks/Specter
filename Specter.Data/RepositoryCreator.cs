using Specter.Data.Models;

namespace Specter.Data
{
    public static class RepositoryCreator
    {
        public static IRepository<T> Create<T>() where T  : Entity => new Repository<T>();
    }
}
