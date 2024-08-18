namespace PrimeSeats_Model.DTO
{
    public class Response
    {
        public string? Message { get; set; }
        public bool ?Success { get; set; }
        public string? Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
