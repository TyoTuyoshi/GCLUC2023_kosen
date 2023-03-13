using System;
using Actor;
using UniRx;
using UnityEngine;

namespace Event
{
    /// <summary>
    ///     攻撃を伝えるイベント
    /// </summary>
    public class AttackEvent : IEvent
    {
        /// <summary>
        ///     攻撃をしたActorの座標
        /// </summary>
        public Vector3 SourcePos { get; init; }

        /// <summary>
        ///     攻撃の範囲
        /// </summary>
        public float AttackRange { get; init; }

        /// <summary>
        ///     ダメージ量
        /// </summary>
        public float Amount { get; init; }

        /// <summary>
        ///     ノックバックの方向
        /// </summary>
        public float KnockBackPower { get; init; }

        public static IObservable<AttackEvent> RegisterListenerInRange(Transform self)
        {
            return EventPublisher.Instance
                .RegisterListener<AttackEvent>()
                .Where(e => e.SourcePos != self.position)
                .Where(e => (self.position - e.SourcePos).sqrMagnitude < Mathf.Pow(e.AttackRange, 2));
        }
    }
}