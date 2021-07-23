using System;

namespace Task1.Models.ValueObjects
{
    public record ReceiptVO
    {
        public string TaxType { get; init; }
        public DateTimeOffset Expires { get; init; }
    }
}