namespace Shared.Extensions;

public interface DatabaseConfiguration<TProvider>
    where TProvider : Enum
{
    public TProvider Provider { get; }

    public string Configuration { get; }
}

public interface AuthConfiguration
{
    public AuthenticationType AuthenticationType { get; }

    public string Configuration { get; }
}