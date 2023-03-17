#nullable enable
using Actor;
using UnityEngine;

namespace Event
{
    public class SpawnDestroyEvent: IEvent
    {
        public bool IsSpawn { get; init; }
        public Transform? Transform { get; init; }
        public SpriteRenderer? SpriteRenderer { get; init; }
        public SpriteRendererComposite? Composite { get; init; }
    }
}