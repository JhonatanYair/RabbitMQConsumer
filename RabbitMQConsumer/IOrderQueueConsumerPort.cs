using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQConsumer
{
    public interface IOrderQueueConsumerPort
    {
        string RecibirPedido();
    }

}
