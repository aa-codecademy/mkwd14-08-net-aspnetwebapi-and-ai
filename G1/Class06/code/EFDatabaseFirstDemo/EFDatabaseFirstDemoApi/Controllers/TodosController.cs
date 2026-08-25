using EFDatabaseFirstDemoApi.Domain.Context;
using EFDatabaseFirstDemoApi.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFDatabaseFirstDemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TodosController(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/todos
        [HttpGet]
        public ActionResult<List<Todo>> GetAll()
        {
            // _context.Todos is a QUERY, not a list. Nothing has touched the database
            // yet, and Include() only adds JOINs to the query being built.
            // ToList() is the line that actually runs the SQL - that is deferred
            // execution, and it is the single most useful thing to say out loud here.
            List<Todo> todos = _context.Todos
                .Include(todo => todo.Category)
                .Include(todo => todo.Status)
                .ToList();

            return Ok(todos);
        }
    }
}
