using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tourism.Core.Entities;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.Conversation.DTOs;
using Tourism.Core.Features.Conversation.Interfaces;
using Tourism.Core.Interfaces;

namespace Tourism.Service
{
    public class ConversationService : IConversationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ConversationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Create Conversation
        public async Task<ConversationDto> CreateConversationAsync(string touristId, CreateConversationDto dto)
        {
            // Return conversation if it already exists
            var existing = await _unitOfWork.Repository<Conversation>().GetFirstAsync(
                predicate: c => c.TouristId == touristId && c.GuideId == dto.GuideId,
                include: q => q.Include(c => c.Guide).ThenInclude(g => g.User)
                               .Include(c => c.Tourist).ThenInclude(t => t.User));

            if (existing != null)
                return _mapper.Map<ConversationDto>(existing);

            var conversation = new Conversation
            {
                TouristId = touristId,
                GuideId = dto.GuideId
            };

            await _unitOfWork.Repository<Conversation>().AddAsync(conversation);
            await _unitOfWork.CompleteAsync();

            // after saving, we need to include the related data for mapping
            var created = await _unitOfWork.Repository<Conversation>().GetFirstAsync(
                predicate: c => c.Id == conversation.Id,
                include: q => q.Include(c => c.Guide).ThenInclude(g => g.User)
                               .Include(c => c.Tourist).ThenInclude(t => t.User));

            return _mapper.Map<ConversationDto>(created!);
        }

        // Get My Conversations
        public async Task<List<ConversationDto>> GetMyConversationsAsync(string userId)
        {
            var conversations = await _unitOfWork.Repository<Conversation>().GetAllAsync(
                predicate: c => c.TouristId == userId || c.GuideId == userId,
                include: q => q.Include(c => c.Guide).ThenInclude(g => g.User)
                               .Include(c => c.Tourist).ThenInclude(t => t.User)
                               .Include(c => c.Messages));

            return conversations
                .OrderByDescending(c => c.Messages.Max(m => (DateTime?)m.SentAt))
                .Select(c =>
                {
                    var dto = _mapper.Map<ConversationDto>(c);

                    dto.UnreadCount = c.Messages
                        .Count(m => !m.IsRead && m.SenderId != userId);

                    var last = c.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault();
                    if (last != null)
                        dto.LastMessage = new MessagePreviewDto
                        {
                            Content = last.Content,
                            SentAt = last.SentAt
                        };

                    return dto;
                })
                .ToList();
        }

        // Get Conversation By Id
        public async Task<ConversationDto> GetConversationByIdAsync(int id, string userId)
        {
            var conversation = await _unitOfWork.Repository<Conversation>().GetFirstAsync(
                predicate: c => c.Id == id,
                include: q => q.Include(c => c.Guide).ThenInclude(g => g.User)
                               .Include(c => c.Tourist).ThenInclude(t => t.User));

            if (conversation == null)
                throw new NotFoundException("Conversation not found");

            // Fix — ownership check
            if (conversation.TouristId != userId && conversation.GuideId != userId)
                throw new ForbiddenException("You don't have access to this conversation");

            return _mapper.Map<ConversationDto>(conversation);
        }
    }
}