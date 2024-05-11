using Marketplace.Domain;
using Marketplace.Framework;
using MarketPlace.Data;
using MarketPlace.Services;
using Microsoft.EntityFrameworkCore;
using Raven.Client.Documents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ClassifiedAdDbContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//const string connectionString =
// "Host=localhost:5432;Database=Marketplace_Chapter8;Username=POSTGRES;Password=mysecretpassword"; 
//builder.Services
// .AddEntityFrameworkNpgsql()
// .AddDbContext<ClassifiedAdDbContext>(
// options => options.UseNpgsql(connectionString));
//builder.Services.AddSingleton<Marketplace.Domain.ICurrencyLookup, FixedCurrencyLookup>();
builder.Services.AddSingleton<ICurrencyLookup,FixedCurrencyLookup>();
builder.Services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
builder.Services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>
();
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
