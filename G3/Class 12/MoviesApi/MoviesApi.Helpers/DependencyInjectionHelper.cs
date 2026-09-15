using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoviesApi.DataAccess;
using MoviesApi.DataAccess.Implementations;
using MoviesApi.DataAccess.Interfaces;
using MoviesApi.Services.Implementations;
using MoviesApi.Services.Interfaces;

namespace MoviesApi.Helpers
{
    public static class DependencyInjectionHelper
    {
        public static void InjectDbContext(IServiceCollection services)
        {
            services.AddDbContext<MovieDbContext>(x => x.UseSqlServer("Server=.\\SQLExpress;Database=MoviesAppG3;Trusted_Connection=True;TrustServerCertificate=True"));
        }

        public static void InjectRepositories(IServiceCollection services)
        {
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void InjectServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMovieService, MovieService>();
        }
    }
}
