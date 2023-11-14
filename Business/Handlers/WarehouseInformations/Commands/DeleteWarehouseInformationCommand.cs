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

namespace Business.Handlers.WarehouseInformations.Commands
{
    public class DeleteWarehouseInformationCommand:IRequest<IResult>
    {
        public int Id { get; set; }
        public class DeleteWarehouseInformationCommandHandler : IRequestHandler<DeleteWarehouseInformationCommand, IResult>
        {
            private readonly IWarehouseInformationRepository _warehouseInformationRepository;

            public DeleteWarehouseInformationCommandHandler(IWarehouseInformationRepository warehouseInformationRepository)
            {
                _warehouseInformationRepository = warehouseInformationRepository;
            }
            [SecuredOperation(Priority = 1)]
            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            public async Task<IResult> Handle(DeleteWarehouseInformationCommand request, CancellationToken cancellationToken)
            {
                var warehouseInformationToDelete = _warehouseInformationRepository.Get(p => p.Id == request.Id);
                warehouseInformationToDelete.isDeleted = true;
                _warehouseInformationRepository.Update(warehouseInformationToDelete);
                await _warehouseInformationRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}
