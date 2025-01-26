namespace ConfigServer.Domain.Entities
{
    public class Config
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Primary key
        public string Key { get; set; } // Configuration key
        public string Value { get; set; } // Configuration value
        public string Description { get; set; } // Optional description
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}