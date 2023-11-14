using Business.Handlers.Customers.Commands;
using Business.Handlers.Customers.Queries;
using Business.Handlers.Products.Commands;
using Business.Handlers.Products.Queries;
using Business.Handlers.UserClaims.Commands;
using Business.Handlers.UserClaims.Queries;
using Core.Entities.Dtos;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductsController : BaseApiController
    {
        /// <summary>
        /// Add Product.
        /// </summary>
        /// <param name="createProductCommand"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateProductCommand createProductCommand)
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(createProductCommand));
        }

        /// <summary>
        /// List Products
        /// </summary>
        /// <remarks>bla bla bla Products</remarks>
        /// <return>Products List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetProductsQuery()));
        }
        /// <summary>
        /// Update Product.
        /// </summary>
        /// <param name="updateCustomerDto"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProductDto updateProductDto)
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(new UpdateProductCommand { Id = updateProductDto.Id, Name = updateProductDto.Name, ColorId = updateProductDto.ColorId, Size = updateProductDto.Size }));
        }
        /// <summary>
        /// Delete Product.
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
            return GetResponseOnlyResultMessage(await Mediator.Send(new DeleteProductCommand { Id = id }));
        }
        /// <summary>
        /// It brings the details according to its id.
        /// </summary>
        /// <remarks>bla bla bla </remarks>
        /// <return>Products List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetProductQuery { Id = id }));
        }
        /// <summary>
        /// It brings the details according to its id.
        /// </summary>
        /// <remarks>bla bla bla </remarks>
        /// <return>Color List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SelectionItem>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("colors/{id}")]
        public async Task<IActionResult> GetColorByProductId([FromRoute] int id)
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetProductLookupByColorQuery { Id = id }));
        }
        /// <summary>
        /// Update Color.
        /// </summary>
        /// <param name="updateProductColorDto"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut("colors/")]
        public async Task<IActionResult> Update([FromBody] UpdateProductColorDto updateProductColorDto )
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(new UpdateProductColorCommand { ProductId = updateProductColorDto.Id, ColorId = updateProductColorDto.ColorIds }));
        }
    }
}
