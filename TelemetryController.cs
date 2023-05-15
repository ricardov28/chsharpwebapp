using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
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
            // Fetch telemetry data from all connected clients via the hub
            var telemetryData = await _hubContext.Clients.All.SendAsync("GetTelemetryData");

            // Create a list to store telemetry data models
            List<TelemetryModel> telemetryList = new List<TelemetryModel>();

            // Convert the telemetry data received from the hub to TelemetryModel objects
            foreach (var telemetry in telemetryData)
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
        public async Task SendTelemetryData(TelemetryModel telemetry)
        {
            await Clients.All.SendAsync("GetTelemetryData", telemetry);
        }
    }
}
