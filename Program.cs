using Aplicacion_SOA.Models;
using Aplicacion_SOA.Services;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion_SOA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 📌 Registrar el DbContext con la cadena de conexión de appsettings.json
            builder.Services.AddDbContext<CatalogoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Catalogo")));

            // 📌 Registrar los servicios de negocio
            builder.Services.AddScoped<IProductoService, ProductoService>();
            builder.Services.AddScoped<IClienteService, ClienteService>();
            builder.Services.AddScoped<IPedidosService, PedidosService>();

            // 📌 Registrar HttpClient para comunicación entre microservicios
            builder.Services.AddHttpClient("ProductosService", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7148/api/");
            });

            builder.Services.AddHttpClient("ClientesService", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7148/api/");
            });

            // Configurar CORS (opcional para desarrollo)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aplicacion SOA v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
