using System.Globalization;
using FeatureSettingsLib.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FeatureSettingsLib;

public class FeatureSettingsConverter : JsonConverter
{
    private readonly Type[] _types;

    public FeatureSettingsConverter(params Type[] types)
    {
        _types = types;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value), "Value object cannot be null");

        var token = JToken.FromObject(value);

        if (token.Type != JTokenType.Object)
            token.WriteTo(writer);
        else
        {
            var featureSettings = (FeatureSettings)value;
            var content = new JObject { new JProperty(featureSettings.SubDomainName, token) };
            var parent = new JObject(new JProperty(new JProperty(featureSettings.DomainName, content)));

            parent.WriteTo(writer);
        }
    }

    public override object ReadJson(JsonReader reader,
        Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jsonObject = (JToken)JObject.Load(reader);

        if (Activator.CreateInstance(objectType) is not FeatureSettings setting)
            throw new ArgumentNullException(nameof(objectType), "Object type cannot be null");

        IterateJsonObjectAndPopulateFeatureSettingsObject(objectType, jsonObject, setting);

        return setting;
    }

    private static void IterateJsonObjectAndPopulateFeatureSettingsObject(Type objectType, JToken jsonObject,
        FeatureSettings setting)
    {
        foreach (var domain in jsonObject)
        {
            var property = (JProperty)domain;

            setting.SetDomainName(property.Name);
            var jToken = property.Value;

            IterateOverSubDomainProperties(objectType, setting, jToken);
        }
    }

    private static void IterateOverSubDomainProperties(Type objectType, FeatureSettings setting, JToken jToken)
    {
        foreach (var subDomain in jToken)
        {
            var subProperty = (JProperty)subDomain;
            setting.SetSubDomainName(subProperty.Name);

            IterateOverSubPropertiesValues(objectType, setting, subProperty);
        }
    }

    private static void IterateOverSubPropertiesValues(Type objectType, FeatureSettings setting, JProperty subProperty)
    {
        foreach (var attribute in subProperty.Value)
        {
            var attr = (JProperty)attribute;
            var attributeInfo = objectType.GetProperty(attr.Name);
            var value = attr.Value as JValue;

            if (attributeInfo is null)
                throw new InvalidOperationException("Missing property name in one of the sub properties");

            var type = Nullable.GetUnderlyingType(attributeInfo.PropertyType) ?? attributeInfo.PropertyType;

            if (type is null)
                throw new InvalidOperationException("Unable to identify property type");

            var safeValue = GetValueFromProperty(type, value);

            attributeInfo.SetValue(setting, safeValue, null);
        }
    }

    private static object? GetValueFromProperty(Type type, JValue? value)
    {
        object? safeValue;

        if (type.BaseType == typeof(Enum) && value is not null)
            safeValue = Enum.Parse(type, value.ToString(CultureInfo.InvariantCulture), true);
        else
            safeValue = (value?.Value is null) ? null : Convert.ChangeType(value, type);

        return safeValue;
    }

    public override bool CanRead => true;

    public override bool CanConvert(Type objectType) => _types.Any(t => t == objectType);
}
