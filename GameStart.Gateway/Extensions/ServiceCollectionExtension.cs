namespace GameStart.Gateway.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddAllowingEverythingCors(this IServiceCollection services)
        {
            return services.AddCors(options => options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
        }
    }
}
