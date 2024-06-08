using EventStore.Client;
using MarketPlace;
using MarketPlace.ClassifiedAd;
using MarketPlace.Domain.Shared;
using MarketPlace.Framework;
using MarketPlace.Infrastructure;
using MarketPlace.UserProfiles;
using Raven.Client.Documents.Operations.ConnectionStrings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
const string connectionString = "esdb+discover://admin:changeit@localhost:2113?tls=true&tlsVerifyCert=false";

var settings = EventStoreClientSettings.Create(connectionString);

var client = new EventStoreClient(settings);

var store = new EsAggregateStore(client);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var purgomalumClient = new PurgomalumClient();
builder.Services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
builder.Services.AddSingleton<IAggregateStore>(store);
builder.Services.AddSingleton(new ClassifiedAdsApplicationService(store,new FixedCurrencyLookup()));
builder.Services.AddScoped(c =>
new UserProfileApplicationService(
 text => purgomalumClient.CheckForProfanity(text).Result,
store));
builder.Services.AddScoped<ClassifiedAdsApplicationService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.EnsureDatabase();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
