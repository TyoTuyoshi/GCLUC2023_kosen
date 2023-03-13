using System;
using UniRx;

namespace Actor.Player
{
    public partial class Player : ActorBase
    {
        private readonly Subject<IActorEvent> _onActorEvent = new();
        public override int Level => 1;
        public IObservable<IActorEvent> OnActorEvent => _onActorEvent;

        public override void PublishActorEvent(IActorEvent ev)
        {
            _onActorEvent.OnNext(ev);
        }
    }
}