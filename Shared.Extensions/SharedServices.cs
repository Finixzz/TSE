
using ImpromptuInterface;

namespace Shared.Extensions;
public interface IConfigurationService
{
    DatabaseConfiguration? GetDatabaseConfiguration<TDatabaseProvider>(
        TDatabaseProvider provider,
        string configuration);

    AuthConfiguration? GetAuthConfiguration(AuthenticationType authType, string configuration);
}

public class ConfigurationService : IConfigurationService
{
    public DatabaseConfiguration? GetDatabaseConfiguration<TDatabaseProvider>(
        TDatabaseProvider provider,
        string configuration)
    {
        return new
        {
            Provider = provider,
            Configuration = configuration
        }.ActLike<DatabaseConfiguration>();
    }

    public AuthConfiguration? GetAuthConfiguration(AuthenticationType authType, string configuration)
    {
        return new
        {
            AuthenticationType = authType,
            Configuration = configuration
        }.ActLike<AuthConfiguration>();
    }
}
