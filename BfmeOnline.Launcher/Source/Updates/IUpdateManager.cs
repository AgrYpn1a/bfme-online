using System.Threading.Tasks;

namespace BfmeOnline.Launcher.Source.Updates
{
    interface IUpdateManager
    {


        Task<bool> HasUpdates();

        Task Update();

    }
}
