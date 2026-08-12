using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Class02.ControllersAndActions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet] //no additional rout
        //https://localhost:[port]/api/values
        public List<string> Get()
        {
            return new List<string> { "value1", "value2" };
        }

        [HttpGet("info")] //https://localhost:[port]/api/values/info
        public string GetInfo()
        {
            return "This is a simple API controller that returns values.";
        }

        //HAS SAME HTTPMETHOD AND SAME ADDRESS!!! -> Will cause error while starting the API 
        //The controller doesn't know how to make difference betweeen Get() and GetString()
        //Give custom name to at least one of them in order to work!

        //[HttpGet]
        //public string GetString()
        //{
        //    return "test";
        //}

        [HttpPost]
        public string Post()
        {
            return "OK";
        }

        [HttpGet("details/{id:int}")] //https://localhost:[port]/api/values/details/5
        public string GetById(int id)
        {
            return $"value {id}";
        }
    }
}
