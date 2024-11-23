
namespace Aurora.Web.Events
{
    public class GameStartedEvent : IEvent
    {
        static string IEvent.EventName { get => nameof(GameStartedEvent); }
        public required object EventMessage { get; set; }
    }
}
