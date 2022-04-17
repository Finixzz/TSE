using ImpromptuInterface;

namespace Shared.Extensions;

public interface IConfigurationService
{
    DatabaseConfiguration<TProvider>? GetDatabaseConfiguration<TProvider>(
        TProvider provider,
        string configuration)
        where TProvider : Enum;

    AuthConfiguration? GetAuthConfiguration(AuthenticationType authType, string configuration);
}

public class ConfigurationService : IConfigurationService
{
    public AuthConfiguration? GetAuthConfiguration(AuthenticationType authType, string configuration)
    {
        if (configuration is null)
            throw new ArgumentException(nameof(configuration));

        return new
        {
            AuthenticationType = authType,
            Configuration = configuration
        }.ActLike<AuthConfiguration>();
    }

    public DatabaseConfiguration<TProvider>? GetDatabaseConfiguration<TProvider>(TProvider provider, string configuration)
        where TProvider : Enum
    {
        if (provider.GetType() != typeof(RelationalDatabaseProvider) && provider.GetType() != typeof(NonRelationalDatabaseProvider))
            throw new ArgumentException(nameof(provider));

        if (configuration is null)
            throw new ArgumentException(nameof(configuration));

        return new
        {
            Provider = provider,
            Configuration = configuration
        }.ActLike<DatabaseConfiguration<TProvider>>();
    }
}