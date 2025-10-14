using COMMON.CONTRACT.Abstractions.Behaviors;
using COMMON.CONTRACT.Abstractions.Shared;
using FluentValidation;

using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace INVENTORY.APPLICATION.DependencyInjections.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConffigMediatR(this IServiceCollection services)
        => services.AddMediatR(opt => opt.RegisterServicesFromAssembly(AssemblyReference.Assembly))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
            .AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);

    // public static IServiceCollection AddConfigurationAutoMapper(this IServiceCollection services)
    //     => services.AddAutoMapper(typeof(ServiceProfile));
    
    public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
        => services.AddAutoMapper(typeof(AssemblyReference).Assembly);
    
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        => services.AddScoped<ICurrentUserService, CurrentUserService>();
}