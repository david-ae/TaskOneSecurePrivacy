using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Task1.Models.ValueObjects;

namespace Task1.Dtos
{
    public record CreateTaxPayerDto
    {
        [Required]
        public string Name { get; init; }
        [Required]
        public AddressVO Address { get; init; }
        [Required]
        public string Email { get; init; }
        public string Occupation { get; init; }
        [Required]
        public string Country { get; init; }
        public IEnumerable<ReceiptVO> Receipts { get; init; }

        public CreateTaxPayerDto()
        {
            Receipts = new List<ReceiptVO>();
        }
    }
}