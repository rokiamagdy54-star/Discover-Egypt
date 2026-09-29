using Tourism.Core.Features.Message.DTOs;

namespace Tourism.Core.Features.Message.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDto> SendMessageAsync(string senderId, CreateMessageDto dto);
        Task<List<MessageDto>> GetMessagesAsync(string userId, int conversationId);
        Task MarkAsReadAsync(string userId, int conversationId);
    }
}