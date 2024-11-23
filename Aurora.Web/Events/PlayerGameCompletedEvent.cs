namespace Aurora.Web.Events
{
    public class PlayerGameCompletedEvent : IEvent
    {
        static string IEvent.EventName { get => nameof(PlayerGameCompletedEvent); }
        public required object EventMessage { get; set; }
    }
}
