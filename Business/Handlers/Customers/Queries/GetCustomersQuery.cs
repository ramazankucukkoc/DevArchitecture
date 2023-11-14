using AutoMapper;
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Customers.Queries
{
    public class GetCustomersQuery:IRequest<IDataResult<IEnumerable<CustomerDto>>>
    {

        public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IDataResult<IEnumerable<CustomerDto>>>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly IMapper _mapper;

            public GetCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
            {
                _customerRepository = customerRepository;
                _mapper = mapper;
            }
            [SecuredOperation(Priority = 1)]
            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<IEnumerable<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
            {
                var customerList = await _customerRepository.GetListAsync(c=>c.isDeleted==false);
                var customerDtoList = customerList.Select(customer => _mapper.Map<CustomerDto>(customer)).ToList();

                return new SuccessDataResult<IEnumerable<CustomerDto>>(customerDtoList);

            }
        }
    }
}
