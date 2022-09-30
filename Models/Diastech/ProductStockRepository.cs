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

        public override async Task<ProductStock> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.Id == (int)id);
        }

        public async Task Insert(ProductStock ProductStock)
        {
            await EfDbSet.AddAsync(ProductStock);
        }

        public async Task Update(ProductStock ProductStock)
        {
            var entry = Context.Entry(ProductStock);
            if (entry.State == EntityState.Detached)
            {
                var attachedOrder = await GetById(ProductStock.Id);
                if (attachedOrder != null)
                {
                    Context.Entry(attachedOrder).CurrentValues.SetValues(ProductStock);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(ProductStock ProductStock)
        {
            EfDbSet.Remove(ProductStock);
        }

        public void Save()
        {
            Context.SaveChanges();
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