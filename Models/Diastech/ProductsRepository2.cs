using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_inventory_example.Models
{
    public class ProductsRepository2 : SqlRepository<Product2>, IProduct2sRepository
    {
        public ProductsRepository2(NorthwindDbContext context)
            : base(context)
        {
        }

        public override IQueryable<Product2> GetAll()
        {
            return EfDbSet.Include("Unit");
        }

        public override async Task<Product2> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(o => o.ProductId == (int)id);
        }

        public async Task Insert(Product2 Product2)
        {
            await EfDbSet.AddAsync(Product2);
        }

        public async Task Update(Product2 Product2)
        {
            var entry = Context.Entry(Product2);
            if (entry.State == EntityState.Detached)
            {
                var attachedProduct2 = await GetById(Product2.ProductId);
                if (attachedProduct2 != null)
                {
                    Context.Entry(attachedProduct2).CurrentValues.SetValues(Product2);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Product2 Product2)
        {
            EfDbSet.Remove(Product2);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IProduct2sRepository
    {
        Task Insert(Product2 Product2);
        Task Update(Product2 Product2);
        void Delete(Product2 Product2);
        void Save();
    }
}