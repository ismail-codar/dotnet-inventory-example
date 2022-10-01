using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_inventory_example.Models
{
    public class StockBuildingRepository : SqlRepository<StockBuilding>, IStockBuildingRepository
    {
        public StockBuildingRepository(InventoryDbContext context)
            : base(context)
        {
        }

        public override IQueryable<StockBuilding> GetAll()
        {
            return EfDbSet;
        }

        public override async Task<StockBuilding> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.StockBuildingId == (int)id);
        }

        public async Task Insert(StockBuilding StockBuilding)
        {
            await EfDbSet.AddAsync(StockBuilding);
        }

        public async Task Update(StockBuilding StockBuilding)
        {
            var entry = Context.Entry(StockBuilding);
            if (entry.State == EntityState.Detached)
            {
                var attachedOrder = await GetById(StockBuilding.StockBuildingId);
                if (attachedOrder != null)
                {
                    Context.Entry(attachedOrder).CurrentValues.SetValues(StockBuilding);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(StockBuilding StockBuilding)
        {
            EfDbSet.Remove(StockBuilding);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IStockBuildingRepository
    {
        Task Insert(StockBuilding StockBuilding);
        Task Update(StockBuilding StockBuilding);
        void Delete(StockBuilding StockBuilding);
        void Save();
    }
}