namespace ConfigServer.Domain.Entities
{
    public class Config
    {
        public int Id { get; private set; } 
        public string Key { get; private set; } 
        public string Value { get; private set; } 
        public DateTime LastUpdated { get; private set; } 

        
        public Config(string key, string value)
        {
            Key = key;
            Value = value;
            LastUpdated = DateTime.UtcNow;
        }

       
        public void UpdateValue(string newValue)
        {
            Value = newValue;
            LastUpdated = DateTime.UtcNow;
        }
    }
}
