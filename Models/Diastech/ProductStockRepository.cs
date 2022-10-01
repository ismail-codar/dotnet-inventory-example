using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_inventory_example.Models
{
    public class ProductStockRepository : SqlRepository<ProductStock>, IProductStockRepository
    {
        public ProductStockRepository(NorthwindDbContext context)
            : base(context)
        {
        }

        public override IQueryable<ProductStock> GetAll()
        {
            return EfDbSet;
        }

        public async Task Insert(ProductStock ProductStock)
        {
            await EfDbSet.AddAsync(ProductStock);
        }

        public void Delete(ProductStock ProductStock)
        {
            EfDbSet.Remove(ProductStock);
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public Task Update(ProductStock ProductStock)
        {
            throw new System.NotImplementedException();
        }

        public override Task<ProductStock> GetById(object id)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IProductStockRepository
    {
        Task Insert(ProductStock ProductStock);
        Task Update(ProductStock ProductStock);
        void Delete(ProductStock ProductStock);
        void Save();
    }
}