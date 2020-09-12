using BfmeOnline.Launcher.Source.logger;
using System;

namespace BfmeOnline.Launcher.logger.model
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
