using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Colors.Commands
{
    public class DeleteColorCommand : IRequest<IResult>
    {
        public int Id { get; set; }


        public class DeleteColorCommandHandler : IRequestHandler<DeleteColorCommand, IResult>
        {
            private readonly IColorRepository _colorRepository;
            public DeleteColorCommandHandler(IColorRepository colorRepository)
            {
                _colorRepository = colorRepository;
            }

            [SecuredOperation(Priority = 1)]
            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            public async Task<IResult> Handle(DeleteColorCommand request, CancellationToken cancellationToken)
            {
                var colorToDelete = _colorRepository.Get(p => p.Id == request.Id);
                colorToDelete.isDeleted = true;
                _colorRepository.Update(colorToDelete);
                await _colorRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}
