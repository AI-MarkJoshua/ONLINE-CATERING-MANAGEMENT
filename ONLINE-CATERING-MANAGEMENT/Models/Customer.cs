namespace ONLINE_CATERING_MANAGEMENT.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }

        public string CustomerFName { get; set; }
        public string CustomerMName { get; set; }
        public string CustomerLName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }

        public string Address { get; set; }
        public string HouseNo { get; set; }
        public string Street { get; set; }
        public string Barangay { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
    }
}