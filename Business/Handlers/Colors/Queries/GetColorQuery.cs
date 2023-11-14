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

namespace Business.Handlers.Colors.Queries
{
    public class GetColorQuery:IRequest<IDataResult<ColorDto>>
    {
        public int Id { get; set; }

        public class GetColorQueryHandler : IRequestHandler<GetColorQuery, IDataResult<ColorDto>>
        {
            private readonly IColorRepository _colorRepository;
            private readonly IMapper _mapper;

            public GetColorQueryHandler(IColorRepository colorRepository, IMapper mapper)
            {
                _colorRepository = colorRepository;
                _mapper = mapper;
            }

            [SecuredOperation(Priority = 1)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<ColorDto>> Handle(GetColorQuery request, CancellationToken cancellationToken)
            {
                var color = await _colorRepository.GetAsync(p => p.Id == request.Id && p.isDeleted == false);
                var colorDto = _mapper.Map<ColorDto>(color);
                return new SuccessDataResult<ColorDto>(colorDto);
            }
        }
    }
}
