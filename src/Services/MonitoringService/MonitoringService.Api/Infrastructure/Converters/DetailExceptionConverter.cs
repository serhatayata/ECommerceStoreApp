﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace MonitoringService.Api.Infrastructure.Converters;

public class DetailExceptionConverter : JsonConverter<Exception>
{
    public override bool CanRead => false;
    public override bool CanWrite => true;

    public override Exception? ReadJson(JsonReader reader, Type objectType, Exception? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
    }

    public override void WriteJson(JsonWriter writer, Exception? value, JsonSerializer serializer)
    {
        var exceptionType = value?.GetType();
        var type = value?.GetType();

        JObject jObject = new();
        jObject["ClassName"] = type?.Name;

        var properties = type != null ?
            type.GetProperties()
                .Where(e => e.PropertyType != typeof(Type))
                .Where(e => e.PropertyType.Namespace != typeof(MemberInfo).Namespace)
                .ToList() : null;

        foreach (var property in properties)
        {
            try
            {
                jObject.AddFirst(new JProperty(property.Name, property.GetValue(value, null)));
            }
            catch (Exception ex)
            {
                continue;
            }
        }

        jObject.WriteTo(writer);
    }
}
