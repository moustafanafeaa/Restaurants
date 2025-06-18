using Restaurants.Infrastructure.Extentions;
using Restaurants.Infrastructure.Seeders;
using Restaurants.Application.Extensions;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Restaurants.API.MiddleWares;
using Restaurants.Domain.Entities;
using Microsoft.OpenApi.Models;


try
{


    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("bearerAuth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth"}
            },
            []
        }
        });

        c.MapType<DateOnly>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "date"
        });
    });

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddScoped<ErrorHandlingMiddle>();
    builder.Services.AddScoped<TimeLoggingMiddle>();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration)
    );



    var app = builder.Build();


    using (var scope = app.Services.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
        await seeder.Seed();
    }
    // Configure the HTTP request pipeline.  
    //if (app.Environment.IsDevelopment())
    //{
    app.UseSwagger();
    app.UseSwaggerUI();
    //}
    //else
    //{
    //    app.UseSwagger();
    //    app.UseSwaggerUI(c =>
    //    {
    //        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    //        c.RoutePrefix = string.Empty; // Serve Swagger UI at the root URL
    //    });
    //}

    app.UseMiddleware<ErrorHandlingMiddle>();
    app.UseMiddleware<TimeLoggingMiddle>();

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();

    app.MapGroup("api/identity/")
        .WithTags("Identity")
        .MapIdentityApi<User>();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Application startup faild");
}
finally
{
    Log.CloseAndFlush();
}
public partial class Program { }