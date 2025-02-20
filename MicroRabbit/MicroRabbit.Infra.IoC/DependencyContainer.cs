using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MicroRabbit.Infra.IoC;

public static class DependencyContainer
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BankingDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("BankingDbConnection"));
        });

        //Domain Bus
        services.AddTransient<IEventBus, RabbitMQBus>();

        //Application Services
        services.AddTransient<IAccountService, AccountService>();

        //Data
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<BankingDbContext>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        return services;
    }
}
