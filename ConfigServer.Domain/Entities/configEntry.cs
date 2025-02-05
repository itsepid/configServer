namespace ConfigServer.Domain.Entities
{
    public class ConfigEntry
    {
        public Guid Id { get; private set; } 
        public Guid ConfigProjectId { get; private set; } 
        public string Key { get;  set; } 
        public string Value { get;  set; } 
        public string Environment { get; set;}

        public ConfigProject ConfigProject { get; private set; } 

        private ConfigEntry() { } 

        public ConfigEntry(Guid configProjectId, string key, string value, string environment)
        {
            Id = Guid.NewGuid();
            ConfigProjectId = configProjectId;
            Key = key;
            Value = value;
            Environment = environment;

        }

        public void UpdateValue(string value)
        {
            Value = value;
        }
    }
}
