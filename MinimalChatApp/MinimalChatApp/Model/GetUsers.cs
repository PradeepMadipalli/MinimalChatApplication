namespace MinimalChatApplication.Model
{
    public class GetUsers
    {
        public string? UserId { get; set; }
        public string? UserName { get; set;}
        public string? UserEmail { get; set;}
    }

    public class Userss
    {
        public List<GetUsers>? Userrs { get; set; }
    }
}
