using RabbitMQ.Client;
using System.Text;

//Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://jvvojbgk:OhXgCcfKkwDlmcnZPpo_-pbUjKquYAdH@chimpanzee.rmq.cloudamqp.com/jvvojbgk");

//Bağlantıyı Aktifleştirme Ve Kanal Açma
using IConnection connection = factory.CreateConnection();
using IModel channel  = connection.CreateModel();

//Queue Oluşturma
channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);
//exclusive : true -> bu bağlantının dışındaki başka bir bağlantı bu kuyruğu kullanamaz. Bu sebeple false yapıyoruz.
//durable : true -> Bu durumda kuyruk kalıcı olcak ve rabbitmq restart olsa bile kuyruktaki mesajlar kaybolmayacaktır.

//Queue'ya Mesaj Gönderme
IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true;

for (int i = 0; i < 100; i++)
{
    await Task.Delay(250);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message, basicProperties: properties);
}

Console.ReadLine();