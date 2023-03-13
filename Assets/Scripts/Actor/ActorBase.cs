using System;
using System.Net.NetworkInformation;
using UniRx;
using UnityEngine;

namespace Actor
{
    public abstract class ActorBase: MonoBehaviour
    {
        public abstract int Level { get; }

        public abstract void PublishActorEvent(IActorEvent ev);
    }

    public abstract class ActorBaseHelper : ActorBase
    {
        public override int Level => 1;
        public override void PublishActorEvent(IActorEvent ev)
        {
            
        }

        protected virtual void Start()
        {
            
        }
    }
}