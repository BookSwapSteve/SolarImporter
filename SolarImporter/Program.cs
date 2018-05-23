namespace SolarImporter
{
    /// <summary>
    /// Gets the energy data from Enlighten public systems feeds and converts 
    /// it to csv files.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The Enphase system Id for the solar install.
        /// Solar Roadways (https://enlighten.enphaseenergy.com/pv/public_systems/V3vh1173801/overview) is 1173801
        /// https://enlighten.enphaseenergy.com/pv/public_systems/1173801/lifetime_energy
        /// 
        /// You can get the system id from the URL in the "Take me to the logged-in view" button.
        /// </summary>
        const string systemId = "1173801";

        static void Main(string[] args)
        {
            LifetimeImporter.ExportLifetimeEnergyAsync(systemId).Wait();

            DailyImporter.ExportDailyEnergyAsync(systemId).Wait();
        }
    }
}
