using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_inventory_example.Models
{
    public class WorkOrderProductRepository : SqlRepository<WorkOrderProduct>, IWorkOrderProductRepository
    {
        public WorkOrderProductRepository(InventoryDbContext context)
            : base(context)
        {
        }

        public override IQueryable<WorkOrderProduct> GetAll()
        {
            return EfDbSet.Include("Product");
        }

        public override async Task<WorkOrderProduct> GetById(object id)
        {
            throw new System.NotImplementedException("GetById");
        }

        public async Task Insert(WorkOrderProduct WorkOrderProduct)
        {
            await EfDbSet.AddAsync(WorkOrderProduct);
        }

        public async Task Update(WorkOrderProduct WorkOrderProduct)
        {
            throw new System.NotImplementedException("Update");
        }

        public void Delete(WorkOrderProduct WorkOrderProduct)
        {
            EfDbSet.Remove(WorkOrderProduct);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IWorkOrderProductRepository
    {
        Task Insert(WorkOrderProduct WorkOrderProduct);
        Task Update(WorkOrderProduct WorkOrderProduct);
        void Delete(WorkOrderProduct WorkOrderProduct);
        void Save();
    }
}