using System.Collections.Generic;

namespace SolarImporter.Models
{
    /// <summary>
    /// Serialization class for the Enphase daily energy data feed
    /// </summary>
    /// <remarks>
    /// Lower case properties to match json.
    /// </remarks>
    public class DailyEnergyData
    {
        public int system_id { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }

        /// <summary>
        /// Daily statsistics.
        /// Data is for each day from the start date to end date (typically last day of the month).
        /// </summary>
        public List<ProductionStatistics> stats { get; set; }
    }
}