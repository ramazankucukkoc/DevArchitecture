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

namespace Business.Handlers.Products.Commands
{
    public class UpdateProductCommand:IRequest<IResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ColorId { get; set; }
        public Size Size { get; set; }


        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, IResult>
        {
            private readonly IProductRepository _productRepository;
            private readonly IHttpContextAccessor _contextAccessor;

            public UpdateProductCommandHandler(IProductRepository productRepository, IHttpContextAccessor contextAccessor)
            {
                _productRepository = productRepository;
                _contextAccessor = contextAccessor;
            }
            [SecuredOperation(Priority = 1)]
            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            public async Task<IResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                var isThereAnyProduct = await _productRepository.GetAsync(u => u.Id == request.Id);
                if (isThereAnyProduct != null)
                {
                    var product = await _productRepository.GetAsync(c => c.Id != isThereAnyProduct.Id && c.Name == request.Name);
                    if (product != null) return new ErrorResult(Messages.ProductNameIsNotUsed);
                }

                isThereAnyProduct.Name = request.Name;
                isThereAnyProduct.ColorId= request.ColorId;
                isThereAnyProduct.Size = (Size)Convert.ToInt16(request.Size);
                isThereAnyProduct.LastUpdatedUserId = _contextAccessor.HttpContext.User.GetUserId();
                isThereAnyProduct.LastUpdatedDate = DateTime.Now;

                _productRepository.Update(isThereAnyProduct);
                await _productRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);


            }
        }

    }
}
