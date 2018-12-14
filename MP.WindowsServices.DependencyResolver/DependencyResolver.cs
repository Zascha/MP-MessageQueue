using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using MP.WindowsServices.AOP;
using MP.WindowsServices.CentralServerListener;
using MP.WindowsServices.CentralServerNotify;
using MP.WindowsServices.Common.FileSystemHelpers;
using MP.WindowsServices.Common.FileSystemHelpers.Interfaces;
using MP.WindowsServices.Common.Logger;
using MP.WindowsServices.Common.SafeExecuteManagers;
using MP.WindowsServices.Common.Serializer;
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

        public static IContainer Container => _container ?? (_container = Resolve());

        private static IContainer Resolve()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Logger>().As<ILogger>();
            builder.RegisterType<JsonSerializer>().As<ISerializer>();
            builder.RegisterType<SafeExecuteManager>().As<ISafeExecuteManager>();

            builder.RegisterType<LogMethodExceptionsAspect>().PropertiesAutowired();
            builder.RegisterType<LogMethodInfoAspect>().PropertiesAutowired();

            builder.RegisterType<RabbitMQChannel>().AsSelf();
            builder.RegisterType<RabbitMQPublisher<FileBatchMessage>>().As<IPublisher<FileBatchMessage>>()
                                                                       .WithParameter("exchangeName", RabbitMQExchangeConsts.ExchangeBatchesToProceedName);
            builder.RegisterType<RabbitMQSubscriber<FileBatchMessage>>().As<ISubscriber<FileBatchMessage>>()
                                                                        .WithParameter("exchangeName", RabbitMQExchangeConsts.ExchangeBatchesToProceedName);

            builder.RegisterType<RabbitMQPublisher<ServiceStateInfoMessage>>().As<IPublisher<ServiceStateInfoMessage>>()
                                                                       .WithParameter("exchangeName", RabbitMQExchangeConsts.ExchangeServiceStateInfoName);
            builder.RegisterType<RabbitMQSubscriber<ServiceStateInfoMessage>>().As<ISubscriber<ServiceStateInfoMessage>>()
                                                                        .WithParameter("exchangeName", RabbitMQExchangeConsts.ExchangeServiceStateInfoName);

            builder.RegisterType<RabbitMQPublisher<UpdateStateInfoMessage>>().As<IPublisher<UpdateStateInfoMessage>>()
                                                                             .WithParameter("exchangeName", RabbitMQExchangeConsts.ExchangeServiceStateInfoName);
            builder.RegisterType<RabbitMQSubscriber<UpdateStateInfoMessage>>().As<ISubscriber<UpdateStateInfoMessage>>()
                                                                              .WithParameter("exchangeName", RabbitMQExchangeConsts.ExchangeServiceStateInfoName);

            builder.RegisterType<ImagesBatchSubscriber>().As<IImagesBatchSubscriber>();
            builder.RegisterType<CentralServerNotifyer>().As<ICentralServerNotifyer>();
            builder.RegisterType<SettingsManager>().As<ISettingsManager>();

            builder.RegisterType<FilePatchMessageFactory>().As<IFilePatchMessageFactory>();

            builder.RegisterType<MigraDocPdfGenerator>().As<IPdfGenerator>();
            builder.RegisterType<LocalFileSystemHelper>().As<IFileSystemHelper>();
            builder.RegisterType<LocalFileSystemObserver>().As<IFileStorageObserver>();

            // Enable AOP approach
            builder.RegisterType<PdfImagesBatchHandler>().As<IImagesBatchHandler>()
                                                       .EnableInterfaceInterceptors()
                                                       .InterceptedBy(typeof(LogMethodInfoInterceptor))
                                                       .InterceptedBy(typeof(LogMethodExceptionsInterceptor));

            builder.RegisterType<LogMethodInfoInterceptor>().AsSelf().Named<IInterceptor>("log-info");
            builder.RegisterType<LogMethodExceptionsInterceptor>().AsSelf().Named<IInterceptor>("log-exceptions");

            builder.RegisterType<ImagesBatchProvider>().As<IImagesBatchProvider>();
            builder.RegisterType<ImagesBatchFilesCleaner>().As<IImagesBatchCleaner>();
            builder.RegisterType<ImagesBatchPublisher>().As<IImagesBatchPublisher>();

            builder.RegisterType<FileStorageWorkflowBuilder>().As<IFileStorageWorkflowBuilder>();

            return builder.Build();
        }
    }
}
