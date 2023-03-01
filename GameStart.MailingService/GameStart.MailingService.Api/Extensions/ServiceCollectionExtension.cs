using GameStart.MailingService.Common.Consumers;
using GameStart.MailingService.Common.Services;
using GameStart.MailingService.Common.Settings;
using GameStart.Shared;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using MassTransit;

namespace GameStart.MailingService.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddSingleton<IEmailService, EmailService>();
        }

        public static IServiceCollection AddPreconfiguredHangfire(this IServiceCollection services)
        {
            services.AddHangfire(options =>
            {
                options.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                options.UseSimpleAssemblyNameTypeSerializer();
                options.UseRecommendedSerializerSettings();
                options.UseMongoStorage(Constants.MailingService.ConnectionStrings.HangfireDb, new MongoStorageOptions()
                {
                    MigrationLockTimeout = TimeSpan.FromMinutes(5),
                    InvisibilityTimeout = TimeSpan.FromMinutes(30),
                    CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection,
                    MigrationOptions = new()
                    {
                        MigrationStrategy = new MigrateMongoMigrationStrategy(),
                        BackupStrategy = new CollectionMongoBackupStrategy()
                    }
                });
            });

            return services.AddHangfireServer();
        }

        public static IServiceCollection AddEmailConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
        }

        public static IServiceCollection AddMassTransitEventConsuming(this IServiceCollection services)
        {
            return services.AddMassTransit(options =>
            {
                options.AddConsumer<EmailVerificationConsumer>();

                options.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(Constants.MessageBus.RabbitMQRoot, host =>
                    {
                        host.Username(Constants.MessageBus.Username);
                        host.Password(Constants.MessageBus.Password);
                    });

                    configurator.ConfigureEndpoints(context);
                });
            });
        }
    }
}
