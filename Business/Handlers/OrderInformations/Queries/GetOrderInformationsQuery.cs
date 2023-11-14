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

namespace Business.Handlers.OrderInformations.Queries
{
    public class GetOrderInformationsQuery:IRequest<IDataResult<IEnumerable<OrderInformationDto>>>
    {
        public class GetOrderInformationsQueryHandler : IRequestHandler<GetOrderInformationsQuery, IDataResult<IEnumerable<OrderInformationDto>>>
        {
            private readonly IOrderInformationRepository _orderInformationRepository;
            public GetOrderInformationsQueryHandler(IOrderInformationRepository orderInformationRepository)
            {
                _orderInformationRepository = orderInformationRepository;
            }
            [SecuredOperation(Priority = 1)]
            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<IEnumerable<OrderInformationDto>>> Handle(GetOrderInformationsQuery request, CancellationToken cancellationToken)
            {
                var orderInformationList = await _orderInformationRepository.GetAll();
                return new SuccessDataResult<IEnumerable<OrderInformationDto>>(orderInformationList);
            }
        }

    }
}
