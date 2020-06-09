using BfmeOnline.Launcher.logger;
using BfmeOnline.Launcher.logger.model;
using BfmeOnline.Launcher.Source.logger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using static BfmeOnline.Launcher.Source.logger.Logger;

namespace BfmeOnline.Launcher.ViewModel
{
    public class LogWindowViewModel
    {
        /// <summary>
        /// add new tabs here, and pass the name and content in order
        /// LogTab(String name, String content) heres the info here so you dont have to dig thru files
        /// </summary>
        private readonly ObservableCollection<ITab> tabs;

        public LogWindowViewModel()
        {
            tabs = new ObservableCollection<ITab>();
            tabs.CollectionChanged += Tabs_CollectionChanged;
            Tabs = tabs;
            Logger.lwvm = this;

            foreach (LogType lg in Enum.GetValues(typeof(LogType)))
                Tabs.Add(new LogTab(Logger.logNames[lg], ""));
        }

        private void Tabs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ITab tab;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    tab = (ITab)e.NewItems[0];
                    tab.CloseRequested += OnTabCloseRequested;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    tab = (ITab)e.OldItems[0];
                    tab.CloseRequested -= OnTabCloseRequested;
                    break;
            }
        }

        private void OnTabCloseRequested(object sender, EventArgs e)
        {
            Tabs.Remove((ITab)sender);
        }

        public ICollection<ITab> Tabs { get; }
    }
}
