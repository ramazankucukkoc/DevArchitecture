using Business.Handlers.Users.Queries;
using Business.Handlers.WarehouseInformations.Commands;
using Business.Handlers.WarehouseInformations.Queries;
using Core.Entities.Dtos;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class WarehouseInformationsController : BaseApiController
    {
        /// <summary>
        /// Add WarehouseInformation.
        /// </summary>
        /// <param name="createWarehouseInformationCommand"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateWarehouseInformationCommand createWarehouseInformationCommand)
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(createWarehouseInformationCommand));
        }
        /// <summary>
        /// List WarehouseInformations
        /// </summary>
        /// <remarks>bla bla bla WarehouseInformations</remarks>
        /// <return>WarehouseInformations List</return>
        /// <response code="200"></response>
   
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WarehouseInformationDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetWarehouseInformationsQuery()));
        }
        /// <summary>
        /// Update WarehouseInformation.
        /// </summary>
        /// <param name="updateWarehouseInformationDto"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateWarehouseInformationDto updateWarehouseInformationDto )
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(new UpdateWarehouseInformationCommand
            {
                Id = updateWarehouseInformationDto.Id,
                ProductId = updateWarehouseInformationDto.ProductId,
                Count = updateWarehouseInformationDto.Count,
                ReadyForSale = updateWarehouseInformationDto.ReadyForSale
            }));
        }
        /// <summary>
        /// Delete WarehouseInformation.
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
            return GetResponseOnlyResultMessage(await Mediator.Send(new DeleteWarehouseInformationCommand { Id = id }));
        }

        /// <summary>
        /// It brings the details according to its id.
        /// </summary>
        /// <remarks>bla bla bla </remarks>
        /// <return>WarehouseInformation List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WarehouseInformationDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetWarehouseInformationQuery { Id = id }));
        }
    }
}
