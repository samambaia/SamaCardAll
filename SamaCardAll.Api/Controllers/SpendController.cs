using Microsoft.AspNetCore.Mvc;
using SamaCardAll.Core.Models;
using SamaCardAll.Core.Services;
using System;
using System.Collections.Generic;

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

        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("Ping successful! The API is up and running.");
        }
    }
}
