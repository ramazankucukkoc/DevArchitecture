using AutoMapper;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Dtos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.WarehouseInformations.Queries
{
    public class GetWarehouseInformationQuery:IRequest<IDataResult<WarehouseInformationDto>>
    {
        public int Id { get; set; }
        public class GetWarehouseInformationQueryHandler : IRequestHandler<GetWarehouseInformationQuery, IDataResult<WarehouseInformationDto>>
        {
            private readonly IWarehouseInformationRepository _warehouseInformationRepository;
            private readonly IMapper _mapper;

            public GetWarehouseInformationQueryHandler(IWarehouseInformationRepository warehouseInformationRepository, IMapper mapper)
            {
                _warehouseInformationRepository = warehouseInformationRepository;
                _mapper = mapper;
            }
            [SecuredOperation(Priority = 1)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<WarehouseInformationDto>> Handle(GetWarehouseInformationQuery request, CancellationToken cancellationToken)
            {
                var warehouseInformation= await _warehouseInformationRepository.GetAsync(p => p.Id == request.Id);
                var warehouseInformationDto = _mapper.Map<WarehouseInformationDto>(warehouseInformation);
                return new SuccessDataResult<WarehouseInformationDto>(warehouseInformationDto);
            }
        }

    }
}
