using AutoMapper;
using Lucien.Application.Contracts.Sessions.Dto;
using Lucien.Application.Contracts.Sessions.Interfaces;
using Lucien.Domain.Sessions.Entities;
using Lucien.Domain.Sessions.Interfaces;

namespace Lucien.Application.Sessions
{
    public class SessionApplicationService : ISessionApplicationService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IMapper _mapper;

        public SessionApplicationService(ISessionRepository sessionRepository, IMapper mapper)
        {
            _sessionRepository = sessionRepository;
            _mapper = mapper;
        }

        public async Task<SessionDto> GetByRefreshTokenAsync(string? refreshToken)
        {
            Session session = await _sessionRepository.GetByRefreshTokenAsync(refreshToken);
            return _mapper.Map<SessionDto>(session);
        }

        public async Task<SessionDto> CreateAsync(CreateSessionDto input)
        {
            var session = _mapper.Map<Session>(input);
            var createdSession = await _sessionRepository.CreateAsync(session);
            return _mapper.Map<SessionDto>(createdSession);
        }


        public Task<bool> ValidateRefreshTokenAsync(Guid userId, string? refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
