namespace CustomerLibrary.BusinessEntities
{
    public class Customer
    {
        public int CustomerId { get; set; } = 0;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal? TotalPurchasesAmount { get; set; } = null;
    }
}
