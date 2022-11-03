namespace FeatureSettingsLib.Contract;

public interface IFeatureSettingsService<T> where T : class
{
    string? Serialize(T serviceFeatureSettings);

    T? Deserialize(string serializedValue);
}
