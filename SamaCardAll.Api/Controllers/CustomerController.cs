using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.Models;
using SamaCardAll.Shared.Contracts.ViewModels;

namespace SamaCardAll.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var customers = await _customerService.GetCustomersAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null
                    ? $"Internal server error: {ex.Message} | Inner exception: {ex.InnerException.Message}"
                    : $"Internal server error: {ex.Message}";

                return StatusCode(500, errorMessage);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerViewModel customerVM)
        {
            var customerModel = _mapper.Map<Customer>(customerVM);

            if (ModelState.IsValid)
            {
                try
                {
                    await _customerService.CreateAsync(customerModel);
                    return CreatedAtAction(nameof(GetById), new { id = customerModel.IdCustomer }, customerModel);
                }
                catch (Exception ex)
                {
                    var errorMessage = ex.InnerException != null
                        ? $"Internal server error: {ex.Message} | Inner exception: {ex.InnerException.Message}"
                        : $"Internal server error: {ex.Message}";

                    return StatusCode(500, errorMessage);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerViewModel customerVM)
        {
            if (id != customerVM.IdCustomer)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerModel = _mapper.Map<Customer>(customerVM);

            try
            {
                await _customerService.UpdateAsync(customerModel);
                return NoContent();
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null
                    ? $"Internal server error: {ex.Message} | Inner exception: {ex.InnerException.Message}"
                    : $"Internal server error: {ex.Message}";

                return StatusCode(500, errorMessage);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _customerService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException)
                {
                    return BadRequest($"Could not delete {ex.InnerException}");
                }
                else
                {
                    var errorMessage = ex.InnerException != null
                        ? $"Internal server error: {ex.Message} | Inner exception: {ex.InnerException.Message}"
                        : $"Internal server error: {ex.Message}";

                    return StatusCode(500, errorMessage);
                }
            }
        }
    }
}