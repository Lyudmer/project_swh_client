﻿using ClientSWH.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Collections;



namespace ClientSWH.SendReceivServer.Settings
{
    public class RabbitMQBase : IRabbitMQBase
    {
        public IModel GetConfigureRabbitMQ()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                             .Build();

            var connection = GetRabbitConnection(configuration);
            var channel = connection.CreateModel();
            return channel;
        }

        public IConnection GetRabbitConnection(IConfiguration configuration)
        {

            var rmqSettings = configuration.Get<ApplicationSettings>()?.RmqSettings;
            ConnectionFactory factory = new()
            {
                HostName = rmqSettings?.Host,
                VirtualHost = rmqSettings?.VHost,
                UserName = rmqSettings?.Login,
                Password = rmqSettings?.Password,
            };

            return factory.CreateConnection();
        }

        public bool CloseModelRabbitMQ(IModel channel)
        {
            try
            {
                channel.Close();
            }
            catch (Exception)
            {
                return true;
            }
            return false;
        }
    }
}