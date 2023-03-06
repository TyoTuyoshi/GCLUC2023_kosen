namespace Actor
{
    public interface IActor
    {
        public int Level { get; }

        public void PublishActorEvent(IActorEvent ev);
    }
}