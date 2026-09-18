using Microsoft.Extensions.Options;
using NotesAppClientApi.Configuration;
using NotesAppClientApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===> Bind the "NotesApi" section of appsettings.json to a class, so nobody
// reads configuration by string key - they ask for IOptions<NotesApiSettings>.
builder.Services.Configure<NotesApiSettings>(builder.Configuration.GetSection("NotesApi"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
