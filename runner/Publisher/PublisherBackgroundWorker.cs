using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using BlogAtor.Store;
using BlogAtor.Store.Sql.Data;

namespace BlogAtor.Publisher
{
    public class PublisherBackgroundWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IEnumerable<ISocialNetworkClient> _networkClients;
        private readonly TimeSpan _period = TimeSpan.FromSeconds(60); // периодичность опроса бд

        public PublisherBackgroundWorker(
            IServiceScopeFactory scopeFactory, 
            IEnumerable<ISocialNetworkClient> networkClients)
        {
            _scopeFactory = scopeFactory;
            _networkClients = networkClients;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_period);

            while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                await ProcessPendingPublicationsAsync();
            }
        }

        private async Task ProcessPendingPublicationsAsync()
        {
            // для безопасной работы с контекстом БД в фоновом потоке
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            // у которых флаг IsPublished равен false
            var pendingItems = await dbContext.DataItems
                .Where(x => !x.IsPublished) 
                .Take(5) // берем по 5 штук, чтобы не перегружать сеть
                .ToListAsync();

            if (!pendingItems.Any()) return;

            foreach (var item in pendingItems)
            {
                bool allSuccess = true;

                // перебираем все зарегистрированные клиенты (Telegram, Discord)
                foreach (var client in _networkClients)
                {
                    var isSent = await client.PublishPostAsync(item);
                    if (!isSent)
                    {
                        allSuccess = false;
                        Console.WriteLine($"[Publisher]: Ошибка отправки поста {item.Id} в сеть {client.NetworkName}");
                    }
                }

                // помечаем пост в БД
                if (allSuccess)
                {
                    item.IsPublished = true;
                }
            }

            // сохраняем измененные статусы постов в PostgreSQL
            await dbContext.SaveChangesAsync();
        }
    }
}