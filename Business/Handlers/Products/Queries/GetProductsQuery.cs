using AutoMapper;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Dtos;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Products.Queries
{
    public class GetProductsQuery : IRequest<IDataResult<IEnumerable<ProductDto>>>
    {

        public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IDataResult<IEnumerable<ProductDto>>>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;

            public GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _mapper = mapper;
            }
            [SecuredOperation(Priority = 1)]
            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<IEnumerable<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
            {
                var producList = await _productRepository.GetListAsync(p => p.isDeleted == false);
                var productDtoList = producList.Select(product => _mapper.Map<ProductDto>(product)).ToList();

                return new SuccessDataResult<IEnumerable<ProductDto>>(productDtoList);

            }
        }
    }
}
