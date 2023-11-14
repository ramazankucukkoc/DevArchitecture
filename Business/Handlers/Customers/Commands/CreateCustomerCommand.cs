using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Extensions;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using IResult = Core.Utilities.Results.IResult;

namespace Business.Handlers.Customers.Commands
{
    public class CreateCustomerCommand: IRequest<IResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string MobilePhones { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

       
        public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, IResult>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly IHttpContextAccessor _contextAccessor;

            public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IHttpContextAccessor contextAccessor)
            {
                _customerRepository = customerRepository;
                _contextAccessor = contextAccessor;
            }
            [SecuredOperation(Priority = 1)]
            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            public async Task<IResult> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
            {
                var getByIdCustomer = await _customerRepository.GetAsync(c => c.Email == request.Email);
                if (getByIdCustomer != null) return new ErrorResult(Messages.CustomerAvailable);
                var customer = new Customer()
                {
                    Name = request.Name,
                    Code = request.Code,
                    MobilePhones = request.MobilePhones,
                    Address = request.Address,
                    Email = request.Email,
                    CreatedDate = DateTime.Now,
                    isDeleted = false,
                    Status = true,
                    CreatedUserId = _contextAccessor.HttpContext.User.GetUserId(),
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedUserId = _contextAccessor.HttpContext.User.GetUserId()
                };
                _customerRepository.Add(customer);
                await _customerRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);

            }
        }

    }
}
