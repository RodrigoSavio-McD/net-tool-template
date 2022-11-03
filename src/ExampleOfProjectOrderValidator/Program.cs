using ExampleOfProjectOrderValidator;
using ExampleOfProjectOrderValidator.FeatureSettings;
using FeatureSettingsLib.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

var serviceCollection = new ServiceCollection();

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration config = builder.Build();

serviceCollection
    .AddSingleton<IFeatureSettingsService<PosOrderValidation>, FeatureSettingsService<PosOrderValidation>>();
serviceCollection.AddSingleton(config);
serviceCollection.AddSingleton<SomeClass>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var entryPoint = serviceProvider.GetService<SomeClass>();

if (entryPoint is null)
    throw new InvalidOperationException("EntryPoint cannot be null");

// generating schema
var featureSchema = new PosOrderValidation().ToJSchema().ToString();

var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
var schemaFilePath = string.Concat(path, "\\", "featureSettings.json");

await File.WriteAllTextAsync(schemaFilePath, featureSchema);

Console.WriteLine($"Schema generated at {path}");

var serializableValue = entryPoint.GetSerializableValue(new PosOrderValidation());

var serializableValueFilePath = string.Concat(path, "\\", "serializableValue.json");
await File.WriteAllTextAsync(serializableValueFilePath, serializableValue);

const string value =
    @"{'Order':{'OrderDocument':{'HighQuantitySaleLimit':50,'HighQuantityLimit':0,'HighAmountSaleLimit':90,'ReductionValidationMode':2}}}";

var deserializableValue = entryPoint.GetDeserializableValue(value);

Console.WriteLine(deserializableValue);
