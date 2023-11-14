using Core.DataAccess.EntityFramework;
using Core.Entities.Dtos;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class ProductRepository : EfEntityRepositoryBase<Product, ProjectDbContext>, IProductRepository
    {
        public ProductRepository(ProjectDbContext context) : base(context)
        {
        }

  

        public async Task<IEnumerable<SelectionItem>> GetColorSelectedList(int colorId)
        {

            //var list = await(from c in Context.Colors
            //                 join product in Context.Products on c.Id equals product.ColorId
            //                 where product.ColorId == colorId
            //                 select new SelectionItem()
            //                 {
            //                     Id = c.Id.ToString(),
            //                     Label = c.Name
            //                 }).ToListAsync();

            var list = await (from p in Context.Products
                              join color in Context.Colors on p.ColorId equals color.Id
                              where p.Id == colorId
                              select new SelectionItem()
                              {
                                  Id = color.Id.ToString(),
                                  Label = color.Name
                              }).ToListAsync();

            return list;
        }
    }
}
