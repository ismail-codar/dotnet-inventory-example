using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_inventory_example.Models
{
    public class UnitsRepository : SqlRepository<Units>, IUnitssRepository
    {
        public UnitsRepository(NorthwindDbContext context)
            : base(context)
        {
        }

        public override IQueryable<Units> GetAll()
        {
            return EfDbSet;
        }

        public override async Task<Units> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(o => o.UnitId == (int)id);
        }

        public async Task Insert(Units Units)
        {
            await EfDbSet.AddAsync(Units);
        }

        public async Task Update(Units Units)
        {
            var entry = Context.Entry(Units);
            if (entry.State == EntityState.Detached)
            {
                var attachedUnits = await GetById(Units.UnitId);
                if (attachedUnits != null)
                {
                    Context.Entry(attachedUnits).CurrentValues.SetValues(Units);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Units Units)
        {
            EfDbSet.Remove(Units);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IUnitssRepository
    {
        Task Insert(Units Units);
        Task Update(Units Units);
        void Delete(Units Units);
        void Save();
    }
}