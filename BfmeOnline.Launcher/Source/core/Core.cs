using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BfmeOnline.Launcher.Source.core
{

    public interface ICoreOperation<T>
    {
        Task<T> Do();

        virtual string OpName() => "Default";
    }

    //public interface ICoreCondition : ICoreOperation { }

    public interface IOpResult { }

    public class NullOpResult : IOpResult { }

    //public class Execute : ICoreOperation<NullOpResult>
    //{
    //    public Execute(Action action)
    //    {
    //        action?.Invoke();
    //    }

    //    public async Task<NullOpResult> Do()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class Branch : ICoreOperation
    //{
    //    public Branch(Predicate<ICoreOperation> predicate, Action actionSatisfied, Action actionOtherwise)
    //    {

    //    }
    //}

    /// <summary>
    /// For test purpose
    /// </summary>
    public class LogOp : ICoreOperation<NullOpResult>
    {
        public async Task<NullOpResult> Do()
        {
            int random = new Random().Next(5000);
            await Task.Delay(random);

            return new NullOpResult();
        }
    }

    public enum CoreState
    {
        HALT = 0,
        RUNNING = 1
    }

    public enum Game
    {
        Bfme1,
        Bfme2,
        BfmeRotwk
    }

    /// <summary>
    /// Singleton, launcher's main class.
    /// </summary>
    public sealed class Core
    {
        /** Events */
        public delegate void LauncherStateChange(LauncherState newState);
        public event LauncherStateChange OnLauncherStateChange;

        private static readonly Core _instance = new Core();
        public static Core Instance { get => _instance; }

        /** States */
        public LauncherState LauncherState { get; private set; } = LauncherState.Default;
        public CoreState State { get; set; } = CoreState.HALT;

        private Queue<ICoreOperation<IOpResult>> _operations = new Queue<ICoreOperation<IOpResult>>();
        private ICoreOperation<IOpResult> _currentOp = null;

        public void Run(Queue<ICoreOperation<IOpResult>> newOps)
        {
            // If there are none, just exit
            if (newOps == null || newOps.Count == 0) return;

            // Add new operations to the queue
            while (newOps.Count > 0)
            {
                _operations.Enqueue(newOps.Dequeue());
            }

            if (State == CoreState.HALT)
            {
                _currentOp = _operations.Dequeue();
                _currentOp.Do();
            }
        }

        public void ChangeState(LauncherState newState)
        {
            LauncherState = newState;
            OnLauncherStateChange?.Invoke(newState);
        }

        /// <summary>
        /// Not all games are supported right away. Use this to make
        /// sure that the game is currently supported.
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public bool IsGameSupported(Game game) => game == Game.Bfme1;

        /// <summary>
        /// Checks if the given game is installed.
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public bool IsGameInstalled(Game game)
        {
            return RegistryManager.IsGameInstalled();
        }
    }
}
