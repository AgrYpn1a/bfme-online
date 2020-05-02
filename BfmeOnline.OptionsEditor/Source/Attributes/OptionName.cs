using System;
using System.Collections.Generic;
using System.Text;

namespace BfmeOnline.OptionsEditor.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionNameAttribute : Attribute
    {
        public string StringName { get; private set; }

        public OptionNameAttribute(string name)
        {
            StringName = name;
        }
    }
}
