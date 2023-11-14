using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Customers.Commands
{
    public class DeleteCustomerCommand:IRequest<IResult>
    {
        public int Id { get; set; }

      
        public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, IResult>
        {
            private readonly ICustomerRepository _customerRepository;
            public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
            {
                _customerRepository = customerRepository;
            }

            [SecuredOperation(Priority = 1)]
            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            public async Task<IResult> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
            {
                var customerToDelete = _customerRepository.Get(p => p.Id == request.Id);
                customerToDelete.isDeleted = true;
                _customerRepository.Update(customerToDelete);
                await _customerRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}
