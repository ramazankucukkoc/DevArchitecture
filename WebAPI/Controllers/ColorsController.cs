using Business.Handlers.Colors.Commands;
using Business.Handlers.Colors.Queries;
using Business.Handlers.Customers.Commands;
using Business.Handlers.OperationClaims.Queries;
using Core.Entities.Dtos;
using Entities.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ColorsController : BaseApiController
    {
        /// <summary>
        /// Add Color.
        /// </summary>
        /// <param name="createColorCommand"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateColorCommand createColorCommand)
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(createColorCommand));
        }
        /// <summary>
        /// List Colors
        /// </summary>
        /// <remarks>bla bla bla Colors</remarks>
        /// <return>Colors List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ColorDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetColorsQuery()));
        }
        /// <summary>
        /// Update Color.
        /// </summary>
        /// <param name="updateCustomerDto"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateColorDto updateColorDto )
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(new UpdateColorCommand { Id = updateColorDto.Id,Name = updateColorDto.Name}));
        }
        /// <summary>
        /// Delete Color.
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
            return GetResponseOnlyResultMessage(await Mediator.Send(new DeleteColorCommand { Id = id }));
        }
        /// <summary>
        /// It brings the details according to its id.
        /// </summary>
        /// <remarks>bla bla bla </remarks>
        /// <return>Color List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ColorDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetColorQuery { Id = id }));
        }
        /// <summary>
        /// List Colors
        /// </summary>
        /// <remarks>bla bla bla Colors</remarks>
        /// <return>Colors List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SelectionItem>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("lookups")]
        public async Task<IActionResult> GetColorLookup()
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetColorLookupQuery()));
        }

    }
}
