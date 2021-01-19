using Microsoft.AspNetCore.Mvc;
using PetCareIoTMiddleware.Models;
using PetCareIoTMiddleware.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetCareIoTMiddleware.Controllers
{
    [Route("api/data")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly DataService dataService;

        public DataController(DataService dataService)
        {
            this.dataService = dataService;
        }

        [HttpGet("get-last-light-state")]
        public int GetLastLightState()
        {
            return dataService.GetLastLightState();
        }

        [HttpGet("get-all-pump-events")]
        public List<BaseEvent> GetAllPumpEvents()
        {
            return dataService.GetAllPumpEvents();
        }

        [HttpGet("get-all-feeder-events")]
        public List<BaseEvent> GetAllFeederEvents()
        {
            return dataService.GetAllPumpEvents();
        }
    }
}