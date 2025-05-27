using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamaCardAll.Core.Interfaces;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var cards = _cardService.GetCardsAsync();
                return Ok(cards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("active", Name = "GetActiveCards")]
        public IActionResult GetActive()
        {
            try
            {
                var cards = _cardService.GetActiveCardsAsync();
                return Ok(cards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server erro: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var card = _cardService.GetByIdAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            return Ok(card);
        }

        [HttpPost]
        public IActionResult Create(Card card)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _cardService.CreateAsync(card);
                    return CreatedAtAction(nameof(GetById), new { id = card.IdCard }, card);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Card card)
        {
            if (id != card.IdCard)
            {
                return BadRequest("ID mismatch");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _cardService.UpdateAsync(card);
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
                _cardService.DeleteAsync(id);
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
}
