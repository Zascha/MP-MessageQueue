using MP.WindowsServices.MQManager.Messages;
using System;
using System.Threading.Tasks;

namespace MP.WindowsServices.MQManager
{
    public interface ISubscriber<T>
    {
        void Receive(Action<T> procceddMessage);

        Task ReceiveAsync(Action<T> procceddMessage);
    }
}
