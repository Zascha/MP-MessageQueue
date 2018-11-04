using MP.WindowsServices.Common.Interfaces;
using System.Collections.Generic;

namespace MP.WindowsServices.ProcessingBuilder.Extentions
{
    public static class WorkflowStepExecutorExtentions
    {
        public static LinkedList<IWorkflowStepExecutor> ToLinkedList(this IEnumerable<IWorkflowStepExecutor> stepExecutors)
        {
            var linkedList = new LinkedList<IWorkflowStepExecutor>();

            foreach(var step in stepExecutors)
            {
                linkedList.AddLast(step);
            }

            return linkedList;
        }

        public static WorkflowStepChain ToWorkflowStepChain(this IEnumerable<IWorkflowStepExecutor> chainMembers)
        {
            var chain = new WorkflowStepChain();

            var linkedList = chainMembers.ToLinkedList();

            var node = linkedList.First;

            while (node.Next != null)
            {
                node.Value.StepExecuted += node.Next.Value.HandlePreviousStepResult;
                node = node.Next;
            }

            chain.ExecutorsChain = linkedList;

            return chain;
        }
    }
}
