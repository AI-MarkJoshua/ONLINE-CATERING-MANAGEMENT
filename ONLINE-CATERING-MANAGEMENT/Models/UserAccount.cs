namespace ONLINE_CATERING_MANAGEMENT.Models
{
    public class UserAccount
    {
        public int UserAccountID { get; set; }
        public string Email { get; set; } 
        public string PasswordHash { get; set; }
        public string Role { get; set; } // e.g., "Customer", "Admin", "Staff"

        public bool IsVerified { get; set; } = false; // to block fake reservation

        //Relationship to customer profile
        public int? CustomerID { get; set; }
        public Customer? Customer { get; set; }
    }
}
