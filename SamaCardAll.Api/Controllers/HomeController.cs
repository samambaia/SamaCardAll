using Microsoft.AspNetCore.Mvc;
using SamaCardAll.Core.DTO;
using SamaCardAll.Core.Services;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IReportService _reportService;

        public HomeController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /*
         * TO DO:Handle Error
         */

        [HttpGet("{customerId}/{monthYear}")]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInstallments(int? customerId, string? monthYear)
        {
            try
            {
                var filteredInstallments = await _reportService.GetFilteredInstallments(customerId, monthYear);

                var invoiceDtos = filteredInstallments.Select(i => new InvoiceDto
                {
                    DescriptionSpend = i.DescriptionSpend,
                    CustomerName = i.CustomerName,
                    CardName = i.CardName,
                    InstallmentAmount = i.InstallmentAmount,
                    MonthYear = i.MonthYear,
                    Installment = i.Installment
                }).ToList();

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
        public async Task<ActionResult<IEnumerable<string>>> GetDistinctMonthYears()
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
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetTotalCustomerPerMonth(string monthYear)
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
        public async Task<ActionResult<IEnumerable<TotalCardMonthYearDTO>>> GetTotalCardPerMonth(string monthYear)
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
        public async Task<ActionResult<decimal>> SummarizeSpends(string monthYear)
        {
            try
            {
                var totalSpends = await _reportService.SummarizeSpends(monthYear);
                return Ok(totalSpends);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
