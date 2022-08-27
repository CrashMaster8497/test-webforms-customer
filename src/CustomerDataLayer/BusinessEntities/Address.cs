namespace CustomerLibrary.BusinessEntities
{
    public class Address
    {
        public int AddressId { get; set; } = 0;
        public int CustomerId { get; set; } = 0;
        public string AddressLine { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public AddressType AddressType { get; set; } = AddressType.Unknown;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }

    public enum AddressType
    {
        Unknown,
        Shipping,
        Billing
    }
}
