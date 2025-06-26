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
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IMapper _mapper;

        public CardController(ICardService cardService,IMapper mapper)
        {
            _cardService = cardService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var cards = await _cardService.GetCardsAsync();
                return Ok(cards);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null
                    ? $"Internal server error: {ex.Message} | Inner exception: {ex.InnerException.Message}"
                    : $"Internal server error: {ex.Message}";

                return StatusCode(500, errorMessage);
            }
        }

        [HttpGet("active", Name = "GetActiveCards")]
        public async Task<IActionResult> GetActive()
        {
            try
            {
                var cards = await _cardService.GetActiveCardsAsync();
                return Ok(cards);
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
            var card = await _cardService.GetByIdAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            return Ok(card);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CardViewModel cardVM)
        {
            var cardModel = _mapper.Map<Card>(cardVM);

            if (ModelState.IsValid)
            {
                try
                {
                    await _cardService.CreateAsync(cardModel);
                    return CreatedAtAction(nameof(GetById), new { id = cardModel.IdCard }, cardModel);
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
        public async Task<IActionResult> Update(int id, [FromBody] CardViewModel cardVM)
        {
            if (id != cardVM.IdCard)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cardModel = _mapper.Map<Card>(cardVM);

            try
            {
                await _cardService.UpdateAsync(cardModel);
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
                await _cardService.DeleteAsync(id);
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
