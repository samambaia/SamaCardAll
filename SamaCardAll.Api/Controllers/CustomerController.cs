using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Services;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                IEnumerable<Customer> customers = _customerService.GetCustomers();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var customer = _customerService.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _customerService.Create(customer);
                    return CreatedAtAction(nameof(GetById), new { id = customer.IdCustomer }, customer);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Customer customer)
        {
            if (id != customer.IdCustomer)
            {
                return BadRequest("ID mismatch");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _customerService.Update(customer);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _customerService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException)
                {
                    return BadRequest($"Could not delete {ex.InnerException}");
                }
                else
                    return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class ForeignKeyException : Exception
    {
        public ForeignKeyException(string message) : base(message)
        {
        }
    }
}