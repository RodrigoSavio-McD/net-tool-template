using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FeatureSettingsLib.FeatureAttribute;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExampleOfProjectOrderValidator.FeatureSettings;

[Feature(Name = "Order", SubdomainName = "OrderDocument", ChannelName = "POS")]
public class PosOrderValidation : FeatureSettingsLib.Entity.FeatureSettings
{
    /// <summary>
    /// Feature Settings for Order Validation rules for POS Registers where crew is taking orders.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ReductionValidationModes
    {
        Off,
        Limits,
        EachReduction,
        EachReductionAfterLimit
    }

    /// <summary>
    /// This parameter defines the sales quantity limit for a single item in an order for requiring manager authorization to proceed. This parameter is also known as HILO (High Item Limit Order). If this authorization process is not desired, place a very high value in the HILO parameter. This parameter is mandatory and requires a valid value
    /// </summary>
    [Required]
    [DefaultValue(10)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    [Display(Name = "HighQuantitySaleLimit",
        Description =
            "This parameter defines the sales quantity limit for a single item in an order for requiring manager authorization to proceed. This parameter is also known as HILO (High Item Limit Order). If this authorization process is not desired, place a very high value in the HILO parameter. This parameter is mandatory and requires a valid value.")]
    public int HighQuantitySaleLimit { get; set; }

    /// <summary>
    /// This parameter indicates the maximum number of gift cards that can be purchased in the same order. The default value for this parameter is \"99\". Any positive number is accepted as valid value.
    /// </summary>
    [Required]
    [DefaultValue(99)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    [Display(Name = "HighQuantityLimit",
        Description =
            "This parameter indicates the maximum number of gift cards that can be purchased in the same order. The default value for this parameter is \"99\". Any positive number is accepted as valid value.")]
    public int HighQuantityLimit { get; set; }

    /// <summary>
    /// "This parameter defines the sales amount limit (in pennies) of an order for requiring manager authorization to proceed. This parameter is also known as \"HALO\" (High Amount Limit Order). If this authorization process is not desired, place a very high value in the HALO parameter. This parameter is mandatory and requires a valid value.
    /// </summary>
    [Required]
    [DefaultValue(50000)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    [Display(Name = "HighAmountSaleLimit",
        Description =
            "This parameter defines the sales amount limit (in pennies) of an order for requiring manager authorization to proceed. This parameter is also known as \"HALO\" (High Amount Limit Order). If this authorization process is not desired, place a very high value in the HALO parameter. This parameter is mandatory and requires a valid value.")]
    public int HighAmountSaleLimit { get; set; }

    /// <summary>
    /// The default value for this parameter is \"Off\" = Reduction is always allowed. No manager authorization will be required (default). Limits = Reduction is allowed according to the values set in the parameters <TRedBeforeTotal>, <TRedAfterTotalAmount> and <TRedAfterTotalQuantity>. Manager authorization is required when the values set are reached. EachReduction = Manager authorization is required every time a reduction is attempted. EachReductionAfterLimit =  Manager authorization is required when a reduction limit is reached and for all subsequent reductions.
    /// </summary>
    [Required]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    [DefaultValue(ReductionValidationModes.Off)]
    [Display(Name = "ReductionValidationMode",
        Description =
            "The default value for this parameter is \"Off\" = Reduction is always allowed. No manager authorization will be required (default). Limits = Reduction is allowed according to the values set in the parameters <TRedBeforeTotal>, <TRedAfterTotalAmount> and <TRedAfterTotalQuantity>. Manager authorization is required when the values set are reached. EachReduction = Manager authorization is required every time a reduction is attempted. EachReductionAfterLimit =  Manager authorization is required when a reduction limit is reached and for all subsequent reductions.")]
    public ReductionValidationModes ReductionValidationMode { get; set; }

    [Required]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    [DefaultValue(0)]
    [Display(Name = "TRedAmountBeforeTotal",
        Description =
            "This parameter defines the amount of items reduction that can be made without the manager authorization for POS; i.e., this parameter limits the amount of voided items using the Void Line Function. If the amount exceeds the value configured for the parameter, the application will require the manager's authorization in order to continue the operation. If the parameter is configured with the value \"0\", the application will not perform the verification. The default value for this parameter is \"0\".")]
    public int TRedAmountBeforeTotal { get; set; }

    [Required]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    [DefaultValue(10000)]
    [Display(Name = "TRedBeforeTotal",
        Description =
            "This parameter defines the limit for the number of items that can be reduced from an order before going to the \"Total\" or \"Tender\" screens. When this limit is reached, a manager authorization will be necessary to proceed. If set to \"0\", no validation will be performed. This parameter should be included in the xml file, however it will only be used if the <ReductionValidationMode> parameter is set to \"Limits\".")]
    public int TRedBeforeTotal { get; set; }
}
