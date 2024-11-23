public interface IEvent
{
    static abstract string EventName { get; }

    public abstract object EventMessage {get; set;}
}