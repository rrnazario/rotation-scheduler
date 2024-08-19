using API;
using Rotation.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.AddAplication();
builder.AddInfrastructure();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseApplication();

app.Run();