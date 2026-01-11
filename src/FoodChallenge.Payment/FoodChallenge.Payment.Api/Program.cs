using FoodChallenge.Payment.Api.Filters;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using FoodChallenge.Common.Validators;
using FoodChallenge.Infrastructure.Clients.MercadoPago;
using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Clientes.Interfaces;
using FoodChallenge.Ioc;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

var mongoConnectionString = configuration.GetSection("MongoDb:ConnectionString").Value ?? string.Empty;
var mongoDatabaseName = configuration.GetSection("MongoDb:DatabaseName").Value ?? string.Empty;
builder.Services.AddMongoDbDependency(mongoConnectionString, mongoDatabaseName);
builder.Services.AddControllersDependency();
builder.Services.AddRepositoryDependency();

BootstrapMercadoPago.Configure(builder.Services, configuration);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ValidationFilterAttribute>();
        options.Filters.Add<ExceptionFilterAttribute>();
    })
    .AddJsonOptions(jsonOptions =>
    {
        jsonOptions.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddScoped(_ => new ValidationContext());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddDefaultHealthChecks();

const string originsPolicy = "AllowAllOrigins";
var headersExposed = new[] { "Date", "Content-Type", "Content-Disposition", "Content-Length" };

builder.Services.AddCors(options =>
{
    options.AddPolicy(originsPolicy, x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders(headersExposed));
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IClienteSeedService>();
await seeder.SeedAsync();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "FoodChallenge API v1");
});

app.UseHeaderPropagation();
app.MapHealthCheckDefaultEndpoints();
app.UseCors(originsPolicy);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
