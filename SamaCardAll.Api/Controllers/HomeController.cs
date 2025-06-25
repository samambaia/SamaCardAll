using Microsoft.AspNetCore.Mvc;
using SamaCardAll.Shared.Contracts.Report;
using SamaCardAll.Core.Interfaces;

namespace SamaCardAll.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IReportService reportService, ILogger<HomeController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        /*
         * TODO:Handle Error
         */

        [HttpGet("{customerId}/{monthYear}")]
        public async Task<ActionResult<List<InvoiceDTO>>> GetInstallments(int? customerId, string monthYear)
        {
            try
            {
                var filteredInstallments = await _reportService.GetFilteredInstallments(customerId, monthYear);

                var invoiceDtos = filteredInstallments.Select(i => new InvoiceDTO(i.DescriptionSpend, i.CustomerName, i.CardName, i.InstallmentAmount, i.MonthYear, i.Installment));

                return Ok(invoiceDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public ActionResult UpdateInstallments()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _reportService.UpdateInstallments();
                        return NoContent();
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"Internal server error: {ex.Message}");
                    }
                }
                return BadRequest(ModelState);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("distinct-monthYears")]
        public async Task<ActionResult<List<string>>> GetDistinctMonthYears()
        {
            try
            {
                var distinctMonthYears = await _reportService.GetDistinctInstallmentMonthYear();
                return Ok(distinctMonthYears);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("customer/{monthYear}", Name = "TotalCustomerPerMonth")]
        public async Task<ActionResult<List<InvoiceDTO>>> GetTotalCustomerPerMonth(string monthYear)
        {
            try
            {
                var customerTotals = await _reportService.GetTotalCustomerPerMonth(monthYear);
                return Ok(customerTotals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("card/{monthYear}", Name = "TotalCardPerMonth")]
        public async Task<ActionResult<List<TotalCardMonthYearDTO>>> GetTotalCardPerMonth(string monthYear)
        {
            try
            {
                var cardTotals = await _reportService.GetTotalCardMonthYear(monthYear);
                return Ok(cardTotals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("summarize/{monthYear}")]
        public async Task<ActionResult<float>> SummarizeSpends(string monthYear)
        {
            _logger.LogInformation("SummarizeSpends called with monthYear: {monthYear}", monthYear);

            try
            {
                var totalSpends = await _reportService.SummarizeSpends(monthYear);

                // Convert the totalSpends to double or float
                var totalSpendsDouble = totalSpends.Select(s => (double)s).Sum();

                // Convert the result back to decimal
                var totalSpendsDecimal = (decimal)totalSpendsDouble;

                return Ok(totalSpendsDecimal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in SummarizeSpends for {monthYear}", monthYear);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("detailed-card/{cardId}/{monthYear}")]
        public async Task<ActionResult<List<DetailedCardDTO>>> DetailedCard(int? cardId, string monthYear)
        {
            _logger.LogInformation("DetailedCard called with cardId: {cardId} and monthYear: {monthYear}", cardId, monthYear);

            try
            {
                var detailedCards = await _reportService.DetailedCard(cardId, monthYear);
                return Ok(detailedCards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
