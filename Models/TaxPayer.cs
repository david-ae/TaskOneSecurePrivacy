using System;
using System.Collections.Generic;
using Task1.Models.ValueObjects;

namespace Task1.Models
{
    public record TaxPayer
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public AddressVO Address { get; init; }
        public string Email { get; init; }
        public string Occupation { get; init; }
        public string Country { get; init; }
        public IEnumerable<ReceiptVO> Receipts { get; init; }
        public DateTimeOffset CreatedDate { get; init; }

        public TaxPayer()
        {
            Receipts = new List<ReceiptVO>();
        }
    }
}