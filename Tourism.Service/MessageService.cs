using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tourism.Core.Entities;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.Message.DTOs;
using Tourism.Core.Features.Message.Interfaces;
using Tourism.Core.Interfaces;

namespace Tourism.Service
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Send Message
        public async Task<MessageDto> SendMessageAsync(string senderId, CreateMessageDto dto)
        {
            var conversation = await _unitOfWork.Repository<Conversation>().GetByIdAsync(dto.ConversationId);

            if (conversation == null)
                throw new NotFoundException("Conversation not found");

            // Check if sender is part of the conversation
            if (conversation.TouristId != senderId && conversation.GuideId != senderId)
                throw new ForbiddenException("You are not a participant in this conversation");

            var message = new Message
            {
                ConversationId = dto.ConversationId,
                SenderId = senderId,
                Content = dto.Content,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            await _unitOfWork.Repository<Message>().AddAsync(message);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<MessageDto>(message);
        }

        // Get Messages
        public async Task<List<MessageDto>> GetMessagesAsync(string userId, int conversationId)
        {
            var conversation = await _unitOfWork.Repository<Conversation>().GetByIdAsync(conversationId);

            if (conversation == null)
                throw new NotFoundException("Conversation not found");

            // Check if user is part of the conversation
            if (conversation.TouristId != userId && conversation.GuideId != userId)
                throw new ForbiddenException("You don't have access to this conversation");

            var messages = await _unitOfWork.Repository<Message>().GetAllAsync(
                predicate: m => m.ConversationId == conversationId);

            return _mapper.Map<List<MessageDto>>(
                messages.OrderBy(m => m.SentAt));
        }

        // Mark As Read
        public async Task MarkAsReadAsync(string userId, int conversationId)
        {
            var conversation = await _unitOfWork.Repository<Conversation>().GetByIdAsync(conversationId);

            if (conversation == null)
                throw new NotFoundException("Conversation not found");

            if (conversation.TouristId != userId && conversation.GuideId != userId)
                throw new ForbiddenException("You don't have access to this conversation");

            var unread = await _unitOfWork.Repository<Message>().GetAllAsync(
                predicate: m => m.ConversationId == conversationId &&
                                m.SenderId != userId &&
                                !m.IsRead);

            foreach (var message in unread)
            {
                message.IsRead = true;
                _unitOfWork.Repository<Message>().Update(message);
            }

            await _unitOfWork.CompleteAsync();
        }
    }
}