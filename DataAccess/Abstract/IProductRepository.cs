using Core.DataAccess;
using Core.Entities.Concrete;
using Core.Entities.Dtos;
using Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IProductRepository:IEntityRepository<Product>
    {
        Task<IEnumerable<SelectionItem>> GetColorSelectedList(int colorId);
    }
}
