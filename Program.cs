
using CartServiceApp.BusinessLogic.Interfaces;
using CartServiceApp.BusinessLogic.Services;
using CartServiceApp.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;
using Asp.Versioning;
using CartServiceApp.Extension;
using Microsoft.Extensions.Options;
using CartServiceApp.HostedServcies;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<LiteDbOptions>(builder.Configuration.GetSection("LiteDbOptions"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

// Add API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;  // This reports the available API versions in response headers.
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);  // Set the default API version
})
    .AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";  // Format of the versioned API groups in Swagger
    options.SubstituteApiVersionInUrl = true; // Enables the versioned URL format
});


builder.Services.AddSingleton<ILiteDbContext, LiteDbContext>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddHostedService<ProductChangeKafkaConsumerService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
    options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });

}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();


app.Run();
