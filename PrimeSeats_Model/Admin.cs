namespace PrimeSeats_Model
{
    public class Admin
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string  FullName { get; set; }
        public string FullName
        {
            get => $"{FirstName} {LastName}";
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    FirstName = string.Empty;
                    LastName = string.Empty;
                }
                else
                {
                    var names = value.Split(' ');
                    if (names.Length >= 2)
                    {
                        FirstName = names[0];
                        LastName = string.Join(' ', names.Skip(1));
                    }
                    else
                    {
                        FirstName = names[0];
                        LastName = string.Empty;
                    }
                }
            }
        }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public int PhoneNumber { get; set; }
        public string Address { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
