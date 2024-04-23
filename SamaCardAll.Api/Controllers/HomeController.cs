﻿using Microsoft.AspNetCore.Mvc;
using SamaCardAll.Core.DTO;
using SamaCardAll.Core.Services;

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
                MonthYear = i.MonthYear
            }).ToList();

            return Ok(invoiceDtos);
        }
    }
}
