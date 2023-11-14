using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IOrderInformationRepository:IEntityRepository<OrderInformation>
    {
        Task<List<OrderInformationDto>> GetAll();

    }
}
