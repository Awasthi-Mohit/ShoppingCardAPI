namespace ApiForAng.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Cartitems> CartItems { get; set; } = new List<Cartitems>();
    }
}
