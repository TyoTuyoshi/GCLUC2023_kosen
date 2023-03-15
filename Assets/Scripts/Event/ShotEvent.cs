using Actor;
using Actor.Bullet;
using UnityEngine;

namespace Event
{
    /// <summary>
    ///     銃弾発射用のイベント
    /// </summary>
    public class ShotEvent : IEvent
    {
        /// <summary>
        ///     発射する場所
        /// </summary>
        public Vector3 Pos { get; init; }

        /// <summary>
        ///     発射する方向
        /// </summary>
        public Vector3 Dir { get; init; }

        /// <summary>
        ///     弾のプレハブ
        /// </summary>
        public BulletBase Prefab { get; init; }

        /// <summary>
        ///     発射したActor
        /// </summary>
        public ActorBase Source { get; init; }
    }
}