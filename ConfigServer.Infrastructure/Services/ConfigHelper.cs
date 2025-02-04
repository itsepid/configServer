using System.Collections.Generic;
using ConfigServer.Domain.Entities;

namespace ConfigServer.Infrastructure.Services
{
    public class ConfigHelper
    {
        public Dictionary<string, object> BuildConfig(IEnumerable<ConfigEntry> entries)
        {
            var configDict = new Dictionary<string, object>();

            foreach (var entry in entries)
            {
                InsertIntoDictionary(configDict, entry.Key.Split(':'), entry.Value);
            }

            return configDict;
        }

        private void InsertIntoDictionary(Dictionary<string, object> dict, string[] keys, string value)
        {
            var current = dict;
            for (int i = 0; i < keys.Length; i++)
            {
                if (i == keys.Length - 1)
                {
                    // When at the last key, assign the value.
                    current[keys[i]] = value;
                }
                else
                {
                    // If the key doesn't exist, create a new nested dictionary.
                    if (!current.ContainsKey(keys[i]))
                        current[keys[i]] = new Dictionary<string, object>();

                    current = current[keys[i]] as Dictionary<string, object>;
                }
            }
        }
    }
}
