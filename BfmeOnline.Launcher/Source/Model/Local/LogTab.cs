using BfmeOnline.Launcher.Interfaces;
using BfmeOnline.Launcher.Source;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BfmeOnline.Launcher.Model
{
    public class LogTab : Tab
    {

        public LogTab(String name, String content)
        {
            Logger.lt = this;
            Name = name;
            Content = content;
        }

        public String TabName
        {
            get
            {
                return Name;
            }
        }

        public String TabContent
        {
            get
            {
                return Content;
            }

            set
            {
                Content = value;
            }
        }
    }
}
