using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using WebAppWithVersioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = false;
    //options.DefaultApiVersion = new ApiVersion(2, 0);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddVersionedApiExplorer(x =>
{
    x.GroupNameFormat = "'v'VVV";
    x.SubstituteApiVersionInUrl = true;
    x.ApiVersionParameterSource = new UrlSegmentApiVersionReader();
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

var apiVersionDescriptionProvider = app.Services.GetService<IApiVersionDescriptionProvider>();

app.UseSwaggerUI(options =>
{
    foreach (var desc in apiVersionDescriptionProvider?.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.ApiVersion.ToString());
        options.RoutePrefix = "";
    }
});

app.Run();
