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

namespace Business.Handlers.Customers.Queries
{
    public class GetCustomerQuery:IRequest<IDataResult<CustomerDto>>
    {
        public int Id { get; set; }

        public class GetColorQueryHandler : IRequestHandler<GetCustomerQuery, IDataResult<CustomerDto>>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly IMapper _mapper;

            public GetColorQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
            {
                _customerRepository = customerRepository;
                _mapper = mapper;
            }

            [SecuredOperation(Priority = 1)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<CustomerDto>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
            {
                var customer = await _customerRepository.GetAsync(p => p.Id == request.Id && p.isDeleted == false);
                var customerDto = _mapper.Map<CustomerDto>(customer);
                return new SuccessDataResult<CustomerDto>(customerDto);
            }
        }
    }
}
