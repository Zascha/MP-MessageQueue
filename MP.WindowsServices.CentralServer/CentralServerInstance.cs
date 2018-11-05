using Autofac;
using MP.WindowsServices.CentralServerListener;

namespace MP.WindowsServices.CentralServer
{
    internal class CentralServerInstance
    {
        private readonly ILifetimeScope _scope;

        public CentralServerInstance()
        {
            _scope = DependencyResolver.DependencyResolver.Container.BeginLifetimeScope();
        }

        public void Start()
        {
            RunService();
        }

        public void Stop()
        {
            StopService();
            _scope.Dispose();
        }

        #region Private methods

        private void RunService()
        {
            _scope.Resolve<IImagesBatchSubscriber>().StartListening();
        }

        private void StopService()
        {
        }

        #endregion
    }
}
