using MP.WindowsServices.Common.Interfaces;

namespace MP.WindowsServices.ImagesManager.Interfaces
{
    public interface IImagesBatchHandler : IWorkflowStepExecutor
    {
    }

    public interface IImagesBatchProvider : IWorkflowStepExecutor
    {
    }

    public interface IImagesBatchPublisher : IWorkflowStepExecutor
    {
    }

    public interface IImagesBatchCleaner : IWorkflowStepExecutor
    {
    }
}
