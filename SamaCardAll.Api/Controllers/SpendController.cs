using Microsoft.AspNetCore.Mvc;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.VO;

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
                var spends = _spendService.GetSpendsAsync();
                return Ok(spends);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Create(SpendVO spend)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _spendService.CreateAsync(spend);
                    int spendId = spend.IdSpend;
                    return CreatedAtAction(nameof(GetById), new { id = spend.IdSpend }, spend);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SpendVO spend)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _spendService.UpdateAsync(spend);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _spendService.DeleteAsync(id);
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
            var spend = _spendService.GetByIdAsync(id);
            if (spend == null)
            {
                return NotFound();
            }
            return Ok(spend);
        }
    }
}
