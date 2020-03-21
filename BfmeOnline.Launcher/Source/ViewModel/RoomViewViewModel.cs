using BfmeOnline.Launcher.Model;
using System;
using System.Windows.Input;
using BfmeOnline.Launcher.Commands;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Diagnostics;

namespace BfmeOnline.Launcher.ViewModel
{
    internal class RoomViewViewModel
    {
        private App _appGetter = (App)Application.Current;
        /// <summary>
        /// We will populate RoomView with data from the server
        /// this is the place meant for that!!!
        /// </summary>
        public RoomViewViewModel()
        {
            CreateRoomCommand = new RoomViewCreateRoomCommand(this);
            _RoomView = new RoomView();
        }

        public RoomViewViewModel(MainWindowViewModel mwvm)
        {
            CreateRoomCommand = new RoomViewCreateRoomCommand(this);
            _RoomView = new RoomView();
            _MWVM = mwvm;

        }

        private MainWindowViewModel _MWVM;

        public MainWindowViewModel MVWM
        {
            get
            {
                return _MWVM;

            }
        }
        private RoomView _RoomView;
        public RoomView RoomView
        {
            get
            {
                return _RoomView;
            }
        }


        /// <summary>
        /// starts the vpn command
        /// </summary>
        public void StartVpn()
        {
            string error = "";
            try
            {
                //MainWindow mw = (MainWindow)Application.Current.MainWindow;
                //mw.RoomViewItems.Add(RoomView);
                //mw.roomViewList.ItemsSource = mw.RoomViewItems;
                _appGetter.vpnProcess.ExecuteCommand("START", (res) => { MessageBox.Show(res); });
            }
            catch (Exception err)
            {

            }

            if (!error.Equals(""))
            {
                MessageBox.Show("Грешка волино!");
            }
        }

        public bool CanCreateRoom
        {
            get
            {
                if (RoomView == null)
                    return false;
                return !String.IsNullOrWhiteSpace(RoomView.RoomName);
            }
        }

        public ICommand CreateRoomCommand
        {
            get;
            private set;
        }
    }

}
