namespace Aurora.Web.Events
{
    public class PlayerScoreUpdatedEvent : IEvent
    {
        static string IEvent.EventName { get => nameof(PlayerScoreUpdatedEvent); }
        public required object EventMessage { get; set; }
    }
}
