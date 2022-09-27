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
                    (station.ParameterId.Contains("temp_mean_past1h") == true) &&
                    (station.Country == "DNK"))
                {
                    stationList.Add(station.StationId, station);

                    Debug.WriteLine($"Adding station {station.Name}");
                }
            }
            Debug.WriteLine($"Added {stationList.Count} stations");

            int stationCount = 0;

            DateTime startTime = new DateTime(2012, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            DateTime endTime = new DateTime(2021, 12, 31, 23, 59, 59, 0, DateTimeKind.Local);

            foreach (Station station in stationList.Values)
            {
                int count = 0;
                int limit = 10000;
                int offset = 0;

                // if (station.Name == "Mejrup")
                {

                    do
                    {
                        var observations = await client.GetObservations(
                            limit: limit,
                            offset: offset,
                            stationId: station.StationId,
                            fromDateTime: startTime,
                            toDateTime: endTime,
                            parameterId: "temp_mean_past1h");

                        if (observations.ContainsKey(station.StationId))
                        {
                            StationData stationData = observations[station.StationId];
                            count = stationData.Count;
                            
                            foreach (string parameterName in stationData.Parameters.Keys)
                            {
                                ParameterData parameterData;
                                if (station.Parameters.ContainsKey(parameterName))
                                {
                                    parameterData = station.Parameters[parameterName];
                                }
                                else
                                {
                                    parameterData = new ParameterData()
                                    {
                                        Location = null,
                                        Name = parameterName,
                                        ObservationData = new SortedList<DateTime, Observation>()
                                    };
                                    station.Parameters.Add(parameterName, parameterData);
                                }

                                foreach (Observation observation in stationData.Parameters[parameterName].ObservationData.Values)
                                {
                                    parameterData.ObservationData.Add(observation.Observed, observation);
                                }
                            }

                        }
                        else
                        {
                            count = 0;
                        }

                        offset += count;

                        Debug.WriteLine($"Got {count} elements for {station.Name}");
                        if (count == 0)
                        {
                            // Shit Hack :-(
                            stationCount++;
                        }

                        //if (count > 0)
                        //{
                        //    Debug.WriteLine($"First timestamp is : {observations.First().Value.Parameters.First().Value.ObservationData.First().Value.Observed.ToString()}");
                        //    Debug.WriteLine($"Last timestamp is : {observations.First().Value.Parameters.First().Value.ObservationData.Last().Value.Observed.ToString()}");
                        //}

                    } while (count > 0);

                    //if (stationCount >= 5)
                    //{
                    //    break;
                    //}

                }
                // Only take the first station
                // break;
            }


            StreamWriter streamWriter = new StreamWriter("C:\\Users\\Jon\\Desktop\\data.csv");

            streamWriter.Write("Timestamp; Year");
            foreach (Station station in stationList.Values)
            {
                if (station.Parameters.Count > 0)
                {
                    streamWriter.Write($";{station.Name}");
                }
            }

            streamWriter.WriteLine("");

            for (DateTime time = startTime; time <= endTime; time += TimeSpan.FromHours(1.0))
            {
                streamWriter.Write($"{time};{time.Year}");

                foreach (Station station in stationList.Values)
                {
                    if (station.Parameters.Count > 0)
                    {

                        if (station.Parameters["temp_mean_past1h"].ObservationData.ContainsKey(time))
                        {
                            streamWriter.Write($";{station.Parameters["temp_mean_past1h"].ObservationData[time].Value}");
                        }
                        else
                        {
                            streamWriter.Write(";");
                        }
                    }
                }
                streamWriter.WriteLine("");
            }

            streamWriter.Close();

            Debug.WriteLine("We are done");
        }
    }
}