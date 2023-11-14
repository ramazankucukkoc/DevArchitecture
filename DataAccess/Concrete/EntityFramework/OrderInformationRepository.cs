using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class OrderInformationRepository : EfEntityRepositoryBase<OrderInformation, ProjectDbContext>, IOrderInformationRepository
    {
        public OrderInformationRepository(ProjectDbContext context) : base(context)
        {
        }

        public async Task<List<OrderInformationDto>> GetAll()
        {
            var getList = (from oi in Context.OrderInformations
                           join p in Context.Products
                           on oi.ProductId equals p.Id
                           join c in Context.Customers
                           on oi.CustomerId equals c.Id
                           select new OrderInformationDto
                           {
                               Id = oi.Id,
                               ProductId = p.Id,
                               ProductName = p.Name,
                               CustomerId = c.Id,
                               Count = oi.Count,
                               CustomerName = c.Name,
                               CustomerEmail = c.Email,
                               CustomerAddress = c.Address,
                               isDeleted = oi.isDeleted,
                               LastUpdatedDate= oi.LastUpdatedDate.ToString("yyyy-MM-dd"),
                           });

            return await getList.Where(io=>io.isDeleted==false).ToListAsync();
        }

    }
}
