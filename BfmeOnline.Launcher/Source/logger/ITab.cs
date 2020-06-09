using BfmeOnline.Launcher.Commands;
using System;
using System.Windows.Input;

namespace BfmeOnline.Launcher.logger
{
    public interface ITab
    {
        string Name { get; set; }

        ICommand CloseCommand { get; }

        event EventHandler CloseRequested;
    }

    public abstract class Tab : ITab
    {
        public Tab()
        {
            CloseCommand = new ActionCommand(p => CloseRequested?.Invoke(this, EventArgs.Empty));
        }

        public string Name { get; set; }

        public string Content { get; set; }

        public ICommand CloseCommand { get; }

        public event EventHandler CloseRequested;
    }
}
