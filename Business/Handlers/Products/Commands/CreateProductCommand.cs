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

namespace Business.Handlers.Products.Commands
{
    public class CreateProductCommand:IRequest<IResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ColorId { get; set; }
        public Size Size { get; set; }

        public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, IResult>
        {
            private readonly IProductRepository _productRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IWarehouseInformationRepository _warehouseInformationRepository;

            public CreateProductCommandHandler(IProductRepository productRepository,
                IHttpContextAccessor contextAccessor,
                IWarehouseInformationRepository warehouseInformationRepository)
            {
                _productRepository = productRepository;
                _contextAccessor = contextAccessor;
                _warehouseInformationRepository = warehouseInformationRepository;
            }
            [SecuredOperation(Priority = 1)]
            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            public async Task<IResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                var getByProduct = await _productRepository.GetAsync(p=>p.Name.ToLower().Trim() == request.Name.ToLower().Trim());
                if (getByProduct != null) return new ErrorResult(Messages.ProductIsAlreadyName);

                var product = new Product()
                {
                    Name = request.Name,
                    CreatedDate = DateTime.Now,
                    isDeleted = false,
                    Status = true,
                    CreatedUserId = _contextAccessor.HttpContext.User.GetUserId(),
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedUserId = _contextAccessor.HttpContext.User.GetUserId(),
                    ColorId = request.ColorId,
                    Size = (Size)Convert.ToInt16(request.Size),
                };
                _productRepository.Add(product);
                await _productRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}
