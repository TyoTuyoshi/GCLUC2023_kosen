using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

namespace Sounds
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioSource seSource, bgmSource, uiSource;

        [Space] [Header("サウンド設定")] [SerializeField]
        private AssetReferenceT<AudioClip>[] sounds;

        [SerializeField] private GroupSoundEffect[] groupSounds;

        private Dictionary<int, ISoundEffect> _soundDic;

#if UNITY_EDITOR
        public IEnumerable<string> Sounds => sounds
            .Select(v => new SoundEffect { Clip = v })
            .Concat<ISoundEffect>(groupSounds)
            .Select(v => v.Label);
#endif

        private void Start()
        {
#if UNITY_EDITOR
            // 一つのみ存在していることの確認
            if (GameObject.FindGameObjectsWithTag("SoundManager").Length > 1) Destroy(gameObject);
#endif

            _soundDic = sounds
                .Select(v => new SoundEffect { Clip = v })
                .Concat<ISoundEffect>(groupSounds)
                .ToDictionary(v => (int)Enum.Parse<SeEnum>(v.Label), v => v);
        }

        public float GetVolume(AudioGroup group)
        {
            audioMixer.GetFloat(group.ToString(), out var value);
            return (value + 80) / 80f;
        }

        public void SetVolume(AudioGroup group, float value)
        {
            var vol = Mathf.Clamp(value, 0, 1) * 80 - 80;
            audioMixer.SetFloat(group.ToString(), vol);
        }

        public void PlaySeOneShot(SeEnum seType, float scale = 0.8f)
        {
            _soundDic[(int)seType].PlayOneShot(seSource, scale);
        }
    }

    public enum AudioGroup
    {
        MasterVolume,
        SeVolume,
        BgmVolume
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SoundManager))]
    public class SoundManagerEditor : Editor
    {
        private static Type _seEnumType;
        private Enum _selectedSe;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var manager = target as SoundManager;

            if (GUILayout.Button("Create Const Enum")) SoundEnumCreator.CreateSource(manager!.Sounds, "SeEnum.cs");

            using (new EditorGUILayout.HorizontalScope())
            {
                if (!Application.isPlaying) return;
                
                _seEnumType ??= Type.GetType($"{nameof(Sounds)}.{nameof(SeEnum)}");
                if (_seEnumType is null) return;
                _selectedSe ??= Enum.GetValues(_seEnumType).GetValue(0) as Enum;

                _selectedSe = EditorGUILayout.EnumPopup(_selectedSe);
                if (GUILayout.Button("PlayOneShot")) manager!.PlaySeOneShot((SeEnum)_selectedSe);
            }
        }
    }
#endif
}