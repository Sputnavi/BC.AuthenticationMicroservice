namespace BC.AuthenticationMicroservice.Boundary.Response
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public double MinutesToExpire { get; set; }
        public string Role { get; set; }
    }
}
