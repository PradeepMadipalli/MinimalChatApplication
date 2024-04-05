namespace MinimalChatApplication.Model
{
    public class TokenResponse
    {
       public string? Token { get; set; }
        public Profile? Profiles { get; set; }
    }

    public class Profile
    {
        public string? UId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }

}
