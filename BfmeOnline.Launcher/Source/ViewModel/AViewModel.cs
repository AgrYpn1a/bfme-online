using BfmeOnline.Launcher.Source.model;

namespace BfmeOnline.Launcher.Source.viewmodel
{
    public abstract class AViewModel<VM, M> where VM : AViewModel<VM, M>
    {
        protected M m_model { get; set; }

        public AViewModel(System.Windows.Window window)
        {
            m_model = GetModel();
            window.DataContext = m_model;
        }

        protected abstract M GetModel();
    }
}
