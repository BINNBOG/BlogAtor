using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlogAtor.Framework;

namespace BlogAtor.Publisher
{
    public static class PublisherExtensions
    {
        public static Runner AddPublisher(this Runner runner)
        {
            runner.Services.AddTransient<ISocialNetworkClient, TelegramClient>();
            runner.Services.AddTransient<ISocialNetworkClient, DiscordClient>();
            runner.Services.AddHostedService<PublisherBackgroundWorker>();

            return runner;
        }
    }
}