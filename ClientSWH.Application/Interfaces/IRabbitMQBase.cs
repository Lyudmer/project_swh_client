using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace ClientSWH.Application.Interfaces
{
    public interface IRabbitMQBase
    {
        IModel GetConfigureRabbitMQ();
        IConnection GetRabbitConnection(IConfiguration configuration);
        bool CloseModelRabbitMQ(IModel channel);
    }
}