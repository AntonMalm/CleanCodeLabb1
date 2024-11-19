namespace WebShop.Entities
{
    public class Customer
    {
        public int Id { get; set; } // Unikt ID för kunden
        public string Name { get; set; } // Namn på kunden
        public string Email { get; set; } // E-postadress för kunden
        public string Country { get; set; } // Land för kunden
    }
}
