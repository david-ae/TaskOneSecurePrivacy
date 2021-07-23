using Task1.Models.ValueObjects;

namespace Task1.Dtos
{
    public record ProjectTaxPayerDto
    {
        public string Name { get; init; }
        public AddressVO Address { get; init; }
    }
}