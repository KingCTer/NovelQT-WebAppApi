using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NovelQT.Application.Elasticsearch;
using NovelQT.Application.Elasticsearch.Hosting;
using NovelQT.Infra.Data.Context;
using NovelQT.Services.Api.ProgramExtensions;
using Serilog;
using System.Reflection;

// ----- Add Serilog -----
SerilogExtension.AddSerilogConfiguration();

// SYSTEM: Create WebApplicationBuilder.
Log.Information("Configurating WebApplicationBuilder...");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Configuration
// ----- Use Serilog -----
builder.UseSerilogConfiguration();

// ----- Controllers -----
builder.Services.AddControllersWithViews();

builder.Services.AddOptions();

builder.Services.Configure<ElasticsearchOptions>(builder.Configuration.GetSection("Application:Elasticsearch"));
builder.Services.Configure<IndexerOptions>(builder.Configuration.GetSection("Application:Indexer"));

// ----- Database -----
builder.AddDatabaseConfiguration();

// ----- Identity -----
builder.AddIdentityConfiguration();

// ----- Cors -----
builder.Services.AddCors();

// ----- Auth -----
builder.AddAuthConfiguration();

// ----- AutoMapper -----
builder.AddAutoMapperConfiguration();

// ----- MediatR -----
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// ----- Hash -----

// ----- Swagger UI -----
builder.AddSwaggerConfiguration();

// ----- Health check -----
builder.AddHealthCheckConfiguration();

// ----- DI -----
builder.RegisterServices();

var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                o => o.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
               .Options;
var dbContextFactory = new ApplicationDbContextFactory(dbContextOptions);
builder.RegisterContextFactory(dbContextFactory);


#endregion
// End: Add services to the container.


// ----- Check Seed Data -----
// TODO: Change data generation method
var IsSeedData = DatabaseExtension.IsSeedData(ref args);


// SYSTEM: Build WebApplication.
Log.Information("Building WebApplication...");
var app = builder.Build();

// ----- Seed Data -----
app.DatabaseExtensionSeedData(IsSeedData);

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // ----- Use Swagger -----
    app.UseSwaggerConfiguration();

}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints => {
    // ----- Health check -----
    HealthCheckExtension.UseHealthCheckConfiguration(endpoints, app.Environment);
});


Log.Information("Starting host...");
app.Run();