using System.Collections.Generic;
using System.Threading.Tasks;

namespace Specter.Data
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> InsertAsync(T model);
        Task<bool> UpdateAsync(T model);
        Task<bool> DeleteAsync(T model);
    }
}
