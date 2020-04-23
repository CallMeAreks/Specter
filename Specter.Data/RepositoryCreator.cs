namespace Specter.Data
{
    public static class RepositoryCreator
    {
        public static IRepository<T> Create<T>() where T : class => new Repository<T>();
    }
}
