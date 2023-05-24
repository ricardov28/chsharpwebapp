//  file that contains the logic for handling HTTP requests related to telemetry data. 
// It defines methods such as Index that return views to be displayed in the user's browser.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace YourNamespace
{
    public class TelemetryController : Controller
    {
        // SignalR hub context that allows the controller to communicate with the hub
        private readonly IHubContext<TelemetryHub> _hubContext;

        // Constructor that initializes the hub context
        public TelemetryController(IHubContext<TelemetryHub> hubContext)
        {
            _hubContext = hubContext;
        }

        
        // Action method that fetches telemetry data from the hub and sends it to the view
        public async Task<IActionResult> Index()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            string connectionId = "1164e8a7-71ba-4a97-989d-dc7ea0204d15"; // Connection Id of the client

            List<TelemetryModel> telemetryDataList = new();

            // Fetch telemetry data from a particular client via the hub. You may itrate through the multiple clients to get individual responses.
            TelemetryModel telemetryData = await _hubContext.Clients.Client(connectionId).InvokeCoreAsync<TelemetryModel>("GetTelemetryData", new object[] { }, token);

            telemetryDataList.Add(telemetryData);

            // Create a list to store telemetry data models
            List<TelemetryModel> telemetryList = new List<TelemetryModel>();

            // Convert the telemetry data received from the hub to TelemetryModel objects
            foreach (var telemetry in telemetryDataList)
            {
                // Create a new TelemetryModel object and populate it with the telemetry data
                telemetryList.Add(new TelemetryModel
                {
                    ConnectionDeviceId = telemetry.ConnectionDeviceId,
                    EnqueuedTime = telemetry.EnqueuedTime,
                    Altitude = telemetry.Altitude,
                    Latitude = telemetry.Latitude,
                    Longitude = telemetry.Longitude,
                    Pitch = telemetry.Pitch,
                    Roll = telemetry.Roll,
                    Yaw = telemetry.Yaw
                });
            }

            // Pass the list of telemetry data models to the view
            return View(telemetryList);
        }
    }

    // Data model that represents telemetry data
    public class TelemetryModel
    {
        public string ConnectionDeviceId { get; set; }
        public string EnqueuedTime { get; set; }
        public double Altitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Pitch { get; set; }
        public double Roll { get; set; }
        public double Yaw { get; set; }
    }

    // SignalR hub that handles telemetry data communication between clients and server
    public class TelemetryHub : Hub
    {
        // Method that sends telemetry data to all connected clients
        public async Task GetTelemetryData(TelemetryModel telemetry)
        {
            await Clients.All.SendCoreAsync("GetTelemetryData", new object[] { });
            return;
        }
        public async Task SendTelemetryData(TelemetryModel telemetry)
        {
            await Clients.All.SendAsync("GetTelemetryData", telemetry);
        }

    }

   
}
