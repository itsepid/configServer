namespace ConfigServer.Domain.Entities
{
    public class Config
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public string Key { get; set; } 
        public string Value { get; set; } 
        public string Description { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int? UserId { get; set; }
        public string ProjectId { get; set;}
        public string FilePath { get; set; } 
        public string FileUrl { get; set; } 

    }
}   