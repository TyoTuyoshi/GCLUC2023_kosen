using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace Sounds
{
    /// <summary>
    ///     SE
    /// </summary>
    public interface ISoundEffect : IDisposable
    {
        public string Label { get; }
        public void PlayOneShot(AudioSource source, float scale);
        public void Play(AudioSource source);
    }

    [Serializable]
    public class SoundEffect : ISoundEffect
    {
        private AudioClip _instance;

        [SerializeField] private string label;
        [SerializeField] private AssetReferenceT<AudioClip> clipRef;

        public AssetReferenceT<AudioClip> Clip
        {
            get => clipRef;
            init => clipRef = value;
        }

        public void Dispose()
        {
            _instance = null;
            Clip.ReleaseAsset();
        }

        public async void PlayOneShot(AudioSource source, float scale)
        {
            _instance ??= await Addressables.LoadAssetAsync<AudioClip>(Clip);
            source.PlayOneShot(_instance, scale);
        }

        public async void Play(AudioSource source)
        {
            _instance ??= await Addressables.LoadAssetAsync<AudioClip>(Clip);
            source.Stop();
            source.clip = _instance;
            source.Play();
        }

#if UNITY_EDITOR
        public string Label
        {
            get
            {
                if (string.IsNullOrEmpty(label))
                    label = AssetDatabase
                        .GUIDToAssetPath(Clip.AssetGUID)
                        .Split("/")
                        .Last()
                        .Split(".")
                        .First();

                return label;
            }
        }
#else
        public string Label => label;
#endif
    }

    /// <summary>
    ///     複数のクリップを指定し、ランダムで一つ再生
    /// </summary>
    [Serializable]
    public class GroupSoundEffect : ISoundEffect
    {
        [SerializeField] private string label;
        [SerializeField] private AssetReferenceT<AudioClip>[] clips;
        private AudioClip[] _instances;

        public void Dispose()
        {
            _instances = null;
            foreach (var reference in clips) reference.ReleaseAsset();
        }

        public async void PlayOneShot(AudioSource source, float scale)
        {
            source.PlayOneShot(await LoadClip(Random.Range(0, clips.Length)), scale);
        }

        public async void Play(AudioSource source)
        {
            source.Stop();
            source.clip = await LoadClip(Random.Range(0, clips.Length));
            source.Play();
        }

        public string Label => label;

        private async UniTask<AudioClip> LoadClip(int index)
        {
            _instances ??= new AudioClip[clips.Length];
            if (_instances[index] == null)
                _instances[index] = await Addressables.LoadAssetAsync<AudioClip>(clips[index]);
            return _instances[index];
        }
    }
}