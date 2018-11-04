using MP.WindowsServices.Common.Interfaces;
using System.Collections.Generic;

namespace MP.WindowsServices.ProcessingBuilder.Extentions
{
    public class WorkflowStepChain
    {
        public LinkedList<IWorkflowStepExecutor> ExecutorsChain { get; internal set; }
    }
}
