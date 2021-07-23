namespace Task1.Models.ValueObjects
{
    public record AddressVO
    {
        public string Street { get; init; }
        public string City { get; init; }
        public string State { get; init; }
    }
}