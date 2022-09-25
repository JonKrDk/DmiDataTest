using Microsoft.Extensions.Configuration;
using System.Reflection;
using DmiDataLib;
using DmiDataLib.Data;

namespace TestBench
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.AddUserSecrets(Assembly.GetExecutingAssembly());
            IConfiguration config = configBuilder.Build();
            string apiKey = config.GetSection("MetObsV2")["ApiKey"];
            string baseUrl = config.GetSection("MetObsV2")["BaseUrl"];

            MetObsClient client = new MetObsClient(apiKey, baseUrl);

            var stations = client.GetStations();
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
                tbResult.Text += $"{station.Name}: [Status:{station.Status}] {parameters}\r\n";
            }
        }

        private void btnObservations_Click(object sender, EventArgs e)
        {
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.AddUserSecrets(Assembly.GetExecutingAssembly());
            IConfiguration config = configBuilder.Build();
            string apiKey = config.GetSection("MetObsV2")["ApiKey"];
            string baseUrl = config.GetSection("MetObsV2")["BaseUrl"];

            MetObsClient client = new MetObsClient(apiKey, baseUrl);

            var observations = client.GetObservations(
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
    }
}