using API;
using Rotation.Infra;
using Rotation.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.AddAPI();
builder.AddAplication();
builder.AddInfrastructure();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAPI();

app.Run();