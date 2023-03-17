using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace Sounds
{
    /// <summary>
    ///     SEを簡単に再生するためのコンポーネント
    /// </summary>
    [RequireComponent(typeof(VisualEffect))]
    public class SeHelper : VFXOutputEventAbstractHandler
    {
        [SerializeField] private SeEnum se;
        [SerializeField] [Range(0, 1)] private float scale = 0.8f;

        public override bool canExecuteInEditor => false;

        public override void OnVFXOutputEvent(VFXEventAttribute eventAttribute)
        {
            SoundManager.Instance.PlaySeOneShot(se, scale);
        }
    }
}