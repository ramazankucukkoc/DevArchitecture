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

namespace Business.Handlers.Colors.Queries
{
    public class GetColorsQuery:IRequest<IDataResult<IEnumerable<ColorDto>>>
    {

        public class GetColorsQueryHandler : IRequestHandler<GetColorsQuery, IDataResult<IEnumerable<ColorDto>>>
        {
            private readonly IColorRepository _colorRepository;
            private readonly IMapper _mapper;

            public GetColorsQueryHandler(IColorRepository colorRepository, IMapper mapper)
            {
                _colorRepository = colorRepository;
                _mapper = mapper;
            }
            [SecuredOperation(Priority = 1)]
            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<IEnumerable<ColorDto>>> Handle(GetColorsQuery request, CancellationToken cancellationToken)
            {
                var colorList = await _colorRepository.GetListAsync(c=>c.isDeleted==false);
                var colorDtoList = colorList.Select(color => _mapper.Map<ColorDto>(color)).ToList();

                return new SuccessDataResult<IEnumerable<ColorDto>>(colorDtoList);
            }
        }
    }
}
