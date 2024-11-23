
namespace Aurora.Web.Events
{
    public class GameEndedEvent : IEvent
    {
        static string IEvent.EventName { get => nameof(GameEndedEvent); }
        public required object EventMessage { get; set; }
    }
}
