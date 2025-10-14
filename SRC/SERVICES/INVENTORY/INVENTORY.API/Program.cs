using System.Text.Json.Serialization;
using INVENTORY.APPLICATION.DependencyInjections.Extensions;
using INVENTORY.PERSISTENCE.DependencyInjections.Extensions;
using INVENTORY.PERSISTENCE.DependencyInjections.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add configuration
builder.Services.AddHttpContextAccessor();

builder.Services.AddConffigMediatR();
builder.Services.AddApplicationServices();
builder.Services.ConfigureSqlServerRetryOptions(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));
builder.Services.AddSqlConfiguration();

builder.Services.AddRepositoryBaseConfiguration();

builder.Services.AddAuthorization();

builder.Services.AddAutoMapperConfig();

// builder.Services.AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();