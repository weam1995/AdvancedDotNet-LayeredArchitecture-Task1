using Asp.Versioning;
using CartServiceApp.BusinessLogic.Interfaces;
using CartServiceApp.BusinessLogic.Services;
using CartServiceApp.DataAccess;
using Microsoft.Extensions.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<LiteDbOptions>(builder.Configuration.GetSection("LiteDbOptions"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = new QueryStringApiVersionReader("api-version");
    //o.ApiVersionReader = ApiVersionReader.Combine(
    //    new QueryStringApiVersionReader("api-version")
    //    //new HeaderApiVersionReader("X-Version"),
    //    //new MediaTypeApiVersionReader("ver")
    //    );
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSingleton<ILiteDbContext, LiteDbContext>();
builder.Services.AddScoped<ICartService, CartService>();
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
