using Business.BusinessAspects;
using Business.Handlers.OperationClaims.Queries;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Dtos;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Colors.Queries
{
    public class GetColorLookupQuery : IRequest<IDataResult<IEnumerable<SelectionItem>>>
    {
        public class GetColorLookupQueryHandler : IRequestHandler<GetColorLookupQuery,
          IDataResult<IEnumerable<SelectionItem>>>
        {
            private readonly IColorRepository _colorRepository ;

            public GetColorLookupQueryHandler(IColorRepository colorRepository )
            {
                _colorRepository = colorRepository;
            }

            [SecuredOperation(Priority = 1)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<IEnumerable<SelectionItem>>> Handle(GetColorLookupQuery request, CancellationToken cancellationToken)
            {
                var list = await _colorRepository.GetListAsync();

                var color = list.Select(x => new SelectionItem()
                {
                    Id = x.Id.ToString(),
                    Label =  x.Name
                });
                return new SuccessDataResult<IEnumerable<SelectionItem>>(
                    color);
            }
        }

    }
}
