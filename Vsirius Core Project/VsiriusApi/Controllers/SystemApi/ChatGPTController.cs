using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using VsiriusSystem._Services.Interfaces;
using VsiriusSystem.Dtos;

namespace VsiriusApi.Controllers.SystemApi
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatGPTController : ControllerBase
    {
        private readonly IChatGPTService _chatGPTService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IKeyService _keyService;

        public ChatGPTController(
            IKeyService keyService,
            IChatGPTService chatGPTService,
            IHttpContextAccessor httpContextAccessor)
        {
            _keyService = keyService;
            _chatGPTService = chatGPTService;
            _httpContextAccessor = httpContextAccessor;
        }

        //[HttpPost("ChatGPT3")]
        //public async Task<IActionResult> ChatGPT3(string inputChat)
        //{
        //    var result = await _chatGPTService.ChatGPT3(inputChat);
        //    return Ok(result);
        //}

        /// <summary>
        /// Chat GPT 3.5 - Turbo
        /// </summary>
        /// <remarks>
        /// "ChatGPT": "SDKJHKJHUWQ1NMD"
        /// </remarks>
        /// <param name="inputChat">Gửi câu hỏi</param>
        /// <param name="clientId">Nhập clientId</param>
        /// <param name="clientSecret">Nhập clientSecret</param>
        /// <returns></returns>
        [HttpPost("ChatWithGPT")]
        public async Task<IActionResult> ChatWithGPT(
            string inputChat,
            [FromHeader] string clientId,
            [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId ?? string.Empty, clientSecret ?? string.Empty))
            {
                var session = _httpContextAccessor.HttpContext?.Session;
                var chatGPTDtos = session!.Get("chatGPTDtos");
                var listChatOld = new List<ChatGPTDto>();
                if (chatGPTDtos != null)
                {
                    var compressedData = Encoding.UTF8.GetString(chatGPTDtos);
                    listChatOld = JsonSerializer.Deserialize<List<ChatGPTDto>>(compressedData);
                }

                var result = await _chatGPTService.ChatWithGPT(inputChat, listChatOld);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var serializedResult = JsonSerializer.Serialize(result, options);
                var bytes = Encoding.UTF8.GetBytes(serializedResult);
                session!.Set("chatGPTDtos", bytes);

                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }

        }
    }
}
