using UnityEngine;

namespace Actor
{
    public abstract class ActorBase: MonoBehaviour
    {
        public abstract int Level { get; }

        public abstract void PublishActorEvent(IActorEvent ev);
    }
}