using Tourism.Core.Features.RequestGuide.DTOs;

namespace Tourism.Core.Features.RequestGuide.Interfaces
{
    public interface IRequestService
    {
        Task CreateRequestAsync(string touristId, CreateRequestDto dto);
        Task<List<RequestDto>> GetGuideRequestsAsync(string guideId);
        Task<List<RequestDto>> GetTouristRequestsAsync(string touristId);
        Task<RequestDto> GetRequestDetailsAsync(int requestId, string userId);
        Task AcceptRequestAsync(int requestId, string guideId);
        Task RejectRequestAsync(int requestId, string guideId);
        Task CancelRequestAsync(int requestId, string touristId);
    }
}