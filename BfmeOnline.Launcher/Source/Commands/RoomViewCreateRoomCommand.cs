using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using BfmeOnline.Launcher.ViewModel;

namespace BfmeOnline.Launcher.Commands
{
    internal class RoomViewCreateRoomCommand : ICommand
    {
        public RoomViewCreateRoomCommand(RoomViewViewModel viewModel)
        {
            _ViewModel = viewModel;
        }

        private RoomViewViewModel _ViewModel;

        /// <summary>
        /// wires back to the wpf command system(implementing interface directly, not deriving from someething like routed command)
        /// </summary>
        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        /// <summary>
        /// button will enable or disable itself according to the return of this method
        /// buissness logic will be implemented according to the requirements of the project
        /// for now ill just return viewModel
        /// </summary>
      
        public bool CanExecute(object parameter)
        {
            return _ViewModel.CanCreateRoom;
        }

        public void Execute(object parameter)
        {
            _ViewModel.StartVpn();
        }
    }
}
