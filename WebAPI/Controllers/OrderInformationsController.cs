using Business.Handlers.OrderInformations.Commands;
using Business.Handlers.OrderInformations.Queries;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class OrderInformationsController : BaseApiController
    {/// <summary>
     /// Add OrderInformation.
     /// </summary>
     /// <param name="createOrderInformationCommand"></param>
     /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateOrderInformationCommand createOrderInformationCommand)
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(createOrderInformationCommand));
        }
        /// <summary>
        /// List OrderInformation
        /// </summary>
        /// <remarks>bla bla bla OrderInformation</remarks>
        /// <return>OrderInformation List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderInformationDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetOrderInformationsQuery()));
        }
        /// <summary>
        /// Update OrderInformation.
        /// </summary>
        /// <param name="updateOrderInformationDto"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateOrderInformationDto updateOrderInformationDto)
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(new UpdateOrderInformationCommand { Id = updateOrderInformationDto.Id, Count = updateOrderInformationDto.Count, CustomerId = updateOrderInformationDto.CustomerId, ProductId = updateOrderInformationDto.ProductId }));
        }
        /// <summary>
        /// Delete OrderInformation.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(new DeleteOrderInformationCommand { Id = id }));
        }

    }
}
