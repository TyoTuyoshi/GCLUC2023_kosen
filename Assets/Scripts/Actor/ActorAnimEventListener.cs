#nullable enable
using UnityEngine;

namespace Actor
{
    /// <summary>
    ///     Actorのアニメーションイベントをリッスンしてイベントを発行
    /// </summary>
    public class ActorAnimEventListener : MonoBehaviour
    {
        private ActorBase? _actor;

        private void Start()
        {
            if (!TryGetComponent(out _actor)) Debug.LogWarning("このオブジェクトからActorBaseを取得できませんでした。", gameObject);
        }

        private void AnimEvent(string eventName)
        {
            if (_actor == null) return;

            _actor.PublishActorEvent(new AnimationEvent
            {
                EventName = eventName
            });
        }
    }
}