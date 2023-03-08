using UnityEngine;

namespace Sounds
{
    /// <summary>
    ///     BGM再生用の便利コンポーネント
    /// </summary>
    public class BgmHelper : MonoBehaviour
    {
        [Header("シーン開始時に再生")] [SerializeField] private BgmEnum bgm;
        [SerializeField, Range(0, 1)] private float volume;

        private void Start()
        {
            SoundManager.Instance.ChangeBgm(bgm);
            SoundManager.Instance.SetVolume(AudioGroup.BgmVolume, volume);
        }
    }
}