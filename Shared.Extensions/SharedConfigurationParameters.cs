namespace Shared.Extensions;

public interface DatabaseConfiguration
{
    public object? Provider { get; }

    public string Configuration { get; }
}

public interface RelationalDatabaseConfiguration : DatabaseConfiguration
{
    public new RelationalDatabaseProvider? Provider { get; }
}

public interface NonRelationalDatabaseConfiguration : DatabaseConfiguration
{
    public new NonRelationalDatabaseProvider? Provider { get; }
}


public interface AuthConfiguration
{
    public AuthenticationType? AuthenticationType { get; }

    public string Configuration { get; }
}

