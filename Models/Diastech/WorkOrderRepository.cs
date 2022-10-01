using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_inventory_example.Models
{
    public class WorkOrderRepository : SqlRepository<WorkOrder>, IWorkOrderRepository
    {
        public WorkOrderRepository(InventoryDbContext context)
            : base(context)
        {
        }

        public override IQueryable<WorkOrder> GetAll()
        {
            return EfDbSet;
        }

        public override async Task<WorkOrder> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.WorkOrderId == (int)id);
        }

        public async Task Insert(WorkOrder WorkOrder)
        {
            await EfDbSet.AddAsync(WorkOrder);
        }

        public async Task Update(WorkOrder WorkOrder)
        {
            var entry = Context.Entry(WorkOrder);
            if (entry.State == EntityState.Detached)
            {
                var attachedOrder = await GetById(WorkOrder.WorkOrderId);
                if (attachedOrder != null)
                {
                    Context.Entry(attachedOrder).CurrentValues.SetValues(WorkOrder);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(WorkOrder WorkOrder)
        {
            EfDbSet.Remove(WorkOrder);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IWorkOrderRepository
    {
        Task Insert(WorkOrder WorkOrder);
        Task Update(WorkOrder WorkOrder);
        void Delete(WorkOrder WorkOrder);
        void Save();
    }
}