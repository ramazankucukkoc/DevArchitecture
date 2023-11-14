using AutoMapper;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Dtos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Products.Queries
{
    public class GetProductQuery:IRequest<IDataResult<ProductDto>>
    {
        public int Id { get; set; }
        public class GetProductQueryHandler : IRequestHandler<GetProductQuery, IDataResult<ProductDto>>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;

            public GetProductQueryHandler(IProductRepository productRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _mapper = mapper;
            }
            [SecuredOperation(Priority = 1)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetAsync(p => p.Id == request.Id && p.isDeleted == false);
                var productDto = _mapper.Map<ProductDto>(product);
                return new SuccessDataResult<ProductDto>(productDto);
            }
        }
    }
}
