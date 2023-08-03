using System.ComponentModel.DataAnnotations;

namespace IntegrationEventLogEF;

public abstract class BaseIntegrationEventLogContext : DbContext
{
    public BaseIntegrationEventLogContext(DbContextOptions<IntegrationEventLogContext> options) : base(options)
    {
        
    }
    public virtual string GetTableNameWithScheme<T>() where T : class
    {
        var entityType = Model.FindEntityType(typeof(T));
        var schema = entityType?.GetSchema();
        var tableName = entityType?.GetTableName();
        if (schema == null)
            return entityType?.GetTableName() ?? string.Empty;

        return $"{schema}.{tableName}";
    }

    public virtual string GetKeyColumnName<T>() where T : class
    {
        PropertyInfo[] properties = typeof(T).GetProperties();

        foreach (PropertyInfo property in properties)
        {
            object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

            if (keyAttributes != null && keyAttributes.Length > 0)
            {
                object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                if (columnAttributes != null && columnAttributes.Length > 0)
                {
                    ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                    return columnAttribute?.Name ?? string.Empty;
                }
                else
                {
                    return property.Name;
                }
            }
        }

        return null;
    }

    public virtual string GetColumns<T>(bool excludeKey = false) where T : class
    {
        var type = typeof(T);
        var columns = string.Join(", ", type.GetProperties()
            .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
            .Select(p =>
            {
                var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                return columnAttr != null ? columnAttr.Name : p.Name;
            }));

        return columns;
    }

    public virtual string GetPropertyNames<T>(bool excludeKey = false) where T : class
    {
        var properties = typeof(T).GetProperties()
            .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

        var values = string.Join(", ", properties.Select(p =>
        {
            return $"@{p.Name}";
        }));

        return values;
    }

    public virtual IEnumerable<PropertyInfo> GetProperties<T>(bool excludeKey = false) where T : class
    {
        var properties = typeof(T).GetProperties()
            .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

        return properties;
    }

    public virtual string GetKeyPropertyName<T>()
    {
        var properties = typeof(T).GetProperties()
            .Where(p => p.GetCustomAttribute<KeyAttribute>() != null);

        if (properties.Any())
        {
            return properties?.FirstOrDefault()?.Name ?? string.Empty;
        }

        return null;
    }
}
