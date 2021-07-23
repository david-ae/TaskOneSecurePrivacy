using System.Collections.Generic;
using Task1.Dtos;
using Task1.Models;

namespace Task1
{
    public static class Extensions
    {
        public static TaxPayerDto AsDto(this TaxPayer taxPayer)
        {
            return new TaxPayerDto
            {
                Id = taxPayer.Id,
                Name = taxPayer.Name,
                Address = taxPayer.Address,
                Country = taxPayer.Country,
                Receipts = taxPayer.Receipts,
                CreatedDate = taxPayer.CreatedDate,
                Email = taxPayer.Email,
                Occupation = taxPayer.Occupation
            };
        }

        public static List<TaxPayerDto> AsDto(this List<TaxPayer> taxPayers)
        {
            List<TaxPayerDto> newTaxPayers = new List<TaxPayerDto>();

            foreach (var taxPayer in taxPayers)
            {
                TaxPayerDto newTaxPayer = new()
                {
                    Id = taxPayer.Id,
                    Name = taxPayer.Name,
                    Address = taxPayer.Address,
                    Country = taxPayer.Country,
                    Receipts = taxPayer.Receipts,
                    CreatedDate = taxPayer.CreatedDate,
                    Email = taxPayer.Email,
                    Occupation = taxPayer.Occupation
                };
                newTaxPayers.Add(newTaxPayer);
            }
            return newTaxPayers;
        }
    }
}