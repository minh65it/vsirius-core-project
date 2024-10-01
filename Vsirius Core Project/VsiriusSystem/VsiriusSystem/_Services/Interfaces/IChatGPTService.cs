using VsiriusSystem.Dtos;

namespace VsiriusSystem._Services.Interfaces
{
    public interface IChatGPTService
    {
        Task<List<ChatGPTDto>> ChatWithGPT(string inputChat, List<ChatGPTDto> listChatOld, int? retry = 0);
    }
}
