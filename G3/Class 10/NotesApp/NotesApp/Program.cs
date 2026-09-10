using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NotesApp.DataAccess;
using NotesApp.DataAccess.Implementation;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Services.Implementation;
using NotesApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register services

builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<IUserService, UserService>();

//Register repositories



//Register ADO repository - we need to create an instance of NoteAdoRepoistory and send the connection string to it
//builder.Services.AddScoped<INoteRepository>(
//    _ => new NoteAdoRepository("Server=.\\SQLExpress;Database=NotesAppG3;Trusted_Connection=True;TrustServerCertificate=True"));

//Register Dapper repository - we need to create an instance of NoteAdoRepoistory and send the connection string to it
//builder.Services.AddScoped<INoteRepository>(
//    _ => new NoteDapperRepository("Server=.\\SQLExpress;Database=NotesAppG3;Trusted_Connection=True;TrustServerCertificate=True"));


builder.Services.AddScoped<INoteRepository, NoteEFRepository>();
builder.Services.AddScoped<IUserRepository, UserEFRepository>();
builder.Services.AddScoped<ITagRepository, TagEFRepository>();

//Register the db context
builder.Services.AddDbContext<NoteDbContext>(x => x.UseSqlServer("Server=.\\SQLExpress;Database=NotesAppG3;Trusted_Connection=True;TrustServerCertificate=True"));


builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Our secret secret secret secret secret secret secret secret key")),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); //always use authentication before authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
