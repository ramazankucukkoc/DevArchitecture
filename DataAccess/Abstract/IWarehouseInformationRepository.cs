using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IWarehouseInformationRepository:IEntityRepository<WarehouseInformation>
    {
        Task<List<WarehouseInformationDto>> GetAll();

    }
}
