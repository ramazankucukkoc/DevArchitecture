using Business.Fakes.Handlers.Authorizations;
using Business.Fakes.Handlers.OperationClaims;
using Business.Fakes.Handlers.UserClaims;
using Core.Utilities.IoC;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Helpers
{
    public static class OperationClaimCreatorMiddleware
    {
        public static async Task UseDbOperationClaimCreator(this IApplicationBuilder app)
        {
            var mediator = ServiceTool.ServiceProvider.GetService<IMediator>();
            foreach (var operationName in GetOperationNames())
            {
                await mediator.Send(new CreateOperationClaimInternalCommand
                {
                    ClaimName = operationName
                });
            }

            var operationClaims = (await mediator.Send(new GetOperationClaimsInternalQuery())).Data;
            var user = await mediator.Send(new RegisterUserInternalCommand
            {
                FullName = "System Admin",
                Password = "Q1w212*_*",
                Email = "admin@adminmail.com",
            });
            await mediator.Send(new CreateUserClaimsInternalCommand
            {
                UserId = 5,
                OperationClaims = operationClaims
            });
        }

        private static IEnumerable<string> GetOperationNames()
        {
            var operationNames = new List<string>()
            {
                "Yonetici",
                "Kullanici",
                "MusteriTemsilcisi"
            };

            return operationNames;
        }
    }
}
