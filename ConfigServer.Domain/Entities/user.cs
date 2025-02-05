namespace ConfigServer.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Roles { get; set;}
      //  public ICollection<Config> Configs { get; set; } = new List<Config>();

      
        public User(string username, string password, string roles)
        {
            Username = username;
            Password = password;
            Roles = roles;
        }
    }
}
