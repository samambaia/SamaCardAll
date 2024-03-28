using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Services;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpendController : ControllerBase
    {
        private readonly ISpendService _spendService;

        public SpendController(ISpendService spendService)
        {
            _spendService = spendService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                IEnumerable<Spend> spends = _spendService.GetSpends();
                return Ok(spends);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpGet]
        //public IActionResult GetAll(string include = "")
        //{
        //    try
        //    {
        //        IEnumerable<Spend> spendsQuery = _spendService.GetSpends();

        //        // Check if include parameter is provided and include related entities accordingly
        //        if (!string.IsNullOrEmpty(include))
        //        {
        //            foreach (var navProperty in include.Split(',', StringSplitOptions.RemoveEmptyEntries))
        //            {
        //                spendsQuery = spendsQuery.Include(navProperty);
        //            }
        //        }

        //        // Execute the query and return the results
        //        IEnumerable<Spend> spends = spendsQuery.ToList();
        //        return Ok(spends);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}


        [HttpPost]
        public IActionResult Create(Spend spend)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _spendService.Create(spend);
                    return CreatedAtAction(nameof(GetById), new { id = spend.IdSpend }, spend);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{idSpend}")]
        public IActionResult Update(int idSpend, Spend spend)
        {
            if (idSpend != spend.IdSpend)
            {
                return BadRequest("ID mismatch");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _spendService.Update(spend);
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
                _spendService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var spend = _spendService.GetById(id);
            if (spend == null)
            {
                return NotFound();
            }
            return Ok(spend);
        }

        //[HttpGet("Ping")]
        //public IActionResult CheckStatus()
        //{
        //    return Ok("Ping successful! The API is up and running.");
        //}
    }
}
