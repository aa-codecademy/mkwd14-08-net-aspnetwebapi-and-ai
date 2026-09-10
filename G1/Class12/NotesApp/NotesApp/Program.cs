using Microsoft.EntityFrameworkCore;
using NotesApp.DataAccess.Data;
using NotesApp.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===> Configure the JWT authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// ===> Register the database
// AddDbContext makes the DbContext Scoped: a fresh one per HTTP request.
// Never a singleton - a DbContext remembers the objects it loaded, so sharing
// one would leak data between requests.
builder.Services.AddDbContext<NotesAppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("NotesAppDb")));

// ===> Register services
builder.Services.AddApplicationServices();

// ===> Register repositories
builder.Services.AddRepositories();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// The order of middleware is important. Authentication must come before Authorization, otherwise the authorization middleware won't have a user principal to check against and will return a 401 Unauthorized response for all requests, even if the JWT token is valid
app.UseAuthentication(); // This middleware checks the request for a valid JWT token and sets the user principal if valid
app.UseAuthorization();

app.MapControllers();

app.Run();
