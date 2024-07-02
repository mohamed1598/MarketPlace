using EventStore.Client;
using MarketPlace.Ads;
using MarketPlace.Ads.Domain.Shared;
using MarketPlace.EventStore;
using MarketPlace.Framework;
using MarketPlace.Infrastructure;
using MarketPlace.Modules.Images;
using MarketPlace.Users;
using MarketPlace.WebAPI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using System.Security.Cryptography.X509Certificates;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//event store
const string connectionString = "esdb+discover://admin:changeit@localhost:2113?tls=true&tlsVerifyCert=false";

var settings = EventStoreClientSettings.Create(connectionString);

var client = new EventStoreClient(settings);

var store = new EsAggregateStore(client);
builder.Services.AddSingleton<IAggregateStore>(store);
//builder.Services.AddSingleton(new ClassifiedAdsApplicationService(store, new FixedCurrencyLookup()));
builder.Services.AddSingleton(client);
// rabendb
var documentStore = ConfigureRavenDb(builder.Configuration.GetSection("ravenDb"));
builder.Services.AddSingleton(new ImageQueryService(ImageStorage.GetFile));
builder.Services.AddSingleton<EventStoreClient>(client);
builder.Services.AddScoped<IAsyncDocumentSession>(provider =>
{
    return documentStore.OpenAsyncSession();
});
builder.Services.AddSingleton(documentStore);
// services
var purgomalumClient = new PurgomalumClient();
builder.Services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
//builder.Services.AddScoped(c =>
//new UserProfileApplicationService(
// text => purgomalumClient.CheckForProfanity(text).Result,
//store));
//builder.Services.AddScoped<ClassifiedAdsApplicationService>();



//builder.Services.AddSingleton<ProjectionManager>(provider =>
//{
//    var session = documentStore.OpenAsyncSession();
//    return new ProjectionManager(client,
//                new RavenDbCheckpointStore(session, "readmodels"),
//                new ClassifiedAdDetailsProjection(session,
//                    async userId => (await session.GetUserDetails(userId))?.DisplayName),
//                new ClassifiedAdUpcasters(client,
//                    async userId => (await session.GetUserDetails(userId))?.PhotoUrl),
//                new UserDetailsProjection(session));
//});
builder.Services.AddHostedService<EventStoreService>();
builder.Services
                .AddAuthentication(
                    CookieAuthenticationDefaults.AuthenticationScheme
                )
                .AddCookie();

builder.Services
    .AddMvcCore(
        options => options.Conventions.Add(new CommandConvention())
    );
builder.Services.AddUsersModule("Users", purgomalumClient.CheckForProfanity);
builder.Services.AddAdsModule("ClassifiedAds",new FixedCurrencyLookup(), ImageStorage.UploadFile);
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