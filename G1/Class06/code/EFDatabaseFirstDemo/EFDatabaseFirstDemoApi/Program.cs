using EFDatabaseFirstDemoApi.Domain.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

// =============================================================================
// Database First demo - Class 06
//
// 1. Run DatabaseFirstQuery.sql in SSMS. It creates TodoAppDb.
// 2. The EF Core packages are already in the .csproj.
// 3. Scaffold the DbContext and the models FROM the database:
//
//    A) Package Manager Console (Visual Studio):
//      Scaffold-DbContext "Server=.\SQLEXPRESS;Database=TodoAppDb;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Domain/Models -Context AppDbContext -ContextDir Domain/Context 
//
//    B) dotnet CLI (same thing):
//      dotnet ef dbcontext scaffold "Server=.\SQLEXPRESS;Database=TodoAppDb;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Domain/Models --context AppDbContext --context-dir Domain/Context 
// =============================================================================

var builder = WebApplication.CreateBuilder(args);

// Entities point at each other: Todo.Category -> Category.Todos -> Todo...
// Serializing that to JSON throws "A possible object cycle was detected".
// IgnoreCycles writes null where the loop closes - a quick fix.
// The real fix is DTOs, which the Notes App already uses.
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); // for demo
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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
