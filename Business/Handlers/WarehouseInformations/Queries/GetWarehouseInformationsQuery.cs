using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Dtos;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.WarehouseInformations.Queries
{
    public class GetWarehouseInformationsQuery:IRequest<IDataResult<IEnumerable<WarehouseInformationDto>>>
    {
        public class GetWarehouseInformationsQueryHandler : IRequestHandler<GetWarehouseInformationsQuery, IDataResult<IEnumerable<WarehouseInformationDto>>>
        {
            private readonly IWarehouseInformationRepository _warehouseInformationRepository;
            public GetWarehouseInformationsQueryHandler(IWarehouseInformationRepository warehouseInformationRepository)
            {
                _warehouseInformationRepository = warehouseInformationRepository;
            }
            [SecuredOperation(Priority = 1)]
            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<IEnumerable<WarehouseInformationDto>>> Handle(GetWarehouseInformationsQuery request, CancellationToken cancellationToken)
            {
                var warehouseInformationList = await _warehouseInformationRepository.GetAll();
                return new SuccessDataResult<IEnumerable<WarehouseInformationDto>>(warehouseInformationList);
            }
        }
    }
}
