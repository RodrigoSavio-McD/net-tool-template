using Newtonsoft.Json;

namespace FeatureSettingsLib.Contract;

public class FeatureSettingsService<T> : IFeatureSettingsService<T> where T : class
{
    public string Serialize(T serviceFeatureSettings)
    {
        try
        {
            var value = JsonConvert.SerializeObject(serviceFeatureSettings,
                new FeatureSettingsConverter(typeof(T)));

            return value;
        }
        catch (Exception e)
        {
            // check possible exceptions to address them correctly
            // log issue
            Console.WriteLine(e);
            throw;
        }
    }

    public T? Deserialize(string serializedValue)
    {
        try
        {
            if (string.IsNullOrEmpty(serializedValue))
                throw new ArgumentNullException(serializedValue);

            var value = JsonConvert.DeserializeObject<T>(serializedValue,
                new FeatureSettingsConverter(typeof(T)));

            return value;
        }
        catch (Exception e)
        {
            // check possible exceptions to address them correctly
            // log issue
            Console.WriteLine(e);
            throw;
        }
    }
}
