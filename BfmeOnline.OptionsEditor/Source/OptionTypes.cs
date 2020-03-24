using System.ComponentModel;

namespace BfmeOnline.OptionsEditor
{
    public enum Resolution
    {
        [Description("800x600")]
        res_800x600,
        [Description("1024x768")]
        res_1024x768,
        [Description("1920x1080")]
        res_1920x1080
    }

    public enum Details
    {
        [Description("Low")]
        LOW,
        [Description("Medium")]
        MEDIUM,
        [Description("High")]
        HIGH,
        [Description("Ultra")]
        ULTRA_HIGH
    }

    public enum AudioLOD
    {
        [Description("Low")]
        LOW,
        [Description("Medium")]
        MEDIUM,
        [Description("High")]
        HIGH
    }

    public enum YesNoOption
    {
        [Description("yes")]
        YES,
        [Description("no")]
        NO
    }
}
