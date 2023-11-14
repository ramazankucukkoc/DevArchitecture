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

namespace Business.Handlers.OrderInformations.Commands
{
    public class CreateOrderInformationCommand:IRequest<IResult>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Count { get; set; }//Şipariş adeti
        public class CreateOrderInformationCommandHandler : IRequestHandler<CreateOrderInformationCommand, IResult>
        {
            private readonly IOrderInformationRepository _orderInformationRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IWarehouseInformationRepository _warehouseInformationRepository;

            public CreateOrderInformationCommandHandler(IOrderInformationRepository orderInformationRepository,
                IHttpContextAccessor httpContextAccessor,
                IWarehouseInformationRepository warehouseInformationRepository)
            {
                _orderInformationRepository = orderInformationRepository;
                _httpContextAccessor = httpContextAccessor;
                _warehouseInformationRepository = warehouseInformationRepository;
            }
            [SecuredOperation(Priority = 1)]
            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            public async Task<IResult> Handle(CreateOrderInformationCommand request, CancellationToken cancellationToken)
            {
                var warehouseInformation = await _warehouseInformationRepository.GetAsync(w => w.ProductId == request.ProductId);
                if (warehouseInformation == null)return new ErrorResult(Messages.ProductNotFound);

                if (warehouseInformation.Count < request.Count) return new ErrorResult(Messages.ProductOutOfCount + $" {warehouseInformation.Count} adet ürün kaldı..");

                if (warehouseInformation.ReadyForSale==true)
                {
                    var orderInformation = new OrderInformation()
                    {
                        ProductId = request.ProductId,
                        CustomerId = request.CustomerId,
                        Count = request.Count,
                        CreatedDate = DateTime.Now,
                        isDeleted = false,
                        Status = true,
                        CreatedUserId = _httpContextAccessor.HttpContext.User.GetUserId(),
                        LastUpdatedDate = DateTime.Now,
                        LastUpdatedUserId = _httpContextAccessor.HttpContext.User.GetUserId()
                    };
                    _orderInformationRepository.Add(orderInformation);
                    await _orderInformationRepository.SaveChangesAsync();
                    warehouseInformation.Count-=request.Count;
                    await _warehouseInformationRepository.SaveChangesAsync();
                    return new SuccessResult(Messages.Added);
                }
                return new ErrorResult(Messages.OrderNotReady);
            }
        }
    }
}
