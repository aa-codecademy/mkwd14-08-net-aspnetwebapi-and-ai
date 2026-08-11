using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OurFirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        //https://localhost:[port]/api/values
        [HttpGet] //we must specify the Http method
        public IEnumerable<string> GetStrings()
        {
            return new List<string> { "value1", "value2" };
        }

        //https://localhost:[port]/api/values/more
        [HttpGet("more")] //we must add something in the route, to have an unique route
        public IEnumerable<string> GetMoreStrings()
        {
            return new List<string> { "value1", "value2", "value3" };
        }

        [HttpGet("info")] //https://localhost:[port]/api/values/info
        public string GetInfo()
        {
            return "This is our values controller";
        }

        [HttpGet("{id}")] //https://localhost:[port]/api/values/1
        public int GetId(int id)
        {
            return id;
        }

    }
}
