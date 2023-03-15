using System.Collections.Generic;
using System.Linq;
using AutoGenerate;
using Particle;
using UniRx;
using UnityEngine;
using Utils;

namespace Actor
{
    public class AnimEffectHelper : MonoBehaviour
    {
        [SerializeField] private Pair<string, VfxEnum>[] eventVfxList;
        private ActorBase _actor;
        private Dictionary<string, VfxEnum> _dict;

        private void Start()
        {
            _dict = eventVfxList.ToDictionary(v => v.First, v => v.Second);

            TryGetComponent(out _actor);
            _actor.OnAnimEvent.Subscribe(OnVfx).AddTo(this);
        }

        private void OnVfx(string animEvent)
        {
            if (!_dict.ContainsKey(animEvent)) return;

            var trans = transform;
            ParticleManager.Instance.PlayVfx(_dict[animEvent], 1, trans.position, trans.rotation);
        }
    }
}