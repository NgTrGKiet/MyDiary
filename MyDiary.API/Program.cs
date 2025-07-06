using MyDiary.API.Extensions;
using MyDiary.Application;
using MyDiary.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.AddPesentation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
