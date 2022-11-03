namespace FeatureSettingsLib.FeatureAttribute;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class FeatureAttribute : Attribute
{
    public string Name { get; set; } = string.Empty;
    public string SubdomainName { get; set; } = string.Empty;
    public string ChannelName { get; set; } = string.Empty;
}
