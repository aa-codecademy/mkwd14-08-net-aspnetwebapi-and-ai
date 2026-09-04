using Microsoft.Extensions.DependencyInjection;
using NotesApp.DataAccess.Implementations.EntityFramework;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Services.Implementations;
using NotesApp.Services.Interfaces;

namespace NotesApp.Helpers;

public static class DependencyInjectionHelper
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<INoteRepository, NoteRepository>(); // EF Core
        //services.AddScoped<INoteRepository, NoteRepositoryAdoNet>(); // ADO.NET
        //services.AddScoped<INoteRepository, NoteRepositoryDapper>(); // Dapper 
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<INoteService, NoteService>();
    }
}
