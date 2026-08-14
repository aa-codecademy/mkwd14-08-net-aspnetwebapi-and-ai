using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NotesApp.Controllers
{
    [Route("api/[controller]")] //http://loclahost:[port]/api/values
    [ApiController]
    public class ValuesController : ControllerBase
    {

        [HttpGet] //no additional information in the URL, just the base URL
                  //http://loclahost:[port]/api/values
        public List<string> Get()
        {
            return new List<string>() { "value1", "value2" };
        }

        //[HttpGet] //ERROR - has the same http method and same route as the previous method, so it will not work
        ////http://loclahost:[port]/api/values
        //public int GetNumber()
        //{
        //    return 3;
        //}

        [HttpGet("number")] //http://loclahost:[port]/api/values/number
        public int GetNumber()
        {
            return 3;
        }

        [HttpPost]  //This is okay, because it uses a different HTTP method than the previous methods (even though the route is the same, the HTTP method is different)
                    //http://loclahost:[port]/api/values
        public string GetString()
        {
            return "Hello";
        }

        [HttpGet("{number}")] //http://loclahost:[port]/api/values/5
        public int GetFirstNumber(int number)
        {
            return number;
        }

        [HttpGet("{userId}/book/{bookId}")] //http://loclahost:[port]/api/values/1/book/2
        public string GetUserAndBook(int userId, int bookId)
        {
            return $"User: {userId}, book: {bookId}";
        }

        [HttpGet("movie/{number}")] //http://loclahost:[port]/api/values/movie/5
        public List<string> GetMovies(int number)
        {
            return number > 2 ? new List<string>() { "movie1", "movie2" } : new List<string>() { "movie3" };
        }
    }
}
