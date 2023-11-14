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
using System.Threading;
using System.Threading.Tasks;
using IResult = Core.Utilities.Results.IResult;

namespace Business.Handlers.Colors.Commands
{
    public class UpdateColorCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class UpdateColorCommandHandler : IRequestHandler<UpdateColorCommand, IResult>
        {
            private readonly IColorRepository _colorRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            public UpdateColorCommandHandler(IColorRepository colorRepository,IHttpContextAccessor httpContextAccessor)
            {
                _colorRepository = colorRepository;
                _contextAccessor = httpContextAccessor;
            }
            [SecuredOperation(Priority = 1)]
            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            public async Task<IResult> Handle(UpdateColorCommand request, CancellationToken cancellationToken)
            {
                var isThereAnyColor = await _colorRepository.GetAsync(u => u.Id == request.Id);
                if (isThereAnyColor != null)
                {
                    var customer = await _colorRepository.GetAsync(c => c.Id != isThereAnyColor.Id && c.Name.ToLower().Trim() == request.Name.ToLower().Trim());
                    if (customer != null) return new ErrorResult(Messages.EmailAddressIsUsed);
                }
                isThereAnyColor.Name = request.Name;
                isThereAnyColor.LastUpdatedUserId = _contextAccessor.HttpContext.User.GetUserId();
                isThereAnyColor.LastUpdatedDate = DateTime.Now;

                _colorRepository.Update(isThereAnyColor);
                await _colorRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);


            }
        }
    }
}
