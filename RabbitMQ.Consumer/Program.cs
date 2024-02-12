using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://jvvojbgk:OhXgCcfKkwDlmcnZPpo_-pbUjKquYAdH@chimpanzee.rmq.cloudamqp.com/jvvojbgk");

//Bağlantıyı Aktifleştirme Ve Kanal Açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Queue Oluşturma
channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);

//Queue'dan Mesaj Okuma
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "example-queue", autoAck: false, consumer);
//autoAck:false -> Normalde kuyruktaki işlem başarılı veya başarısız tüketilmesi halinde kuyruktan işlem silinir. Fakat parametreyi false yaparak işlemin başarılı olması halinde kuyruktaki işlemi sil demiş oluyoruz.
channel.BasicQos(0, 1, false);

consumer.Received += async (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
    //basicAck ile işlemin başarılı sonuçlandığını ve multiple:false ile sadece bu işlemin başarılı olduğunu belirtiyoruz
};

Console.ReadLine();