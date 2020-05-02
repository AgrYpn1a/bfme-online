using System.ComponentModel;

namespace BfmeOnline.OptionsEditor
{
    public enum Resolution
    {
        [Attributes.OptionStringValue("800 600")]
        [Description("800x600")]
        res_800x600,
        [Attributes.OptionStringValue("1024 768")]
        [Description("1024x768")]
        res_1024x768,
        [Attributes.OptionStringValue("1920 1080")]
        [Description("1920x1080")]
        res_1920x1080
    }

    public enum Details
    {
        [Attributes.OptionStringValue("Low")]
        [Description("Low")]
        LOW,
        [Attributes.OptionStringValue("Medium")]
        [Description("Medium")]
        MEDIUM,
        [Attributes.OptionStringValue("High")]
        [Description("High")]
        HIGH,
        [Attributes.OptionStringValue("UltraHigh")]
        [Description("Ultra")]
        ULTRA_HIGH
    }

    public enum AudioLOD
    {
        [Attributes.OptionStringValue("Low")]
        [Description("Low")]
        LOW,
        [Attributes.OptionStringValue("Medium")]
        [Description("Medium")]
        MEDIUM,
        [Attributes.OptionStringValue("High")]
        [Description("High")]
        HIGH
    }

    public enum YesNoOption
    {
        [Attributes.OptionStringValue("yes")]
        [Description("Yes")]
        YES,
        [Attributes.OptionStringValue("no")]
        [Description("No")]
        NO
    }
}
