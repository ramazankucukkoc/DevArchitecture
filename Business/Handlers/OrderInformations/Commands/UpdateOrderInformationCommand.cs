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

namespace Business.Handlers.OrderInformations.Commands
{
    public class UpdateOrderInformationCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Count { get; set; }//Şipariş adeti

        public class UpdateOrderInformationCommandHandler : IRequestHandler<UpdateOrderInformationCommand, IResult>
        {
            private readonly IOrderInformationRepository _orderInformationRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public UpdateOrderInformationCommandHandler(IOrderInformationRepository orderInformationRepository, IHttpContextAccessor httpContextAccessor)
            {
                _orderInformationRepository = orderInformationRepository;
                _httpContextAccessor = httpContextAccessor;
            }
            [SecuredOperation(Priority = 1)]
            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            public async Task<IResult> Handle(UpdateOrderInformationCommand request, CancellationToken cancellationToken)
            {
                var isThereAnyOrderInformation = await _orderInformationRepository.GetAsync(o => o.Id == request.Id);

                isThereAnyOrderInformation.CustomerId = request.CustomerId;
                isThereAnyOrderInformation.ProductId = request.ProductId;
                isThereAnyOrderInformation.Count = request.Count;
                isThereAnyOrderInformation.LastUpdatedUserId = _httpContextAccessor.HttpContext.User.GetUserId();
                isThereAnyOrderInformation.LastUpdatedDate = DateTime.Now;

                _orderInformationRepository.Update(isThereAnyOrderInformation);
                await _orderInformationRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }

    }
}
