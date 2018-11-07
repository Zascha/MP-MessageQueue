using Autofac;
using MP.WindowsServices.CentralServerNotify;
using MP.WindowsServices.Common.Interfaces;
using MP.WindowsServices.FileStorageObserver.Interfaces;
using MP.WindowsServices.ImagesManager.Interfaces;
using MP.WindowsServices.MQManager;
using MP.WindowsServices.ProcessingBuilder;
using MP.WindowsServices.ProcessingBuilder.Extentions;
using System.Collections.Generic;

namespace MP.WindowsServices.ServiceInstance
{
    internal class ServiceInstance
    {
        private readonly ILifetimeScope _scope;

        public ServiceInstance()
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
            var pdfConvertionStep = _scope.Resolve<IImagesBatchHandler>();
            var publishingStep = _scope.Resolve<IImagesBatchPublisher>();
            var workflowBuilder = _scope.Resolve<IFileStorageWorkflowBuilder>();

            workflowBuilder.StartProcessing(new List<WorkflowStepChain>
            {
                new List<IWorkflowStepExecutor> { pdfConvertionStep, publishingStep }.ToWorkflowStepChain()
            });

            _scope.Resolve<ICentralServerNotifyer>().StartNotify();
        }

        private void StopService()
        {
            _scope.Resolve<IFileStorageObserver>().StopObserving();
            _scope.Resolve<ICentralServerNotifyer>().StopNotify();
            _scope.Resolve<RabbitMQChannel>().Dispose();
        }

        #endregion
    }
}
