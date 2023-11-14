using Business.Handlers.Customers.Commands;
using Business.Handlers.Customers.Queries;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CustomersController : BaseApiController
    {
        /// <summary>
        /// Add Customer.
        /// </summary>
        /// <param name="createCustomerCommand"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCustomerCommand createCustomerCommand )
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(createCustomerCommand));
        }
        /// <summary>
        /// List Customers
        /// </summary>
        /// <remarks>bla bla bla Customers</remarks>
        /// <return>Customers List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CustomerDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetCustomersQuery()));
        }
        /// <summary>
        /// Update Customer.
        /// </summary>
        /// <param name="updateCustomerDto"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerDto updateCustomerDto )
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(new UpdateCustomerCommand { Id = updateCustomerDto.Id, Email = updateCustomerDto.Email, Name = updateCustomerDto.Name, MobilePhones = updateCustomerDto.MobilePhones, Address = updateCustomerDto.Address, Code = updateCustomerDto.Code }));
        }
        /// <summary>
        /// Delete Customer.
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
            return GetResponseOnlyResultMessage(await Mediator.Send(new DeleteCustomerCommand { Id = id }));
        }
        /// <summary>
        /// It brings the details according to its id.
        /// </summary>
        /// <remarks>bla bla bla </remarks>
        /// <return>Customers List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetCustomerQuery { Id = id }));
        }
    }
}
