using System;
using System.Collections.Generic;
using System.Text;

namespace BfmeOnline.OptionsEditor.Attributes
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
}
