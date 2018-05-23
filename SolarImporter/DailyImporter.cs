using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using SolarImporter.Models;

namespace SolarImporter
{
    /// <summary>
    /// Import / Export daily energy data
    /// </summary>
    public static class DailyImporter
    {
        private static readonly HttpClient Client = new HttpClient();
        private static readonly DateTime EpochDate = new DateTime(1970,1, 1);

        public static async Task ExportDailyEnergyAsync(string systemId)
        {
            DateTime date = new DateTime(2017, 03, 01);

            List<ProductionStatistics> productionStatistics = new List<ProductionStatistics>();
            do
            {
                DailyEnergyData dailyEnergyData = await GetDailyEnergyAsync(systemId, date);
                productionStatistics.AddRange(dailyEnergyData.stats);               

                // Daily energy data contains data to the end of the month
                // so jump in months.
                date = date.AddMonths(1);

            } while (date < DateTime.Now);

            ExportDailyEnergy(productionStatistics);
        }

        private static void ExportDailyEnergy(List<ProductionStatistics> productionStatistics)
        {
            if (productionStatistics.Count == 0)
            {
                return;
            }

            string filename = @".\DailyEnergyByDay.csv";

            var interval = productionStatistics.First().interval_length;

            using (StreamWriter file = File.CreateText(filename))
            {
                // Header (Date, 00:00, 00:15, 00:30, 00:45...)
                file.Write("Date,");

                int intervalOffset = 0;
                do
                {
                    DateTime time = EpochDate.AddSeconds(intervalOffset); // hack to get 00:00:00 easily...
                    file.Write(" {0:HH:mm},", time);
                    intervalOffset += interval;
                } while (intervalOffset < 86400);


                // Daily stats data
                foreach (var dayStats in productionStatistics.OrderBy(x => x.start_time))
                {
                    string data = string.Join(",", dayStats.production);
                    DateTime date = EpochDate.AddSeconds(dayStats.start_time);
                    file.WriteLine("{0},{1}", date, data);
                }
            }

            Console.WriteLine("Wrote daily energy by day to '{0}'", filename);
        }

        private static async Task<DailyEnergyData> GetDailyEnergyAsync(string id, DateTime date)
        {
            // See also... https://enlighten.enphaseenergy.com/pv/public_systems/1173801/today

            string url = string.Format("https://enlighten.enphaseenergy.com/pv/public_systems/{0}/daily_energy?start_date={1}",
                id,
                date.ToString("yyyy-MM-dd"));

            Console.WriteLine("Getting daily energy data from: " + url);

            using (var stream = await Client.GetStreamAsync(url))
            {
                var serializer = new DataContractJsonSerializer(typeof(DailyEnergyData));
                return serializer.ReadObject(stream) as DailyEnergyData;
            }
        }
    }
}