using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BlogAtor.Store.Sql.Data;

namespace BlogAtor.Publisher
{
    public class DiscordClient : ISocialNetworkClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _webhookUrl = "https://discord.com/api/webhooks/1508933440295407818/MwYsbstJoudGKzKcvgcRaXhsOuzxfIUgJIrHCroqRqNt-FPjq0noYWmAu8oOVH6tsR_b";

        public string NetworkName => "Discord";

        public async Task<bool> PublishPostAsync(DbDataItem item)
        {
            try
            {
                var messageText = $"**{item.Title}**\n\n[Читать в источнике]({item.Link})";

                var payload = new
                {
                    content = messageText
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_webhookUrl, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Discord Error]: {ex.Message}");
                return false;
            }
        }
    }
}