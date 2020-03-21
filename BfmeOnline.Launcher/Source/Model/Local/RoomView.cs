using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace BfmeOnline.Launcher.Model
{
    public class RoomView : INotifyPropertyChanged
    {
        private String _RoomName;

        private ObservableCollection<RoomView> _RoomViewCollection = new ObservableCollection<RoomView>();

        public ObservableCollection<RoomView> RoomViewCollection
        {
            get
            {
                return _RoomViewCollection;
            }
        }

        public void AddToCollection(RoomView rv)
        {
            _RoomViewCollection.Add(rv);
            OnPropertyChanged("RoomViewCollection");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public String RoomName
        {
            get
            {
                return _RoomName;
            }
            set
            {
                _RoomName = value;
                OnPropertyChanged("RoomName");

            }
        }
        /// <summary>
        /// logiku cu morati udesiti s tobom, ne znam kako zelis, i kako je moguce igrati
        /// </summary>
        private String _Capacity;
        public String Capacity
        {
            get
            {
                return _Capacity;
            }
            set
            {
                _Capacity = value;
                OnPropertyChanged("Capacity");
            }
        }
        private String _Description;
        public String Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                OnPropertyChanged("Description");
            }
        }

        public RoomView(RoomView rw)
        {
            RoomName = rw.RoomName;
            Capacity = rw.Capacity;
            Description = rw.Description;
        }

        public RoomView()
        {
            RoomName = "";
            Capacity = "";
            Description = "";
        }

    }
}
