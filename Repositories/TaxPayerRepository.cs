using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Task1.Dtos;
using Task1.Models;

namespace Task1.Repositories
{
    public class TaxPayerRepository : IRepository
    {
        private const string databaseName = "Task1";
        private const string collectionName = "taxPayers";
        private readonly IMongoCollection<TaxPayer> _taxPayersCollection;
        private readonly FilterDefinitionBuilder<TaxPayer> filterBuilder = Builders<TaxPayer>.Filter;

        public TaxPayerRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            _taxPayersCollection = database.GetCollection<TaxPayer>(collectionName);
        }
        public async Task CreateTaxPayerAsync(TaxPayer taxPayer)
        {
            await _taxPayersCollection.InsertOneAsync(taxPayer);
        }

        public async Task CreateManyTaxPayersAsync(List<TaxPayer> taxPayers)
        {
            await _taxPayersCollection.InsertManyAsync(taxPayers);
        }

        public async Task DeleteTaxPayerAsync(Guid id)
        {
            var filter = filterBuilder.Eq(taxPayer => taxPayer.Id, id);
            await _taxPayersCollection.DeleteOneAsync(filter);
        }

        public async Task<TaxPayer> GetTaxPayerAsync(Guid id)
        {
            var filter = filterBuilder.Eq(taxPayer => taxPayer.Id, id);
            return await _taxPayersCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<TaxPayer>> GetTaxPayersAsync()
        {
            return await _taxPayersCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<IEnumerable<TaxPayer>> GetTaxPayersLivingInACityAsync(string city)
        {
            // builder sort criteria to be used in ordering matching data using the aggregation pipeline
            var sort = Builders<TaxPayer>.Sort.Descending(s => s.CreatedDate);

            return await _taxPayersCollection.Aggregate()
                            .Match<TaxPayer>(t => t.Address.City == city)
                            .Sort(sort)
                            .ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetNumberOfTaxPayersByCountry()
        {
            // get the number of taxpayers in respective countries
            var result = await _taxPayersCollection.Aggregate()
                                .Group(t => t.Country, x => new
                                {
                                    Country = x.First().Country,
                                    Count = x.Count()
                                }).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<TaxPayer>> GetTaxPayersWithSpecificReceipts(string taxType)
        {
            // this is a document array search using features like elemMatch 
            var filter = Builders<TaxPayer>.Filter
                        .ElemMatch(t => t.Receipts, t => t.TaxType == taxType);

            return await _taxPayersCollection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetTaxPayersWithSpecificValidReceipts(string taxType)
        {
            // get list of Tax Payers who still have valid specified receipts
            var sort = Builders<TaxPayer>.Sort.Descending(s => s.CreatedDate);
            var filter = Builders<TaxPayer>.Filter
                        .ElemMatch(t => t.Receipts, t => t.Expires > DateTimeOffset.UtcNow && t.TaxType == taxType);
            var projection = Builders<TaxPayer>.Projection
                            .Exclude(t => t.Id)
                            .Include(t => t.Name)
                            .Include(t => t.Address);

            return await _taxPayersCollection.Aggregate()
                            .Match(filter)
                            .Sort(sort)
                            .Project<ProjectTaxPayerDto>(projection)
                            .ToListAsync();
        }

        public async Task UpdateTaxPayerAsync(TaxPayer taxPayer)
        {
            var filter = filterBuilder.Eq(existingTaxPayer => existingTaxPayer.Id, taxPayer.Id);
            await _taxPayersCollection.ReplaceOneAsync(filter, taxPayer);
        }
    }
}