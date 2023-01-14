using Microsoft.EntityFrameworkCore;
using StokKontrol_API.Repositories.Abstract;
using StokKontrol_API.Repositories.Concrete;
using StokKontrol_API.Repositories.Context;
using StokKontrol_API.Service.Abstract;
using StokKontrol_API.Service.Concrete;

namespace StokKontrol_API.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StokKontrolContext>(options =>
            {
                options.UseSqlServer("Server=desktop-ufhr98h; Database=StokKontrolAPI; uid=sa; pwd=123");
            });

            builder.Services.AddTransient(typeof(IGenericService<>), typeof(GenericManager<>));
            builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

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
        }
    }
}