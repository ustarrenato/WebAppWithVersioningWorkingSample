using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using WebAppWithVersioning;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
    .AddMvc()
    .AddApiExplorer(x =>
    {
        x.GroupNameFormat = "'v'VVV";
        x.SubstituteApiVersionInUrl = true;
    });

//var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFile);

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();
builder.Services.AddSwaggerGen(options =>
{
    //options.IncludeXmlComments(xmlFilePath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "";
    foreach (var desc in app.DescribeApiVersions())
    {
        options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.ApiVersion.ToString());
    }
});

app.Run();
