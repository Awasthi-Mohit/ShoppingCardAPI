using System.Globalization;

namespace ApiForAng.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public String Address { get; set; }
        public string City { get; set; }
        public string Contect {  get; set; }
    }
}
