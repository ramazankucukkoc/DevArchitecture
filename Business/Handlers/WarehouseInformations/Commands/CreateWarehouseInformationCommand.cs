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

namespace Business.Handlers.WarehouseInformations.Commands
{
    public class CreateWarehouseInformationCommand:IRequest<IResult>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; } //adet
        public class CreateWarehouseInformationCommandHandler : IRequestHandler<CreateWarehouseInformationCommand, IResult>
        {
            private readonly IWarehouseInformationRepository _warehouseInformationRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IProductRepository _productRepository;


            public CreateWarehouseInformationCommandHandler(IWarehouseInformationRepository warehouseInformationRepository, 
                IHttpContextAccessor contextAccessor,
                IProductRepository productRepository)
            {
                _warehouseInformationRepository = warehouseInformationRepository;
                _contextAccessor = contextAccessor;
                _productRepository = productRepository;
            }
            [SecuredOperation(Priority = 1)]
            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            public async Task<IResult> Handle(CreateWarehouseInformationCommand request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetAsync(p => p.Id == request.ProductId);
                if (product != null) return new ErrorResult("Eklemek istediğiniz depoda ürün mevcuttur.");

                var warehouseInformation = new WarehouseInformation()
                {
                    Count = request.Count,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = _contextAccessor.HttpContext.User.GetUserId(),
                    LastUpdatedDate = DateTime.Now,
                    ProductId = request.ProductId,
                    isDeleted = false,
                    LastUpdatedUserId = _contextAccessor.HttpContext.User.GetUserId(),
                    ReadyForSale = true,
                    Status = true
                };
                _warehouseInformationRepository.Add(warehouseInformation);
                await _warehouseInformationRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}
