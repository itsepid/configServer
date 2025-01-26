namespace ConfigServer.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
       // public string Role { get; set;}

      
        public User(string username, string password)
        {
            Username = username;
            Password = password;
           // Role = role;
        }
    }
}
