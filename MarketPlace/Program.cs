using EventStore.Client;
using MarketPlace;
using MarketPlace.ClassifiedAd;
using MarketPlace.Domain.Shared;
using MarketPlace.Framework;
using MarketPlace.Infrastructure;
using MarketPlace.UserProfiles;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide;
using static Raven.Client.Constants;
using Raven.Client.Documents.Session;
using MarketPlace.Projections;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//event store
const string connectionString = "esdb+discover://admin:changeit@localhost:2113?tls=true&tlsVerifyCert=false";

var settings = EventStoreClientSettings.Create(connectionString);

var client = new EventStoreClient(settings);

var store = new EsAggregateStore(client);
builder.Services.AddSingleton<IAggregateStore>(store);
builder.Services.AddSingleton(new ClassifiedAdsApplicationService(store, new FixedCurrencyLookup()));
builder.Services.AddSingleton(client);
// rabendb
var documentStore = ConfigureRavenDb(builder.Configuration.GetSection("ravenDb"));
Func<IAsyncDocumentSession> getSession = () => documentStore.OpenAsyncSession();
builder.Services.AddTransient(c => getSession());

// services
var purgomalumClient = new PurgomalumClient();
builder.Services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
builder.Services.AddScoped(c =>
new UserProfileApplicationService(
 text => purgomalumClient.CheckForProfanity(text).Result,
store));
builder.Services.AddScoped<ClassifiedAdsApplicationService>();


var projectionManager = new ProjectionManager(client,
                new RavenDbCheckpointStore(getSession, "readmodels"),
                new ClassifiedAdDetailsProjection(getSession,
                    async userId => (await getSession.GetUserDetails(userId))?.DisplayName),
                new ClassifiedAdUpcasters(client,
                    async userId => (await getSession.GetUserDetails(userId))?.PhotoUrl),
                new UserDetailsProjection(getSession));

builder.Services.AddSingleton(projectionManager);
builder.Services.AddHostedService<HostedService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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



//
 static IDocumentStore ConfigureRavenDb(IConfiguration configuration)
{
    var certificatePath = @"D:\admin.client.certificate.shafey\shafey.pfx"; // Use the path from the export process
    var certificatePassword = "shafey"; // Password used during the export

    var certificate = new X509Certificate2(certificatePath, certificatePassword);
    var store = new DocumentStore
    {
        Urls = new[] { configuration["server"] },
        Database = configuration["database"],
        Certificate = certificate
    };
    store.Initialize();
    var record = store.Maintenance.Server.Send(
        new GetDatabaseRecordOperation(store.Database));
    if (record == null)
    {
        store.Maintenance.Server.Send(
            new CreateDatabaseOperation(new DatabaseRecord(store.Database)));
    }

    return store;
}