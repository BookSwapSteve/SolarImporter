using System.Collections.Generic;

namespace SolarImporter.Models
{
    /// <summary>
    /// Daily energy production statistics
    /// </summary>
    /// <remarks>
    /// Lower case properties to match json.
    /// </remarks>
    public class ProductionStatistics
    {
        /// <summary>
        /// Energy produced per interval
        /// </summary>
        public List<int?> production { get; set; }

        /// <summary>
        /// unix start time
        /// </summary>
        public int start_time { get; set; }

        /// <summary>
        /// Interval between datapoints (i.e. 900s)
        /// </summary>
        public int interval_length { get; set; }
    }
}