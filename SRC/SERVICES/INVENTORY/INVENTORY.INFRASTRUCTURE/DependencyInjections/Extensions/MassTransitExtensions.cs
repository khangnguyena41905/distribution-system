using INVENTORY.INFRASTRUCTURE.DependencyInjections.Options;
using INVENTORY.INFRASTRUCTURE.EventHandlers;
using INVENTORY.INFRASTRUCTURE.EventHandlers.OutboxEvents.Products;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace INVENTORY.INFRASTRUCTURE.DependencyInjections.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddOutboxEventHandlers(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitOptions = new RabbitMqOptions();
        configuration.GetSection(RabbitMqOptions.SectionName).Bind(rabbitOptions);

        services.AddMassTransit(x =>
        {
            // Đăng ký consumer
            x.AddConsumers(AssemblyReference.Assembly);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitOptions.Host, (ushort)rabbitOptions.Port, rabbitOptions.VirtualHost, h =>
                {
                    h.Username(rabbitOptions.Username);
                    h.Password(rabbitOptions.Password);

                    if (rabbitOptions.UseSsl)
                    {
                        h.UseSsl(s =>
                        {
                            s.Protocol = System.Security.Authentication.SslProtocols.Tls12;
                        });
                    }
                });

                cfg.ConfigureEndpoints(context);
                
                cfg.ReceiveEndpoint("product-events-queue", e =>
                {
                    e.ConfigureConsumer<ProductCreatedOutboxEventHandler>(context);
                });
            });
        });
        services.AddMediator(cfg =>
        {
            cfg.AddConsumers(AssemblyReference.Assembly);
        });
        
        return services;
    }
    // public static IServiceCollection AddOutboxEventHandlers(this IServiceCollection services, params Assembly[] assemblies)
    // {
    //     services.AddMassTransit(x =>
    //     {
    //         // Đăng ký tất cả handler implement IOutboxEventHandler<>
    //         x.AddConsumers(assemblies, type =>
    //             type.GetInterfaces().Any(i =>
    //                 i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IOutboxEventHandler<>)
    //             )
    //         );
    //
    //         // cấu hình RabbitMQ
    //         x.UsingRabbitMq((context, cfg) =>
    //         {
    //             cfg.Host("rabbitmq", h =>
    //             {
    //                 h.Username("guest");
    //                 h.Password("guest");
    //             });
    //
    //             cfg.ConfigureEndpoints(context);
    //         });
    //     });
    //
    //     return services;
    // }
}