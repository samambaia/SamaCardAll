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

        //[HttpGet]
        //public IActionResult Index()
        //{
        //    return Ok("Success!");
        //}

        /*
         * TO DO:Handle Error
         */

        [HttpGet("{customerId}/{monthYear}")]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInstallments(int? customerId, string? monthYear)
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

        [HttpPut]
        public ActionResult UpdateInstallments()
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

        [HttpGet("distinct-monthYears")]
        public async Task<IEnumerable<string>> GetDistinctMonthYears()
        {
            return await _reportService.GetDistinctInstallmentMonthYear();
        }

        [HttpGet("customer/{monthYear}", Name = "TotalCustomerPerMonth")]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetTotalCustomerPerMonth(string monthYear)
        {
            var customerTotals = await _reportService.GetTotalCustomerPerMonth(monthYear);
            return Ok(customerTotals);
        }

        [HttpGet("card/{monthYear}", Name = "TotalCardPerMonth")]
        public async Task<ActionResult<IEnumerable<TotalCardMonthYearDTO>>> GetTotalCardPerMonth(string monthYear)
        {
            var cardTotals = await _reportService.GetTotalCardMonthYear(monthYear);
            return Ok(cardTotals);
        }
    }
}
