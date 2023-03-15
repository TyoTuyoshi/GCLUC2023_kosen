using System.Collections.Generic;
using System.Linq;
using Sounds;
using UniRx;
using UnityEngine;
using Utils;

namespace Actor
{
    public class AnimEventSeHelper : MonoBehaviour
    {
        [SerializeField] private Pair<string, SeEnum>[] eventSeList;
        [SerializeField] [Range(0, 1)] private float volume = 0.8f;
        private ActorBase _actor;
        private Dictionary<string, SeEnum> _dict;

        private void Start()
        {
            _dict = eventSeList.ToDictionary(v => v.First, v => v.Second);

            TryGetComponent(out _actor);
            _actor.OnAnimEvent
                .Subscribe(PlaySe);
        }

        private void PlaySe(string e)
        {
            if (_dict.ContainsKey(e))
                SoundManager.Instance.PlaySeOneShot(_dict[e], volume);
        }
    }
}