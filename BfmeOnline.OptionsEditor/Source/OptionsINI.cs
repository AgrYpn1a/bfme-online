using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BfmeOnline.OptionsEditor
{

    /**
     * Options.ini for Bfme1
     * =======================
 
        AllHealthBars = yes
        AlternateMouseSetup = no
        AmbientVolume = 0
        AudioLOD = High
        Brightness = 50
        FirewallBehavior = 1
        FirewallNeedToRefresh = FALSE
        FirewallPortAllocationDelta = 0
        FirewallPortOverride = 16000
        FixedStaticGameLOD = UltraHigh
        FlashTutorial = 0
        HeatEffects = yes
        IdealStaticGameLOD = High
        IsThreadedLoad = yes
        MovieVolume = 0
        MusicVolume = 0
        Resolution = 1600 1200
        SFXVolume = 0
        ScrollFactor = 38
        StaticGameLOD = UltraHigh
        TimesInGame = 153
        UnitDecals = no
        UseEAX3 = no
        VoiceVolume = 0
    */

    public sealed class OptionsINI
    {
        [Attributes.OptionNameAttribute("AllHealthBars")]
        public YesNoOption ShowAllHealthBars { get; set; } = YesNoOption.YES;

        [Attributes.OptionNameAttribute("UnitDecals")]
        public YesNoOption ShowUnitDecals { get; set; } = YesNoOption.NO;

        [Attributes.OptionNameAttribute("FPSLimit")]
        public YesNoOption FPSLimit { get; set; } = YesNoOption.NO;

        [Attributes.OptionNameAttribute("AlternateMouseSetup")]
        public YesNoOption AltMouseSetup { get; set; } = YesNoOption.NO;


        [Attributes.OptionNameAttribute("Resolution")]
        public Resolution Resolution { get; set; } = Resolution.res_1920x1080;

        [Attributes.OptionNameAttribute("Brightness")]
        [Attributes.OptionIntConstraint(0, 100)]
        public int Brightness { get; set; } = 50;

        [Attributes.OptionNameAttribute("ScrollFactor")]
        [Attributes.OptionIntConstraint(0, 100)]
        public int ScrollSpeed { get; set; } = 50;

        // LOD
        [Attributes.OptionNameAttribute("AudioLOD")]
        public AudioLOD AudioLOD { get; set; } = AudioLOD.HIGH;

        [Attributes.OptionNameAttribute("StaticGameLOD")]
        public Details GeneralDetails { get; set; } = Details.ULTRA_HIGH;

        // Volume
        [Attributes.OptionNameAttribute("AmbientVolume")]
        [Attributes.OptionIntConstraint(0, 100)]
        public int VolAmbient { get; set; } = 50;

        [Attributes.OptionNameAttribute("VoiceVolume")]
        [Attributes.OptionIntConstraint(0, 100)]
        public int VolVoice { get; set; } = 50;

        [Attributes.OptionNameAttribute("MovieVolume")]
        [Attributes.OptionIntConstraint(0, 100)]
        public int VolMovie { get; set; } = 50;

        [Attributes.OptionNameAttribute("MusicVolume")]
        [Attributes.OptionIntConstraint(0, 100)]
        public int VolMusic { get; set; } = 50;

        [Attributes.OptionNameAttribute("SFXVolume")]
        [Attributes.OptionIntConstraint(0, 100)]
        public int VolSFX { get; set; } = 50;

        [Attributes.OptionNameAttribute("UseEAX3")]
        public YesNoOption UseEAX { get; set; } = YesNoOption.NO;

        public static T GetAttribute<T>(string propName) where T : Attribute
        {
            var a = typeof(OptionsINI)
                .GetProperty(propName)
                .GetCustomAttribute(typeof(T)) as T;

            return a;
        }

        public static T GetEnumAttribute<T>(Enum enumValue) where T : Attribute
        {
            return enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<T>() as T;
        }

        public static T GetEnumValue<T>(string value) where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList().Find(val => val.ToDescriptionString().Equals(value));
        }

        public Dictionary<string, string> GetOptionsDict()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict.Add(GetAttribute<Attributes.OptionNameAttribute>(nameof(ShowAllHealthBars)).StringName, GetEnumAttribute<Attributes.OptionStringValueAttribute>(ShowAllHealthBars).StringValue);
            dict.Add(GetAttribute<Attributes.OptionNameAttribute>(nameof(ShowUnitDecals)).StringName, GetEnumAttribute<Attributes.OptionStringValueAttribute>(ShowUnitDecals).StringValue);
            dict.Add(GetAttribute<Attributes.OptionNameAttribute>(nameof(AltMouseSetup)).StringName, GetEnumAttribute<Attributes.OptionStringValueAttribute>(AltMouseSetup).StringValue);
            dict.Add(GetAttribute<Attributes.OptionNameAttribute>(nameof(Resolution)).StringName, GetEnumAttribute<Attributes.OptionStringValueAttribute>(Resolution).StringValue);
            dict.Add(GetAttribute<Attributes.OptionNameAttribute>(nameof(Brightness)).StringName, Brightness.ToString());
            dict.Add(GetAttribute<Attributes.OptionNameAttribute>(nameof(ScrollSpeed)).StringName, ScrollSpeed.ToString());
            dict.Add(GetAttribute<Attributes.OptionNameAttribute>(nameof(AudioLOD)).StringName, GetEnumAttribute<Attributes.OptionStringValueAttribute>(AudioLOD).StringValue);
            dict.Add(GetAttribute<Attributes.OptionNameAttribute>(nameof(GeneralDetails)).StringName, GetEnumAttribute<Attributes.OptionStringValueAttribute>(GeneralDetails).StringValue);
            dict.Add(GetAttribute<Attributes.OptionNameAttribute>(nameof(VolAmbient)).StringName, VolAmbient.ToString());
            dict.Add(GetAttribute<Attributes.OptionNameAttribute>(nameof(VolVoice)).StringName, VolVoice.ToString());
            dict.Add(GetAttribute<Attributes.OptionNameAttribute>(nameof(VolMusic)).StringName, VolMusic.ToString());
            dict.Add(GetAttribute<Attributes.OptionNameAttribute>(nameof(VolSFX)).StringName, VolSFX.ToString());
            dict.Add(GetAttribute<Attributes.OptionNameAttribute>(nameof(UseEAX)).StringName, GetEnumAttribute<Attributes.OptionStringValueAttribute>(UseEAX).StringValue);
            dict.Add(GetAttribute<Attributes.OptionNameAttribute>(nameof(FPSLimit)).StringName, GetEnumAttribute<Attributes.OptionStringValueAttribute>(FPSLimit).StringValue);

            // Hardcoded options
            dict.Add("FirewallBehavior", "1");
            dict.Add("FirewallNeedToRefresh", "FALSE");
            dict.Add("FirewallPortAllocationDelta", "0");
            dict.Add("FirewallPortOverride", "16000");
            dict.Add("FlashTutorial", "0");
            dict.Add("IsThreadedLoad", "yes");

            dict.Add("ModelLOD", "Medium");
            dict.Add("HasSeenLogoMovies", "yes");
            dict.Add("IdealStaticGameLOD", "UltraHigh");

            return dict;
        }
    }
}
