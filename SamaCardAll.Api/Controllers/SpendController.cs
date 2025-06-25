using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Core.Models;
using SamaCardAll.Shared.Contracts.ViewModels;

namespace SamaCardAll.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpendController : ControllerBase
    {
        private readonly ISpendService _spendService;
        private readonly IMapper _mapper;

        public SpendController(ISpendService spendService, IMapper mapper)
        {
            _spendService = spendService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var spends = await _spendService.GetSpendsAsync();

                var result = _mapper.Map<List<SpendViewModel>>(spends);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null
                    ? $"Internal server error: {ex.Message} | Inner exception: {ex.InnerException.Message}"
                    : $"Internal server error: {ex.Message}";

                return StatusCode(500, errorMessage);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(SpendViewModel spendVM)
        {
            var spendModel = _mapper.Map<Spend>(spendVM);

            if (ModelState.IsValid)
            {
                try
                {
                    await _spendService.CreateAsync(spendModel);
                    int spendId = spendModel.IdSpend;

                    return CreatedAtAction(nameof(GetById), new { id = spendModel.IdSpend }, spendModel);
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

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SpendViewModel spendVM)
        {
            var spendModel = _mapper.Map<Spend>(spendVM);

            if (ModelState.IsValid)
            {
                try
                {
                    await _spendService.UpdateAsync(spendModel);
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

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _spendService.DeleteAsync(id);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var spend = await _spendService.GetByIdAsync(id);
            if (spend == null)
            {
                return NotFound();
            }
            return Ok(spend);
        }
    }
}
