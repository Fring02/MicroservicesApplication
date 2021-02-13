using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EventBusRabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private bool disposed;
        private IConnection _connection;
        public RabbitMQConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            if (!IsConnected) TryConnect();
        }
        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !disposed;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected) throw new InvalidOperationException("RabbitMQ connection failed");
            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (disposed) return;
            try
            {
                _connection.Dispose();
                disposed = true;
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public bool TryConnect()
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            } catch (BrokerUnreachableException)
            {
                Thread.Sleep(4000);
                _connection = _connectionFactory.CreateConnection();
            }
            if (IsConnected)
            {
                Console.WriteLine("RabbitMQ connection established");
                return true;
            }
            Console.WriteLine("RabbitMQ connection failed");
            return false;
        }
    }
}
