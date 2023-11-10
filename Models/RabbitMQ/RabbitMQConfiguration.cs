using RabbitMQ.Client;

namespace WebApi.Models.RabbitMQ
{
    public class RabbitMQConfiguration
    {
        public string Uri { get; set; }
        public string Queue { get; set; }
        public Boolean durable { get; set; }
        public Boolean exclusive { get; set; }
        public Boolean autoDelete { get; set; }
        public Dictionary<string, object> arguments { get; set;}
        public string exchange { get; set; }
        public IBasicProperties basicProperties { get; set; }
    }
}
