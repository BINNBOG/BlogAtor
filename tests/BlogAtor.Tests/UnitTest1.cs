using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using BlogAtor.Store;
using BlogAtor.Store.Sql.Data;
using BlogAtor.Publisher;
using BlogAtor.Core;

namespace BlogAtor.Tests
{
    public class TestBlogContext : BlogContext
    {
        public TestBlogContext(IStartupConfigProvider provider) : base(provider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("TestBlogDb_" + Guid.NewGuid());
        }
    }

    public class PublisherWorkerTests
    {
        [Fact]
        public async Task ProcessPendingPublications_WhenAllClientsSuccess_ShouldSetIsPublishedToTrue()
        {
            var mockConfigProvider = new Mock<IStartupConfigProvider>();
            using var dbContext = new TestBlogContext(mockConfigProvider.Object);

            var testItem = new DbDataItem
            {
                Id = 1L,
                Uid = Guid.NewGuid().ToString(),
                Title = "Тестовая новость",
                Link = "https://example.com",
                IsPublished = false
            };
            dbContext.DataItems.Add(testItem);
            await dbContext.SaveChangesAsync();

            var mockClient = new Mock<ISocialNetworkClient>();
            mockClient.Setup(c => c.NetworkName).Returns("FakeTelegram");
            mockClient.Setup(c => c.PublishPostAsync(It.IsAny<DbDataItem>())).ReturnsAsync(true);

            var clients = new List<ISocialNetworkClient> { mockClient.Object };

            var serviceProvider = new ServiceCollection()
                .AddSingleton<BlogContext>(dbContext) 
                .BuildServiceProvider();

            var mockScopeFactory = new Mock<IServiceScopeFactory>();
            var mockScope = new Mock<IServiceScope>();
            mockScope.Setup(s => s.ServiceProvider).Returns(serviceProvider);
            mockScopeFactory.Setup(f => f.CreateScope()).Returns(mockScope.Object);

            var worker = new PublisherBackgroundWorker(mockScopeFactory.Object, clients);

            // взламываем воркер через рефлексию и дергаем приватный метод напрямую, не ожидая 60 секунд!
            var processMethod = worker.GetType().GetMethod("ProcessPendingPublicationsAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            var task = (Task)processMethod.Invoke(worker, null);
            await task;

            // --- ASSERT (Проверка) ---
            var updatedItem = await dbContext.DataItems.FirstAsync();
            
            // проверяем, что метод был вызван ровно 1 раз
            mockClient.Verify(c => c.PublishPostAsync(It.IsAny<DbDataItem>()), Times.Once);
            
            // убеждаемся, что флаг успешно переключился
            Assert.True(updatedItem.IsPublished);
        }
    }
}
