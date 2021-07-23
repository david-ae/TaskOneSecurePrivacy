using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Task1.Dtos;
using Task1.Models;
using Task1.Repositories;

namespace Task1.Controllers
{
    [ApiController]
    [Route("taxPayers")]
    public class TaxPayerController : ControllerBase
    {
        private readonly IRepository repository;
        public TaxPayerController(IRepository repository)
        {
            this.repository = repository;
        }

        // GET /taxPayers

        [HttpGet]
        public async Task<IEnumerable<TaxPayerDto>> GetTaxPayersAsync()
        {
            var taxPayers = (await repository.GetTaxPayersAsync())
                            .Select(taxPayer => taxPayer.AsDto());

            return taxPayers;
        }

        // GET /taxPayers/GetTaxPayersInACity/{city}

        [HttpGet("GetTaxPayersInACity/{city}")]
        public async Task<IEnumerable<TaxPayerDto>> GetTaxPayersInACityAsync(string city)
        {
            var taxPayers = (await repository.GetTaxPayersLivingInACityAsync(city))
                            .Select(taxPayer => taxPayer.AsDto());
            return taxPayers;
        }

        // GET /taxPayers/GetNumberOfTaxPayersByState

        [HttpGet("GetNumberOfTaxPayersByCountry")]
        public async Task<IEnumerable<dynamic>> GetNumberOfTaxPayersByCountry()
        {
            var taxPayers = await repository.GetNumberOfTaxPayersByCountry();
            return taxPayers;
        }

        // GET /taxPayers/GetTaxPayersWithSpecificReceipts/{taxType}

        [HttpGet("GetTaxPayersWithSpecificReceipts/{taxType}")]
        public async Task<IEnumerable<TaxPayerDto>> GetTaxPayersWithSpecificReceipts(string taxType)
        {
            var taxPayers = await repository.GetTaxPayersWithSpecificReceipts(taxType);
            return taxPayers.ToList().AsDto();
        }

        // GET /taxPayers/GetTaxPayersWithSpecificValidReceipts/{taxType}

        [HttpGet("GetTaxPayersWithSpecificValidReceipts/{taxType}")]
        public async Task<IEnumerable<dynamic>> GetTaxPayersWithSpecificValidReceipts(string taxType)
        {
            var taxPayers = await repository.GetTaxPayersWithSpecificValidReceipts(taxType);
            return taxPayers.ToList();
        }

        // GET /taxPayers/{id}

        [HttpGet("{id}")]
        public async Task<ActionResult<TaxPayerDto>> GetTaxPayerAsync(Guid id)
        {
            var taxPayer = await repository.GetTaxPayerAsync(id);

            if (taxPayer is null)
            {
                return NotFound();
            }

            return taxPayer.AsDto();
        }

        // POST /taxPayers/
        [HttpPost]
        public async Task<ActionResult<TaxPayerDto>> CreateTaxPayerAsync([FromBody] CreateTaxPayerDto taxPayerDto)
        {
            TaxPayer taxPayer = new()
            {
                Id = Guid.NewGuid(),
                Name = taxPayerDto.Name,
                Address = taxPayerDto.Address,
                Country = taxPayerDto.Country,
                Receipts = taxPayerDto.Receipts,
                CreatedDate = DateTimeOffset.UtcNow,
                Email = taxPayerDto.Email,
                Occupation = taxPayerDto.Occupation
            };

            await repository.CreateTaxPayerAsync(taxPayer);

            return CreatedAtAction(nameof(GetTaxPayerAsync), new { id = taxPayer.Id }, taxPayer.AsDto());
        }

        // PUT /taxPayers/{id}

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTaxPayerAsync(Guid id, [FromBody] UpdateTaxPayerDto taxPayerDto)
        {
            var existingTaxPayer = await repository.GetTaxPayerAsync(id);

            if (existingTaxPayer is null)
            {
                return NotFound();
            }

            TaxPayer updatedTaxPayer = existingTaxPayer with
            {
                Name = taxPayerDto.Name,
                Address = taxPayerDto.Address,
                Country = taxPayerDto.Country,
                Receipts = taxPayerDto.Receipts,
                Email = taxPayerDto.Email,
                Occupation = taxPayerDto.Occupation
            };

            await repository.UpdateTaxPayerAsync(updatedTaxPayer);

            return NoContent();
        }

        // DELETE /taxPayers/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTaxPayerAsync(Guid id)
        {
            var existingTaxPayer = await repository.GetTaxPayerAsync(id);

            if (existingTaxPayer is null)
            {
                return NotFound();
            }

            await repository.DeleteTaxPayerAsync(id);

            return NoContent();
        }

        // POST /taxPayers/CreateManyTaxPayers
        [HttpPost("CreateManyTaxPayers")]
        public async Task<ActionResult> CreateTaxPayersAsync(List<TaxPayerDto> taxPayerDtos)
        {
            List<TaxPayer> newTaxPayers = new List<TaxPayer>();

            foreach (var taxPayerDto in taxPayerDtos)
            {
                TaxPayer taxPayer = new()
                {
                    Id = Guid.NewGuid(),
                    Name = taxPayerDto.Name,
                    Address = taxPayerDto.Address,
                    Country = taxPayerDto.Country,
                    Receipts = taxPayerDto.Receipts,
                    CreatedDate = DateTimeOffset.UtcNow,
                    Email = taxPayerDto.Email,
                    Occupation = taxPayerDto.Occupation
                };
                newTaxPayers.Add(taxPayer);
            }

            await repository.CreateManyTaxPayersAsync(newTaxPayers);

            return CreatedAtAction(nameof(GetTaxPayersAsync), newTaxPayers.AsDto());
        }
    }
}