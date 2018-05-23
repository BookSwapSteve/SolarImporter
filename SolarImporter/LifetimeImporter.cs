using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using SolarImporter.Models;

namespace SolarImporter
{
    /// <summary>
    /// Import / Export lifetime energy data
    /// </summary>
    public static class LifetimeImporter
    {
        private static readonly HttpClient Client = new HttpClient();

        public static async Task ExportLifetimeEnergyAsync(string systemId)
        {
            LifetimeEnergyData lifetimeEnergyData = await GetLifetimeEnergyAsync(systemId);

            var energyByDay = ConvertToDateDictionary(lifetimeEnergyData);

            ExportByMonth(energyByDay);
            ExportByDay(energyByDay);
        }

        /// <summary>
        /// Convert the simple array to a date/energy dictionary
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private static Dictionary<DateTime, int> ConvertToDateDictionary(LifetimeEnergyData json)
        {
            Dictionary<DateTime, int> productionByDay = new Dictionary<DateTime, int>();

            // 2017-03-22
            DateTime date = DateTime.Parse(json.start_date);
            foreach (var power in json.production)
            {
                productionByDay.Add(date, power);
                date = date.AddDays(1);
            }
            return productionByDay;
        }

        /// <summary>
        /// Creates a csv file for power output on each day of the month.
        /// </summary>
        /// <param name="productionByDay"></param>
        private static void ExportByMonth(Dictionary<DateTime, int> productionByDay)
        {
            string filename = @".\LifetimeEnergyMonthly.csv";

            using (StreamWriter file = File.CreateText(filename))
            {
                // header for month column
                file.Write("Month,");

                // Header for day (1..31)
                for (int i = 0; i < 31; i++)
                {
                    // Pad out.
                    file.Write("{0},", i + 1);
                }

                DateTime start = productionByDay.First().Key;
                int month = start.Month;
                file.Write("\n\r");
                file.Write(start.ToString("MMMM"));
                file.Write(",");

                // If the start of the data isn't the 1st of the month
                // we need to pad it out.
                for (int i = 1; i < start.Day; i++)
                {
                    // Pad out.
                    file.Write(",");
                }

                // Now write the data for each day in all of the months.
                foreach (var keyValue in productionByDay)
                {
                    if (month != keyValue.Key.Month)
                    {
                        file.Write("\n\r");
                        file.Write(keyValue.Key.ToString("MMMM"));
                        file.Write(",");
                        month = keyValue.Key.Month;
                    }
                    file.Write("{0},", keyValue.Value);
                }
            }

            Console.WriteLine("Wrote lifetime energy by month to '{0}'", filename);
        }

        /// <summary>
        /// Creates a csv file for power output on each day of the month.
        /// </summary>
        /// <param name="productionByDay"></param>
        private static void ExportByDay(Dictionary<DateTime, int> productionByDay)
        {
            string filename = @".\LifetimeEnergyByDay.csv";

            using (StreamWriter file = File.CreateText(filename))
            {
                // Header
                file.WriteLine("Date,Energy (Wh)");

                foreach (var keyValue in productionByDay)
                {
                    file.WriteLine("{0},{1}", keyValue.Key, keyValue.Value);
                }
            }

            Console.WriteLine("Wrote lifetime energy by day to '{0}'", filename);
        }

        private static async Task<LifetimeEnergyData> GetLifetimeEnergyAsync(string id)
        {
            // See also... https://enlighten.enphaseenergy.com/pv/public_systems/1173801/today

            string url = string.Format("https://enlighten.enphaseenergy.com/pv/public_systems/{0}/lifetime_energy", id);

            Console.WriteLine("Getting lifetime energy data from: " + url);

            using (var stream = await Client.GetStreamAsync(url))
            {
                var serializer = new DataContractJsonSerializer(typeof(LifetimeEnergyData));
                return serializer.ReadObject(stream) as LifetimeEnergyData;
            }
        }
    }
}