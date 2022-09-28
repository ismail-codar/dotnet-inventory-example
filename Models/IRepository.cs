using System.Linq;
using System.Threading.Tasks;

namespace dotnet_inventory_example.Models
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        Task<T> GetById(object id);
    }
}