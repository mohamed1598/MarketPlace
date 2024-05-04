var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseRouting();
//app.UseSwagger();
//app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClassifedAds v1"));
app.UseSwagger();
app.UseSwaggerUI();
app.Run();
