using Assignment.Configuration;
using Assignment.Interfaces;
using Assignment.Services;

var builder = WebApplication.CreateBuilder(args);

if (File.Exists("appsettings.Secrets.json"))
{
    builder.Configuration.AddJsonFile("appsettings.Secrets.json", optional: true, reloadOnChange: true);
}

// Add services to the container.
builder.Services.AddControllers();

// Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Assignment API",
        Version = "v1",
        Description = "A Web API for Agile Actors' Assignment project",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "George Englezos",
            Email = "englezosgiorgos@gmail.com"
        }
    });

    // Include XML comments if you have them
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// HTTP clients
builder.Services.AddHttpClient<IOpenWeatherService, OpenWeatherService>();
builder.Services.AddHttpClient<IGithubApiService, GithubApiService>();
builder.Services.AddHttpClient<INewsApiService, NewsApiService>();

// Scoped
builder.Services.AddScoped<IAggregatedDataService, AggregatedDataService>(); 

// Configure appsettings
builder.Services.Configure<ApiSettings>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger middleware
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Assignment API V1");
        c.RoutePrefix = "swagger"; // Set Swagger UI at /swagger
        c.DocumentTitle = "Assignment API Documentation";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();