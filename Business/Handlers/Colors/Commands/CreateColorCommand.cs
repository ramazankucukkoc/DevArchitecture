using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Extensions;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using IResult = Core.Utilities.Results.IResult;

namespace Business.Handlers.Colors.Commands
{
    public class CreateColorCommand: IRequest<IResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }

     
        public class CreateColorCommandHandler : IRequestHandler<CreateColorCommand, IResult>
        {
            private readonly IColorRepository _colorRepository;
            private readonly IHttpContextAccessor _contextAccessor;

            public CreateColorCommandHandler(IColorRepository colorRepository , IHttpContextAccessor contextAccessor)
            {
                _colorRepository = colorRepository;
                _contextAccessor = contextAccessor;
            }
            [SecuredOperation(Priority = 1)]
            [CacheRemoveAspect()]
            [LogAspect(typeof(FileLogger))]
            public async Task<IResult> Handle(CreateColorCommand request, CancellationToken cancellationToken)
            {
                var getByIdColor = await _colorRepository.GetAsync(c => c.Name.ToLower().Trim() == request.Name.ToLower().Trim());
                if (getByIdColor != null) return new ErrorResult(Messages.CustomerAvailable);
                var color = new Color()
                {
                    Name = request.Name,
                    CreatedDate = DateTime.Now,
                    isDeleted = false,
                    Status = true,
                    CreatedUserId = _contextAccessor.HttpContext.User.GetUserId(),
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedUserId = _contextAccessor.HttpContext.User.GetUserId()
                };
                _colorRepository.Add(color);
                await _colorRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);

            }
        }

    }
}
