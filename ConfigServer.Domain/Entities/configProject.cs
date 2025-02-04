using System;
using System.Security.Cryptography;
using System.Text;

namespace ConfigServer.Domain.Entities
{
    public class ConfigProject
    {
        public Guid Id { get; private set; } 
        public string ProjectName { get; private set; } 
        public string Environment { get; private set;}
        public string PasskeyHash { get; private set; } 
        public DateTime LastUpdated { get; private set; }



        private ConfigProject() { } 

        public ConfigProject(string projectName, string passkey, string environment)
        {
            Id = Guid.NewGuid();
            ProjectName = projectName;
            Environment = environment;
            PasskeyHash = HashPasskey(passkey);
            LastUpdated = DateTime.UtcNow;
        }

        public bool VerifyPasskey(string passkey)
        {
            return PasskeyHash == HashPasskey(passkey);
        }

        
        private string HashPasskey(string passkey)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(passkey);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
