using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace FeatureSettingsLib.Entity;

public abstract class FeatureSettings
{
    internal string DomainName { get; private set; }
    internal string SubDomainName { get; private set; }
    internal string ChannelName { get; private set; }

    protected FeatureSettings()
    {
        if (Attribute.GetCustomAttribute(GetType(), typeof(FeatureAttribute.FeatureAttribute)) is not
            FeatureAttribute.FeatureAttribute attribute)
            throw new ArgumentException("Feature attribute must be defined at the class level");

        DomainName = attribute.Name;
        SubDomainName = attribute.SubdomainName;
        ChannelName = attribute.ChannelName;
    }

    public void SetDomainName(string name) => DomainName = name;
    public void SetSubDomainName(string name) => SubDomainName = name;
    public void SetChannelName(string name) => ChannelName = name;

    /// <summary>
    /// Returns the JSON Schema string of the Feature Attribute Settings class
    /// </summary>
    /// <seealso cref="https://www.newtonsoft.com/jsonschema/help/html/CreateJsonSchemaWithReferences.htm"/>
    /// <returns></returns>
    public JSchema ToJSchema()
    {
        var enumerableType = GetType().GetNestedTypes().FirstOrDefault(t => t.BaseType == typeof(Enum));

        if (enumerableType != null && enumerableType.CustomAttributes.FirstOrDefault(js =>
                js.AttributeType == typeof(JsonConverterAttribute)) == null)
            throw new JsonException(
                $"{enumerableType.Name} enum is missing [JsonConverter(typeof(StringEnumConverter))] attribute decoration.");

        var generator = new JSchemaGenerator();
        generator.GenerationProviders.Add(new StringEnumGenerationProvider());
        var schema = generator.Generate(GetType());

        return new JSchema
        {
            ExtensionData =
            {
                {
                    DomainName, new JSchema
                    {
                        ExtensionData =
                        {
                            {
                                SubDomainName, schema
                            }
                        }
                    }
                }
            }
        };
    }
}
