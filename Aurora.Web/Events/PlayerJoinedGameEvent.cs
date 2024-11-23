namespace Aurora.Web.Events
{
    public class PlayerJoinedGameEvent : IEvent
    {
        static string IEvent.EventName { get => nameof(PlayerJoinedGameEvent); }
        public required object EventMessage { get; set; }
    }
}
