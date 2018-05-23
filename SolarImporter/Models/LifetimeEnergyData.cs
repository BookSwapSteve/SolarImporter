using System.Collections.Generic;

namespace SolarImporter.Models
{
    /// <summary>
    /// Serialization class for the Enphase lifetime energy data feed
    /// </summary>
    /// <remarks>
    /// Lower case properties to match json.
    /// </remarks>
    public class LifetimeEnergyData
    {
        public int system_id { get; set; }
        public string start_date { get; set; }
        public List<int> production { get; set; }
    }
}