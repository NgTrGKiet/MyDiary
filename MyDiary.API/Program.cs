using MyDiary.API.Extensions;
using MyDiary.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddPesentation();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
