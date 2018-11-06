using System.Threading.Tasks;

namespace MP.WindowsServices.MQManager
{
    public interface IPublisher<T>
    {
        void Publish(T message);

        Task PublishAsync(T message);
    }
}
