using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Extensions;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using IResult = Core.Utilities.Results.IResult;

namespace Business.Handlers.WarehouseInformations.Commands
{
    public class UpdateWarehouseInformationCommand:IRequest<IResult>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; } //adet
        public bool ReadyForSale { get; set; }//SatışaHazırmı?

        public class UpdateWarehouseInformationCommandHandler : IRequestHandler<UpdateWarehouseInformationCommand, IResult>
        {
            private readonly IWarehouseInformationRepository _warehouseInformationRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IProductRepository _productRepository;
            public UpdateWarehouseInformationCommandHandler(IWarehouseInformationRepository warehouseInformationRepository,
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
            public async Task<IResult> Handle(UpdateWarehouseInformationCommand request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetAsync(p => p.Id == request.ProductId);
                if (product == null) return new ErrorResult("Güncellemek istediğini ürün depomızda mevcut değildir.");


                var isThereAnyOrderInformation = await _warehouseInformationRepository.GetAsync(o => o.Id == request.Id);
                isThereAnyOrderInformation.ProductId = request.ProductId;
                var checkStock= isThereAnyOrderInformation.Count += request.Count;
                isThereAnyOrderInformation.ReadyForSale = request.ReadyForSale;
                isThereAnyOrderInformation.LastUpdatedUserId = _contextAccessor.HttpContext.User.GetUserId();
                isThereAnyOrderInformation.LastUpdatedDate = DateTime.Now;

                _warehouseInformationRepository.Update(isThereAnyOrderInformation);
                await _warehouseInformationRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
                
            }
        }
    }
}
