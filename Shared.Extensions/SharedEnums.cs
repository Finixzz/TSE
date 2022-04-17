namespace Shared.Extensions;

public enum RelationalDatabaseProvider
{
    MicrosoftSQLServer,
    MySQL,
    PostgreSQL
}

public enum NonRelationalDatabaseProvider
{
    MongoDB
}

public enum AuthenticationType
{
    Bearer,
}