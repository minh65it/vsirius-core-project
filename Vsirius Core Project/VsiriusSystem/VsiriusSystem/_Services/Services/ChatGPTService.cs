using System.Text;
using System.Text.Json;
using VsiriusSystem._Services.Interfaces;
using VsiriusSystem.Dtos;

namespace VsiriusSystem._Services.Services
{
    public class ChatGPTService : IChatGPTService
    {
        private IConfiguration _config;
        public ChatGPTService(IConfiguration config)
        {
            _config = config;          
        }

        public async Task<List<ChatGPTDto>> ChatWithGPT(
            string inputChat,
            List<ChatGPTDto> listChatOld,
            int? retry = 0)
        {
            var _apiKeyGPT = _config.GetSection("KeyAPI:ChatGPT").Value;
            var _modelGPT = _config.GetSection("KeyApi:ModelGPT").Value;
            var _urlChatGPTChat = _config.GetSection("EndpointAPI:OpenAIChat").Value;
            var requestData = new
            {
                model = _modelGPT,
                messages = new List<ChatGPTDto>()
            };

            if (listChatOld.Count == 0)
            {
                var listNew = new List<ChatGPTDto>()
                {
                    new (){ role = "system", content = "ChatGPT by Vsirius" },
                    new () { role = "user", content = inputChat }
                };
                requestData.messages.AddRange(listNew);
            }
            else
            {
                requestData.messages.AddRange(listChatOld);
                requestData.messages.Add(new ChatGPTDto { role = "user", content = inputChat });
            }

            var client = new HttpClient();
            var jsonBody = JsonSerializer.Serialize(requestData);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKeyGPT}");
            var response = await client.PostAsync(_urlChatGPTChat, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<dynamic>(jsonResponse);
                var choices = responseObject.GetProperty("choices");
                var message = choices[0].GetProperty("message");
                var text = message.GetProperty("content").GetString();
                var role = message.GetProperty("role").GetString();

                var chatGPTDtos = new List<ChatGPTDto>();

                chatGPTDtos.AddRange(requestData.messages);

                var chatGPTDto = new ChatGPTDto
                {
                    role = role,
                    content = text,
                };

                chatGPTDtos.Add(chatGPTDto);

                //foreach (var item in chatGPTDtos)
                //{
                //    item.content = "<pre>" + item.content.Replace("\n", "<br>") + "</pre>";
                //}

                return chatGPTDtos;
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(70));
                retry++;
                if (retry < 2)
                {
                    await ChatWithGPT(inputChat, listChatOld, retry);
                }
                return new List<ChatGPTDto>();
            }
        }
    }
}
