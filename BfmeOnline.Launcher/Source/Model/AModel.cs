using BfmeOnline.Launcher.Source.viewmodel;
using System.ComponentModel;

namespace BfmeOnline.Launcher.Source.model
{
    public abstract class AModel<M, VM> : INotifyPropertyChanged
    {
        protected VM m_viewModel;

        public AModel(VM viewModel)
        {
            m_viewModel = viewModel;
        }

        public abstract event PropertyChangedEventHandler PropertyChanged;
    }
}
