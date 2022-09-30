using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_inventory_example.Models
{
    public class ProductUnitRepository : SqlRepository<ProductUnit>, IProductUnitRepository
    {
        public ProductUnitRepository(NorthwindDbContext context)
            : base(context)
        {
        }

        public override IQueryable<ProductUnit> GetAll()
        {
            return EfDbSet;
        }

        public override async Task<ProductUnit> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(o => o.UnitId == (int)id);
        }

        public async Task Insert(ProductUnit ProductUnit)
        {
            await EfDbSet.AddAsync(ProductUnit);
        }

        public async Task Update(ProductUnit ProductUnit)
        {
            var entry = Context.Entry(ProductUnit);
            if (entry.State == EntityState.Detached)
            {
                var attachedProductUnit = await GetById(ProductUnit.UnitId);
                if (attachedProductUnit != null)
                {
                    Context.Entry(attachedProductUnit).CurrentValues.SetValues(ProductUnit);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(ProductUnit ProductUnit)
        {
            EfDbSet.Remove(ProductUnit);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IProductUnitRepository
    {
        Task Insert(ProductUnit ProductUnit);
        Task Update(ProductUnit ProductUnit);
        void Delete(ProductUnit ProductUnit);
        void Save();
    }
}