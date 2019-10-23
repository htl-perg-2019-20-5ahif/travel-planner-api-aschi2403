using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TravelPlanner.Logic;

namespace TravelPlanner.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TravelPlanController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient client;
        private readonly IEnumerable<Route> routes;
        private readonly ConnectionFinder connectionFinder;

        public TravelPlanController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            client = clientFactory.CreateClient();
            routes = GetRoutesFromAPIAsync().Result;
            connectionFinder = new ConnectionFinder(routes);
        }

        [HttpGet]
       public ActionResult<TripWithLocationsAPIResult> GetTripWithLocations(string from, string to, string start)
        {
            var connectionResult = connectionFinder.FindConnection(from, to, start);
            if (connectionResult == null)
                return NotFound();
            return ConvertConnectionResult(connectionResult);
        }

        private TripWithLocationsAPIResult ConvertConnectionResult(TripWithLocations connectionResult)
        {
            return new TripWithLocationsAPIResult()
            {
                Depart = connectionResult.FromCity,
                DepartureTime = connectionResult.Leave,
                Arrive = connectionResult.ToCity,
                ArrivalTime = connectionResult.Arrive
            };
        }

        private async Task<IEnumerable<Route>> GetRoutesFromAPIAsync()
        {
            var routesResponse = await client.GetAsync("https://cddataexchange.blob.core.windows.net/data-exchange/htl-homework/travelPlan.json");
            routesResponse.EnsureSuccessStatusCode();
            var responseBody = await routesResponse.Content.ReadAsStringAsync();
            var routes = JsonSerializer.Deserialize<Route[]>(responseBody);
            return routes;
        }
    }
}
