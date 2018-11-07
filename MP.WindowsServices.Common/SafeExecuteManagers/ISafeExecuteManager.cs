using System;
using System.Threading.Tasks;

namespace MP.WindowsServices.Common.SafeExecuteManagers
{
    public interface ISafeExecuteManager
    {
        void ExecuteWithExceptionLogging(Action action);

        void ExecuteWithExceptionLogging(Func<Task> func);
    }
}
