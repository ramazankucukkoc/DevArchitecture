using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.OrderInformations.Commands
{
    public class DeleteOrderInformationCommand:IRequest<IResult>
    {
        public int Id { get; set; }
        public class DeleteOrderInformationCommandHandler : IRequestHandler<DeleteOrderInformationCommand, IResult>
        {
            private readonly IOrderInformationRepository _orderInformationRepository;
            public DeleteOrderInformationCommandHandler(IOrderInformationRepository orderInformationRepository)
            {
                _orderInformationRepository = orderInformationRepository;
            }
            [SecuredOperation(Priority = 1)]
            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            public async Task<IResult> Handle(DeleteOrderInformationCommand request, CancellationToken cancellationToken)
            {
                var orderInformationToDelete = _orderInformationRepository.Get(p => p.Id == request.Id);
                orderInformationToDelete.isDeleted = true;
                _orderInformationRepository.Update(orderInformationToDelete);
                await _orderInformationRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);


            }
        }
    }
}
