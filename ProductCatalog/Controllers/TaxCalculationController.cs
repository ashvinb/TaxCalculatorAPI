using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using ProductCatalog;
using BusinessLogic;

namespace ProductCatalogue.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxCalculationController : ControllerBase
    {
        private readonly ITaxCalculator _taxCalculator;
        private readonly ILogger<TaxCalculationController> _logger;

        public TaxCalculationController(ILogger<TaxCalculationController> logger, ITaxCalculator taxCalculator)
        {
            _logger = logger;
            _taxCalculator = taxCalculator;
        }

        [HttpGet(Name = "getTaxAmount")]
        public async Task<IActionResult> Get(int annualIncome, string postcode)
        {
            if (annualIncome < 0)
            {
                return BadRequest("Annual income must be a positive value.");
            }

            if (string.IsNullOrWhiteSpace(postcode))
            {
                return BadRequest("Postcode is required.");
            }

            try
            {
                double taxAmount = await _taxCalculator.CalculateTax(annualIncome, postcode);
                return Ok(taxAmount);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while calculating tax: {ex.Message}");
                return StatusCode(500, $"An error occurred while calculating tax: {ex.Message}");
            }
        }

        

    }

}