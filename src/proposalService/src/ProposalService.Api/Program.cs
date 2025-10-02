using Insurence.Platform.Common.Helpers;
using ProposalService.Api.Extensions;
using ProposalService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new LowercaseControllerModelConvention());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
    builder.Services.AddSwaggerDocumentation();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
    app.UseSwaggerDocumentation();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
