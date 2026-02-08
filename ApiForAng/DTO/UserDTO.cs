namespace ApiForAng.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public String Address { get; set; }
        public string City { get; set; }
        public string ?Number { get; set; }
    }
}
