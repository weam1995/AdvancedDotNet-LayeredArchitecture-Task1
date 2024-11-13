
using CartServiceApp.BusinessLogic.Interfaces;
using CartServiceApp.DataAccess.Entities;
using Confluent.Kafka;
using KafkaClient.Consumer;
using KafkaDemo.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CartServiceApp.HostedServcies
{
    public class ProductChangeKafkaConsumerService(IServiceProvider serviceProvider) : BackgroundService
    {
        private readonly IConsumer<string,string> consumer = new KafkaConsumer("productChange-topic").Consumer;
      
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();
            using (var scope = serviceProvider.CreateScope())
            {
                var cartService = scope.ServiceProvider.GetService<ICartService>();
                while (!stoppingToken.IsCancellationRequested && cartService is not null)
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    var productChangedEvent = JsonConvert.DeserializeObject<ProductChangedEvent>(consumeResult.Message.Value);
                    if (productChangedEvent is not null)
                    {
                        
                        cartService.UpdateCartsItems(productChangedEvent);
                    }
                }
            }
                
        }
    }
}
