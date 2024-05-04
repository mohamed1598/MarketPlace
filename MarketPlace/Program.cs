using MarketPlace.Data;
using MarketPlace.Domain;
using MarketPlace.Services;
using Raven.Client.Documents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var store = new DocumentStore
{
    Urls = new[] { "http://127.0.0.1:38888" },
    Database = "Marketplace_Chapter6",
    Conventions =
                  {
                      FindIdentityProperty = m => m.Name == "_databaseId"
                  }
};
store.Conventions.RegisterAsyncIdConvention<ClassifiedAd>(
    (dbName, entity) => Task.FromResult("ClassifiedAd/" + entity.Id.ToString()));
store.Initialize();

builder.Services.AddTransient(c => store.OpenAsyncSession());
builder.Services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
builder.Services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
builder.Services.AddSingleton<ClassifiedAdsApplicationService>();

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
