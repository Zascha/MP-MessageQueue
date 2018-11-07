using System;

namespace MP.WindowsServices.MQManager
{
    public interface ISubscriber<T>
    {
        void Receive(Action<T> procceddMessage);
    }
}
