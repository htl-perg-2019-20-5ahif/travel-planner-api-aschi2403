using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace TravelPlanner.API
{
    public class TripWithLocationsAPIResult
    {
        [JsonPropertyName("depart")]
        public string Depart { get; set; }
        [JsonPropertyName("departureTime")]
        public string DepartureTime { get; set; }
        [JsonPropertyName("arrive")]
        public string Arrive { get; set; }
        [JsonPropertyName("arrivalTime")]
        public string ArrivalTime { get; set; }
    }
}
