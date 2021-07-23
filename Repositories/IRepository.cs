using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task1.Models;

namespace Task1.Repositories
{
    public interface IRepository
    {
        Task<TaxPayer> GetTaxPayerAsync(Guid id);
        Task<IEnumerable<TaxPayer>> GetTaxPayersAsync();
        Task CreateTaxPayerAsync(TaxPayer taxPayer);
        Task UpdateTaxPayerAsync(TaxPayer taxPayer);
        Task DeleteTaxPayerAsync(Guid id);
        Task CreateManyTaxPayersAsync(List<TaxPayer> taxPayers);
        Task<IEnumerable<TaxPayer>> GetTaxPayersLivingInACityAsync(string city);
        Task<IEnumerable<TaxPayer>> GetTaxPayersWithSpecificReceipts(string taxType);
        Task<IEnumerable<dynamic>> GetNumberOfTaxPayersByCountry();
        Task<IEnumerable<dynamic>> GetTaxPayersWithSpecificValidReceipts(string taxType);
    }
}