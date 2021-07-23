using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Task1.Models;

namespace Task1
{
    public class DatabaseIndexService : IHostedService
    {
        private readonly IMongoClient _mongoClient;
        private const string databaseName = "Task1";
        private const string collectionName = "taxPayers";
        private IMongoCollection<TaxPayer> _taxPayersCollection;

        public DatabaseIndexService(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var database = _mongoClient.GetDatabase(databaseName);
            _taxPayersCollection = database.GetCollection<TaxPayer>(collectionName);

            var indexe1 = Builders<TaxPayer>.IndexKeys.Text(t => t.Address.City);
            var indexe2 = Builders<TaxPayer>.IndexKeys.Text(t => t.Country);
            var indexe3 = Builders<TaxPayer>.IndexKeys.Descending(t => t.CreatedDate);
            await _taxPayersCollection.Indexes.CreateOneAsync(new CreateIndexModel<TaxPayer>(indexe1),
                    cancellationToken: cancellationToken);
            await _taxPayersCollection.Indexes.CreateOneAsync(new CreateIndexModel<TaxPayer>(indexe2),
                    cancellationToken: cancellationToken);
            await _taxPayersCollection.Indexes.CreateOneAsync(new CreateIndexModel<TaxPayer>(indexe3),
                    cancellationToken: cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}