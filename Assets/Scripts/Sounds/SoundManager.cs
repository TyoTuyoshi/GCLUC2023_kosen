using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

namespace Sounds
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager _instance;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioSource seSource, bgmSource1, bgmSource2, uiSource;

        [Space] [Header("サウンド設定")] [SerializeField]
        private AssetReferenceT<AudioClip>[] soundEffects;

        [SerializeField] private GroupSoundEffect[] groupSoundEffects;
        [SerializeField] private AssetReferenceT<AudioClip>[] bgmList;
        private Dictionary<int, ISoundEffect> _bgmDict;
        private Tween _bgmFadeTween;
        private AudioGroup _currentBgmSource = AudioGroup.Bgm1Volume;
        private Dictionary<int, ISoundEffect> _soundDict;
        public static SoundManager Instance => _instance ??= FindObjectOfType<SoundManager>();

        private void Start()
        {
            InitDict();
        }

        /// <summary>
        ///     現在の音量が1~0に正規化されて返される
        /// </summary>
        public float GetVolume(AudioGroup group)
        {
            audioMixer.GetFloat(group.GetString(), out var value);
            return (value + 80) / 80f;
        }

        /// <summary>
        ///     valueには1~0を指定
        /// </summary>
        public void SetVolume(AudioGroup group, float value)
        {
            var vol = Mathf.Clamp(value, 0, 1) * 80 - 80;
            audioMixer.SetFloat(group.GetString(), vol);
        }

        /// <summary>
        ///     効果音の再生
        /// </summary>
        public void PlaySeOneShot(SeEnum seType, float scale = 0.8f)
        {
            _soundDict[(int)seType].PlayOneShot(seSource, scale);
        }

        /// <summary>
        ///     BGMの変更、現在かかっているものとクロスフェードして再生
        /// </summary>
        public void ChangeBgm(BgmEnum bgmType, float fadeSec = 1f, float scale = 0.8f)
        {
            // 前のものが生きているなら強制終了
            if (_bgmFadeTween.IsActive() && !_bgmFadeTween.IsComplete()) _bgmFadeTween.Complete();

            var target = _bgmDict[(int)bgmType];
            target.Play(_currentBgmSource == AudioGroup.Bgm1Volume ? bgmSource2 : bgmSource1);

            // クロスフェードさせながら音量が0になったものはストップ
            var endValue = _currentBgmSource == AudioGroup.Bgm1Volume ? 0 : 1;
            _bgmFadeTween = DOTween.Sequence()
                .Append(DOTween.To(() => GetVolume(AudioGroup.Bgm1Volume), v => SetVolume(AudioGroup.Bgm1Volume, v),
                    endValue, fadeSec).OnComplete(() =>
                {
                    if (_currentBgmSource == AudioGroup.Bgm1Volume) bgmSource2.Stop();
                }))
                .Join(DOTween.To(() => GetVolume(AudioGroup.Bgm2Volume), v => SetVolume(AudioGroup.Bgm2Volume, v),
                    1 - endValue, fadeSec).OnComplete(() =>
                {
                    if (_currentBgmSource == AudioGroup.Bgm2Volume) bgmSource1.Stop();
                }))
                .Play()
                .SetLink(gameObject);

            _currentBgmSource = _currentBgmSource == AudioGroup.Bgm1Volume
                ? AudioGroup.Bgm2Volume
                : AudioGroup.Bgm1Volume;
        }

        private void InitDict()
        {
            _soundDict = soundEffects
                .Select(v => new SoundEffect { Clip = v })
                .Concat<ISoundEffect>(groupSoundEffects)
                .ToDictionary(v => (int)Enum.Parse<SeEnum>(v.Label), v => v);
            _bgmDict = bgmList
                .Select(v => new SoundEffect { Clip = v } as ISoundEffect)
                .ToDictionary(v => (int)Enum.Parse<BgmEnum>(v.Label), v => v);
        }

#if UNITY_EDITOR
        public void CreateSoundEnums()
        {
            SoundEnumCreator.CreateSource(soundEffects
                .Select(v => new SoundEffect { Clip = v })
                .Concat<ISoundEffect>(groupSoundEffects)
                .Select(v => v.Label), "SeEnum.cs");
            SoundEnumCreator.CreateSource(bgmList
                .Select(v => new SoundEffect { Clip = v }.Label), "BgmEnum.cs");
        }
#endif
    }

    public enum AudioGroup
    {
        MasterVolume,
        SeVolume,
        Bgm1Volume,
        Bgm2Volume,
        UiVolume
    }

    public static class AudioGroupExt
    {
        public static string GetString(this AudioGroup group)
        {
            return group switch
            {
                AudioGroup.MasterVolume => "MasterVolume",
                AudioGroup.SeVolume => "SeVolume",
                AudioGroup.Bgm1Volume => "Bgm1Volume",
                AudioGroup.Bgm2Volume => "Bgm2Volume",
                AudioGroup.UiVolume => "UiVolume",
                _ => throw new ArgumentOutOfRangeException(nameof(group), group, null)
            };
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SoundManager))]
    public class SoundManagerEditor : Editor
    {
        private BgmEnum _selectedBgm;
        private SeEnum _selectedSe;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var manager = target as SoundManager;

            if (GUILayout.Button("Create Const Enum")) manager!.CreateSoundEnums();

            using (new EditorGUILayout.HorizontalScope())
            {
                if (!Application.isPlaying) return;

                _selectedSe = (SeEnum)EditorGUILayout.EnumPopup(_selectedSe);
                if (GUILayout.Button("PlayOneShot")) manager!.PlaySeOneShot(_selectedSe);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                if (!Application.isPlaying) return;

                _selectedBgm = (BgmEnum)EditorGUILayout.EnumPopup(_selectedBgm);
                if (GUILayout.Button("Play")) manager!.ChangeBgm(_selectedBgm);
            }
        }
    }
#endif
}