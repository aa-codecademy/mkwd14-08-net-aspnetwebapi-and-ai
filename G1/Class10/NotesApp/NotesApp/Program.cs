using Microsoft.EntityFrameworkCore;
using NotesApp.DataAccess.Data;
using NotesApp.Helpers;
using NotesApp.Services.Implementations;
using NotesApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
