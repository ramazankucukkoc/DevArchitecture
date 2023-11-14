using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class WarehouseInformationRepository : EfEntityRepositoryBase<WarehouseInformation, ProjectDbContext>, IWarehouseInformationRepository
    {
        public WarehouseInformationRepository(ProjectDbContext context) : base(context)
        {
        }

        public async Task<List<WarehouseInformationDto>> GetAll()
        {
            var getList = from w in Context.WarehouseInformations
                          join p in Context.Products
                           on w.ProductId equals p.Id
                          select new WarehouseInformationDto
                          {
                              Id = w.Id,
                              ProductId = p.Id,
                              ProductName = p.Name,
                              Count = w.Count,
                              ReadyForSale = w.ReadyForSale,
                              isDeleted=w.isDeleted
                          };

            return await getList.Where(w=>w.isDeleted ==false).ToListAsync();
        }
    }
}
