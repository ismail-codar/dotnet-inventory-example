using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_inventory_example.Models
{
    public class ProductsRepository : SqlRepository<Product>, IProductsRepository
    {
        public ProductsRepository(InventoryDbContext context)
            : base(context)
        {
        }

        public override IQueryable<Product> GetAll()
        {
            return EfDbSet.Include("Unit");
        }

        public override async Task<Product> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(o => o.ProductId == (int)id);
        }

        public async Task Insert(Product Product)
        {
            await EfDbSet.AddAsync(Product);
        }

        public async Task Update(Product Product)
        {
            var entry = Context.Entry(Product);
            if (entry.State == EntityState.Detached)
            {
                var attachedProduct = await GetById(Product.ProductId);
                if (attachedProduct != null)
                {
                    Context.Entry(attachedProduct).CurrentValues.SetValues(Product);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Product Product)
        {
            EfDbSet.Remove(Product);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IProductsRepository
    {
        Task Insert(Product Product);
        Task Update(Product Product);
        void Delete(Product Product);
        void Save();
    }
}