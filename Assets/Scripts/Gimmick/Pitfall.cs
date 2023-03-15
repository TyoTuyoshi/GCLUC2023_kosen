using Actor;
using DG.Tweening;
using Event;
using UnityEngine;

namespace Gimmick
{
    /// <summary>
    ///     落とし穴のコンポーネント
    /// </summary>
    public class Pitfall : MonoBehaviour, IGimmick
    {
        [Tooltip("反応する距離")] [SerializeField] private float effectDistance = 1f;

        [SerializeField] private float stanDurationSec = 1f;
        [SerializeField] private LayerMask gimmickLayer;

        private void Start()
        {
            var col = gameObject.AddComponent<CircleCollider2D>();
            col.radius = effectDistance;
            col.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.TryGetComponent(out ActorBase actor)) return;
            if ((actor.gameObject.layer & gimmickLayer) != 0) return;

            EventPublisher.Instance.PublishEvent(new StanEvent
            {
                SourcePos = transform.position,
                Duration = stanDurationSec,
                EffectRange = effectDistance
            });
            DOVirtual.DelayedCall(stanDurationSec, () => Destroy(gameObject)).SetLink(gameObject);
        }
    }
}