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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IResult = Core.Utilities.Results.IResult;

namespace Business.Handlers.Products.Commands
{
    public class UpdateProductColorCommand:IRequest<IResult>
    {
        public int ProductId { get; set; }
        public int[] ColorId { get; set; }

        public class UpdateProductColorCommandHandler : IRequestHandler<UpdateProductColorCommand, IResult>
        {
            private readonly IProductRepository _productRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            public UpdateProductColorCommandHandler(IProductRepository productRepository,IHttpContextAccessor httpContextAccessor)
            {
                _contextAccessor = httpContextAccessor;
                _productRepository = productRepository;
            }

            [SecuredOperation(Priority = 1)]
            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            public async Task<IResult> Handle(UpdateProductColorCommand request, CancellationToken cancellationToken)
            {
                var productList = await _productRepository.GetAsync(p => p.Id == request.ProductId);
                var arrayLength = request.ColorId.Length;
                if (arrayLength > 1) return new ErrorResult("Bir ürünün bir rengi olur birden fazla olamaz");

                productList.LastUpdatedDate = DateTime.Now;
                productList.LastUpdatedUserId = _contextAccessor.HttpContext.User.GetUserId();
                productList.ColorId = request.ColorId.SingleOrDefault();
                await _productRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}
