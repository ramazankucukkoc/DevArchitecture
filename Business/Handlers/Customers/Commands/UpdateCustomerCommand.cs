using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Extensions;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using IResult = Core.Utilities.Results.IResult;

namespace Business.Handlers.Customers.Commands
{
    public class UpdateCustomerCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string MobilePhones { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public class UpdateColorCommandHandler : IRequestHandler<UpdateCustomerCommand, IResult>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            public UpdateColorCommandHandler(ICustomerRepository customerRepository,IHttpContextAccessor httpContextAccessor)
            {
                _customerRepository = customerRepository;
                _contextAccessor = httpContextAccessor;
            }
            [SecuredOperation(Priority = 1)]
            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            public async Task<IResult> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
            {
                var isThereAnyCustomer = await _customerRepository.GetAsync(u => u.Id == request.Id);
                if (isThereAnyCustomer != null)
                {
                    var customer = await _customerRepository.GetAsync(c => c.Id != isThereAnyCustomer.Id && c.Email == request.Email);
                    if (customer != null) return new ErrorResult(Messages.EmailAddressIsUsed);
                }

                isThereAnyCustomer.Name = request.Name;
                isThereAnyCustomer.Email = request.Email;
                isThereAnyCustomer.MobilePhones = request.MobilePhones;
                isThereAnyCustomer.Address = request.Address;
                isThereAnyCustomer.Code = request.Code;
                isThereAnyCustomer.LastUpdatedUserId = _contextAccessor.HttpContext.User.GetUserId();
                isThereAnyCustomer.LastUpdatedDate = DateTime.Now;

                _customerRepository.Update(isThereAnyCustomer);
                await _customerRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);


            }
        }
    }
}
