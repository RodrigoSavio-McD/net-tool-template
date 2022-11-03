using ExampleOfProjectOrderValidator.FeatureSettings;
using FeatureSettingsLib.Contract;

namespace ExampleOfProjectOrderValidator;

public class SomeClass
{
    private readonly IFeatureSettingsService<PosOrderValidation> _service;

    public SomeClass(IFeatureSettingsService<PosOrderValidation> service)
    {
        _service = service;
    }

    public string? GetSerializableValue(PosOrderValidation featureSettings)
    {
        var value = _service.Serialize(featureSettings);

        return value;
    }

    public PosOrderValidation? GetDeserializableValue(string featureSettings)
    {
        var value = _service.Deserialize(featureSettings);

        return value;
    }
}
