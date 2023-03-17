using Event;
using UnityEngine;

namespace Actor
{
    public class SpawnDestroyEventPublisher : MonoBehaviour
    {
        private void Start()
        {
            TryGetComponent(out SpriteRendererComposite composite);
            TryGetComponent(out SpriteRenderer spriteRenderer);
            EventPublisher.Instance.PublishEvent(new SpawnDestroyEvent
            {
                Composite = composite,
                SpriteRenderer = spriteRenderer,
                IsSpawn = true,
                Transform = transform
            });
        }

        private void OnDestroy()
        {
            EventPublisher.Instance.PublishEvent(new SpawnDestroyEvent
            {
                IsSpawn = false,
                Transform = transform
            });
        }
    }
}