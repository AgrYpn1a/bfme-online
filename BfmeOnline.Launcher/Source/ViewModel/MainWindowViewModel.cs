using System;
using System.Collections.Generic;
using System.Text;
using BfmeOnline.Launcher.Model;
using BfmeOnline.Launcher.ViewModel;

namespace BfmeOnline.Launcher.ViewModel
{
    internal class MainWindowViewModel
    {
        private RoomViewViewModel _ChildViewModel;
        public MainWindow MW;
        public MainWindowViewModel(MainWindow mw)
        {
            _ChildViewModel = new RoomViewViewModel(this);
            MW = mw;

        }

        public RoomViewViewModel RoomViewViewModel { get { return _ChildViewModel; } }
    }
}
