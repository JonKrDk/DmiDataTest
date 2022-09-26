using Microsoft.Extensions.Configuration;
using System.Reflection;
using DmiDataLib;
using DmiDataLib.Data;
using System.Diagnostics;
using static System.Collections.Specialized.BitVector32;

namespace TestBench
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for btnTest Clicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnTest_Click(object sender, EventArgs e)
        {
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.AddUserSecrets(Assembly.GetExecutingAssembly());
            IConfiguration config = configBuilder.Build();
            string apiKey = config.GetSection("MetObsV2")["ApiKey"];

            MetObsClient client = new MetObsClient(apiKey);

            var stations = await client.GetStations();
            var sortedStations = stations.OrderBy(station => station.Name);

            tbResult.Text = "";
            
            foreach (Station station in sortedStations)
            {
                string parameters = "";
                foreach (string parameter in station.ParameterId)
                {
                    if (parameters != "")
                    { 
                        parameters += ", "; 
                    }
                    parameters += $"{parameter}";
                }
                tbResult.Text += $"{station.Name}: [Id:{station.StationId}] {parameters}\r\n";
            }
        }

        /// <summary>
        /// Event handler for btnObservations Clicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnObservations_Click(object sender, EventArgs e)
        {
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.AddUserSecrets(Assembly.GetExecutingAssembly());
            IConfiguration config = configBuilder.Build();
            string apiKey = config.GetSection("MetObsV2")["ApiKey"];

            MetObsClient client = new MetObsClient(apiKey);

            var observations = await client.GetObservations(
                stationId: "06056", 
                fromDateTime: new DateTime(2022, 8, 1, 0, 0, 0, DateTimeKind.Local), 
                toDateTime: new DateTime(2022, 8, 3, 0, 0, 0, DateTimeKind.Local),
                limit: 300000
                // parameterId: "wind_speed"
                );

            // var sortedStations = stations.OrderBy(station => station.Name);

            tbResult.Text = "";

            // foreach (Observation observation in observations)
            {
                //string parameters = "";
                //foreach (string parameter in station.ParameterId)
                //{
                //    if (parameters != "")
                //    {
                //        parameters += ", ";
                //    }
                //    parameters += $"{parameter}";
                //}
                //tbResult.Text += $"{station.Name}: [Status:{station.Status}] {parameters}\r\n";
            }
        }

        private async void btnGetTemps_Click(object sender, EventArgs e)
        {
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.AddUserSecrets(Assembly.GetExecutingAssembly());
            IConfiguration config = configBuilder.Build();
            string apiKey = config.GetSection("MetObsV2")["ApiKey"];

            MetObsClient client = new MetObsClient(apiKey);

            var stations = await client.GetStations();
            SortedList<string, Station> stationList = new SortedList<string, Station>();
            foreach (Station station in stations)
            {
                if ((stationList.ContainsKey(station.StationId) == false) &&
                    (station.ParameterId.Contains("temp_dry") == true) &&
                    (station.Country == "DNK"))
                {
                    stationList.Add(station.StationId, station);

                    Debug.WriteLine($"Adding station {station.Name}");
                }
            }
            Debug.WriteLine($"Added {stationList.Count} stations");

            foreach (Station station in stationList.Values)
            {
                int count = 0;
                int limit = 10000;
                int offset = 0;

                do
                {
                    var observations = await client.GetObservations(
                        limit: limit,
                        offset: offset,
                        stationId: station.StationId,
                        fromDateTime: new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Local),
                        toDateTime: new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Local),
                        parameterId: "temp_dry");

                    count = observations.ContainsKey(station.StationId) ? observations[station.StationId].Count : 0;
                    offset += count;

                    Debug.WriteLine($"Got {count} elements for {station.StationId}");
                    if (count > 0)
                    {
                        Debug.WriteLine($"First timestamp is : {observations.First().Value.Parameters.First().Value.ObservationData.First().Value.Observed.ToString()}");
                        Debug.WriteLine($"Last timestamp is : {observations.First().Value.Parameters.First().Value.ObservationData.Last().Value.Observed.ToString()}");
                    }
                } while (count > 0);


                // Only take the first station
                break;
            }
        }
    }
}