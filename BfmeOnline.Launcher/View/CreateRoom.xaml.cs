using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BfmeOnline.Launcher.ViewModel;

namespace BfmeOnline.Launcher
{
    /// <summary>
    /// Interaction logic for CreateRoom.xaml
    /// </summary>


    public partial class CreateRoom : Window
    {
        
        
        public CreateRoom()
        {
            InitializeComponent();
            DataContext = new RoomViewViewModel();


            

        }
    }
}
