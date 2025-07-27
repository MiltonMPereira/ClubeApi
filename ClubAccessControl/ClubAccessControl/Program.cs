using ClubAccessControl.API.Mappings;
using ClubAccessControl.Infrastructure;
using Microsoft.OpenApi.Models;
using System.Reflection;


var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    EnvironmentName = environment
});

// Carregar config específica do ambiente
builder.Configuration
    .SetBasePath(AppContext.BaseDirectory) 
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// ===================================
// Banco de dados 
// ===================================
builder.Services.AddPersistence(builder.Configuration);

// ===================================
// Injeção de dependências - repositórios e serviços
// ===================================
builder.Services.AddInfrastructure();

// ===================================
// AutoMapper
// ===================================
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ===================================
// Controllers & Swagger
// ===================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Club Access Control API",
        Version = "v1",
        Description = "API para controle de acesso às áreas de um clube",
        Contact = new OpenApiContact
        {
            Name = "Suporte",
            Email = "suporte@clube.com"
        }
    });
    c.EnableAnnotations();
    c.CustomSchemaIds(type => type.FullName);

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

// ===================================
// CORS
// ===================================
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

// ===================================
// Database EnsureCreated
// ===================================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ClubeContext>();
    context.Database.EnsureCreated();
}

// ===================================
// Middleware pipeline
// ===================================

Action configureProduction = () =>
{ 
    app.UseHsts();
    app.UseHttpsRedirection();
};

Action configureDevelopment = () =>
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Club Access Control API V1");
        c.RoutePrefix = string.Empty;
    });
};


(app.Environment.IsDevelopment() ? configureDevelopment : configureProduction)();


app.UseRouting();
app.UseCors("AllowAll");
app.MapControllers();
app.Run();

public partial class Program { }
