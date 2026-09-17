using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BlogAtor.Store.Sql.Data;

namespace BlogAtor.Publisher
{
    public class TelegramClient : ISocialNetworkClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _botToken = "8119425256:AAFwzPfm-FsoJE85UTEVd_qaM5UhOZvzLmo"; 
        private readonly string _chatId = "@rugr1k_fm";

        public string NetworkName => "Telegram";

        public async Task<bool> PublishPostAsync(DbDataItem item)
        {
            try
            {
                var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";
                
                var messageText = $"<b>{item.Title}</b>\n\n<a href='{item.Link}'>Читать в источнике</a>";

                var payload = new
                {
                    chat_id = _chatId,
                    text = messageText,
                    parse_mode = "HTML"
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Telegram Error]: {ex.Message}");
                return false;
            }
        }
    }
}