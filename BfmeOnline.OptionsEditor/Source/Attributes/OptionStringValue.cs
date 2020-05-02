using System;

namespace BfmeOnline.OptionsEditor.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class OptionStringValueAttribute : Attribute
    {
        public string StringValue { get; private set; }

        public OptionStringValueAttribute(string value)
        {
            StringValue = value;
        }
    }
}
