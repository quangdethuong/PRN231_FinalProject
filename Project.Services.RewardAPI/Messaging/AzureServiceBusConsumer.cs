using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Project.Services.RewardAPI.Message;
using Project.Services.RewardAPI.Services;
using System.Text;

namespace Project.Services.RewardAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string orderCreatedTopic;
        private readonly string orderCreatedRewardsSubscription;
        private readonly IConfiguration _configuration;
        private readonly RewardsService _rewardsService;

        private ServiceBusProcessor _rewardsProcessor;
        public AzureServiceBusConsumer(IConfiguration configuration, RewardsService rewardsService)
        {
            _rewardsService = rewardsService;
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            orderCreatedTopic = _configuration.GetValue<string>("TopicAndQueueName:OrderCreatedTopic");
            orderCreatedRewardsSubscription = _configuration.GetValue<string>("TopicAndQueueName:OrderCreated_Rewards_Subscription");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _rewardsProcessor = client.CreateProcessor(orderCreatedTopic, orderCreatedRewardsSubscription);
        }

        public async Task Start()
        {
            _rewardsProcessor.ProcessMessageAsync += OnNewOrderRewardsRequestReceived;
            _rewardsProcessor.ProcessErrorAsync += ErrorHandler;
            await _rewardsProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _rewardsProcessor.StopProcessingAsync();
            await _rewardsProcessor.DisposeAsync();
        }

        private async Task OnNewOrderRewardsRequestReceived(ProcessMessageEventArgs arg)
        {
            //where you will receive message
            var message = arg.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            RewardsMessage objMessage = JsonConvert.DeserializeObject<RewardsMessage>(body);
            try
            {
                //try to log email
                await _rewardsService.UpdateRewards(objMessage);
                await arg.CompleteMessageAsync(arg.Message);
            } catch (Exception ex)
            {
                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
