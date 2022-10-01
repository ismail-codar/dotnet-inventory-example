using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_inventory_example.Models
{
    public class StockRoomRepository : SqlRepository<StockRoom>, IStockRoomRepository
    {
        public StockRoomRepository(InventoryDbContext context)
            : base(context)
        {
        }

        public override IQueryable<StockRoom> GetAll()
        {
            return EfDbSet;
        }

        public override async Task<StockRoom> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.StockRoomId == (int)id);
        }

        public async Task Insert(StockRoom StockRoom)
        {
            await EfDbSet.AddAsync(StockRoom);
        }

        public async Task Update(StockRoom StockRoom)
        {
            var entry = Context.Entry(StockRoom);
            if (entry.State == EntityState.Detached)
            {
                var attachedOrder = await GetById(StockRoom.StockRoomId);
                if (attachedOrder != null)
                {
                    Context.Entry(attachedOrder).CurrentValues.SetValues(StockRoom);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(StockRoom StockRoom)
        {
            EfDbSet.Remove(StockRoom);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IStockRoomRepository
    {
        Task Insert(StockRoom StockRoom);
        Task Update(StockRoom StockRoom);
        void Delete(StockRoom StockRoom);
        void Save();
    }
}