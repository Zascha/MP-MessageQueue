using System;

namespace MP.WindowsServices.Common.Interfaces
{
    public interface IWorkflowStepExecutor
    {
        event EventHandler<FileStoragePipelineEventArgs> StepExecuted;

        void HandlePreviousStepResult(object sender, FileStoragePipelineEventArgs args);
    }
}
