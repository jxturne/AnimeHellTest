using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using JikanDotNet;
using Microsoft.EntityFrameworkCore;



namespace AnimeHellTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            

            builder.Services.AddDbContext<Models.AnimeDB>();
            builder.Services.AddScoped<Services.AnimeService>();
            builder.Services.AddSingleton<IJikan>(new Jikan());
            builder.Services.AddRazorPages();
            

            builder.Services.AddSwaggerGen();
           

            //builder.Services.AddSingleton<>;

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(); // ADD THIS LINE

                
            }

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<Models.AnimeDB>();
                //db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            //do i need to map controller?
            app.MapRazorPages();


            app.Run();
        }
    }
}
