namespace BC.AuthenticationMicroservice.Boundary.Response
{
    public class UserWithRole
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
