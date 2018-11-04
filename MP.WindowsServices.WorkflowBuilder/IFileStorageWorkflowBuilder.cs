using MP.WindowsServices.ProcessingBuilder.Extentions;
using System.Collections.Generic;

namespace MP.WindowsServices.ProcessingBuilder
{
    public interface IFileStorageWorkflowBuilder
    {
        void StartProcessing(IEnumerable<WorkflowStepChain> stepExecutors);
    }
}
