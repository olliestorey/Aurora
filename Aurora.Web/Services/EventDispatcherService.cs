using Aurora.Web.Events;
using Aurora.Web.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Aurora.Web.Services
{
    public interface IEventDispatcherService
    {
        Task<bool> DispatchEventAsync(IEvent eventDto);
    }
    public class EventDispatcherService : IEventDispatcherService
    {
        private readonly IHubContext<GameHub> _gameHubContext;
        private readonly IHubContext<PlayerHub> _playerHubContext;

        public EventDispatcherService(IHubContext<GameHub> gameHubContext, IHubContext<PlayerHub> playerHubContext)
        {
            _gameHubContext = gameHubContext;
            _playerHubContext = playerHubContext;
        }

        public async Task<bool> DispatchEventAsync(IEvent eventDto)
        {
            switch (eventDto)
            {
                case GameStartedEvent gameStartedEvent:
                case GameEndedEvent gameEndedEvent:
                    await _gameHubContext.Clients.All.SendAsync(eventDto.GetType().Name, eventDto.EventMessage);
                    return true;

                case PlayerGameCompletedEvent playerGameCompletedEvent:
                case PlayerJoinedGameEvent playerJoinedGameEvent:
                case PlayerScoreUpdatedEvent playerScoreUpdatedEvent:
                    await _playerHubContext.Clients.All.SendAsync(eventDto.GetType().Name, eventDto.EventMessage);
                    return true;
            }
            return false;
        }
    }
}
