using Autofac;
using MP.WindowsServices.CentralServerNotify;
using MP.WindowsServices.Common;
using MP.WindowsServices.Common.FileSystemHelpers;
using MP.WindowsServices.Common.FileSystemHelpers.Interfaces;
using MP.WindowsServices.FileStorageObserver;
using MP.WindowsServices.FileStorageObserver.Interfaces;
using MP.WindowsServices.ImagesManager;
using MP.WindowsServices.ImagesManager.ImagesBatchCleaner;
using MP.WindowsServices.ImagesManager.ImagesBatchHandlers;
using MP.WindowsServices.ImagesManager.ImagesBatchPublisher;
using MP.WindowsServices.ImagesManager.Interfaces;
using MP.WindowsServices.MQManager;
using MP.WindowsServices.MQManager.FileMessageFactory;
using MP.WindowsServices.MQManager.Messages;
using MP.WindowsServices.ProcessingBuilder;

namespace MP.WindowsServices.DependencyResolver
{
    public static class DependencyResolver
    {
        private static IContainer _container;

        public static IContainer Container
        {
            get
            {
                if(_container == null)
                {
                    _container = Resolve();
                }

                return _container;
            }
        }

        private static IContainer Resolve()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<FilePatchMessageFactory>().As<IFilePatchMessageFactory>();
            builder.RegisterType<RabbitMQPublisher<FileBatchMessage>>().As<IPublisher<FileBatchMessage>>()
                                                                       .WithParameter("exchangeName", RabbitMQExchangeConsts.ExchangeBatchesToProceedName);
            builder.RegisterType<RabbitMQPublisher<ServiceStateInfo>>().As<IPublisher<ServiceStateInfo>>()
                                                                       .WithParameter("exchangeName", RabbitMQExchangeConsts.ExchangeServiceStateInfoName);

            builder.RegisterType<CentralServerNotifyer>().As<ICentralServerNotifyer>();

            builder.RegisterType<MigraDocPdfGenerator>().As<IPdfGenerator>();
            builder.RegisterType<LocalFileSystemHelper>().As<IFileSystemHelper>();
            builder.RegisterType<LocalFileSystemObserver>().As<IFileStorageObserver>();            

            builder.RegisterType<ImagesBatchProvider>().As<IImagesBatchProvider>();
            builder.RegisterType<PdfImagesBatchHandler>().As<IImagesBatchHandler>();
            builder.RegisterType<ImagesBatchFilesCleaner>().As<IImagesBatchCleaner>();
            builder.RegisterType<ImagesBatchPublisher>().As<IImagesBatchPublisher>();

            builder.RegisterType<FileStorageWorkflowBuilder>().As<IFileStorageWorkflowBuilder>();

            return builder.Build();
        }
    }
}
