using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourNamespace
{
    public class TelemetryController : Controller
    {
        private readonly IHubContext<TelemetryHub> _hubContext;

        public TelemetryController(IHubContext<TelemetryHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var telemetryData = await _hubContext.Clients.All.SendAsync("GetTelemetryData");

            List<TelemetryModel> telemetryList = new List<TelemetryModel>();

            foreach (var telemetry in telemetryData)
            {
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

            return View(telemetryList);
        }
    }

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

    public class TelemetryHub : Hub
    {
        public async Task SendTelemetryData(TelemetryModel telemetry)
        {
            await Clients.All.SendAsync("GetTelemetryData", telemetry);
        }
    }
}
