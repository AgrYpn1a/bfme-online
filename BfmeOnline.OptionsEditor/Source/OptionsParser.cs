using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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

namespace BfmeOnline.OptionsEditor
{
    namespace Attributes
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class OptionIntConstraintAttribute : Attribute
        {
            public int Min { get; private set; }
            public int Max { get; private set; }

            public OptionIntConstraintAttribute(int min, int max)
            {
                Min = min;
                Max = max;
            }
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class OptionName : Attribute
        {
            public string StringName { get; private set; }

            public OptionName(string name)
            {
                StringName = name;
            }
        }
    }

    public sealed class OptionsINI
    {
        [Attributes.OptionName("AllHealthBars")]
        public YesNoOption AllHealthBars { get; set; } = YesNoOption.YES;

        [Attributes.OptionName("AlternateMouseSetup")]
        public YesNoOption AltMouseSetup { get; set; } = YesNoOption.NO;

        [Attributes.OptionName("Resolution ")]
        public Resolution Resolution { get; set; } = Resolution.res_1920x1080;

        // LOD
        [Attributes.OptionName("AudioLOD")]
        public AudioLOD AudioLOD { get; set; } = AudioLOD.HIGH;

        // Volume
        [Attributes.OptionName("AmbientVolume")]
        [Attributes.OptionIntConstraint(0, 100)]
        public int VolAmbient { get; set; } = 50;

        public static T GetAttribute<T>(Object property) where T : Attribute
        {
            return property.GetType().GetCustomAttribute(typeof(T)) as T;
        }

        public static T GetEnumValue<T>(string value) where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList().Find(val => val.ToDescriptionString().Equals(value));
        }
    }

    public static class OptionsParser
    {
        public static OptionsINI GetDefaultConfig()
        {
            return new OptionsINI();
        }
    }

}
