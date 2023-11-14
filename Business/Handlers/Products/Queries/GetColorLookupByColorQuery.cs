using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Dtos;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Products.Queries
{
    public class GetProductLookupByColorQuery : IRequest<IDataResult<IEnumerable<SelectionItem>>>
    {
        public int Id { get; set; }

        public class GetColorLookupByColorQueryHandler : IRequestHandler<GetProductLookupByColorQuery,
            IDataResult<IEnumerable<SelectionItem>>>
        {
            private readonly IProductRepository _productRepository;
          

            public GetColorLookupByColorQueryHandler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
              
            }

            [SecuredOperation(Priority = 1)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<IEnumerable<SelectionItem>>> Handle(GetProductLookupByColorQuery request, CancellationToken cancellationToken)
            {
                var data = await _productRepository.GetColorSelectedList(request.Id);
                return new SuccessDataResult<IEnumerable<SelectionItem>>(data);
            }
        }
    }
}
