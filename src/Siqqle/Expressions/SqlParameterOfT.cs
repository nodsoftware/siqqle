using System;

namespace Siqqle.Expressions;

/// <summary>
/// Represents a named parameter in SQL with a provider-specific database type.
/// This type is used for database-specific types such as PostgreSQL's JSONB, SQL Server's XML, etc.
/// </summary>
/// <typeparam name="TDbType">
/// The provider-specific database type enum (e.g., NpgsqlDbType, SqlDbType, MySqlDbType).
/// Must be a struct enum type.
/// </typeparam>
public class SqlParameter<TDbType> : SqlParameter
    where TDbType : struct, Enum
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SqlParameter{TDbType}"/> class using
    /// the specified parameter name, provider-specific type, and value.
    /// </summary>
    /// <param name="parameterName">
    /// The name of the parameter.
    /// </param>
    /// <param name="providerDbType">
    /// The provider-specific database type enum value.
    /// </param>
    /// <param name="value">
    /// The value of the parameter.
    /// </param>
    public SqlParameter(string parameterName, TDbType providerDbType, object value)
        : base(parameterName, value)
    {
        ProviderDbType = providerDbType;
    }

    /// <summary>
    /// Gets the provider-specific database type for this parameter.
    /// </summary>
    public TDbType ProviderDbType { get; }
}
