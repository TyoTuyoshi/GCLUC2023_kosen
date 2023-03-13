using System;
using UnityEngine;

namespace Sounds
{
    /// <summary>
    /// SEを簡単に再生するためのコンポーネント
    /// </summary>
    public class SeHelper : MonoBehaviour
    {
        [SerializeField] private SeEnum se;
        [SerializeField, Range(0, 1)] private float scale = 0.8f;
        
        private void Start()
        {
            SoundManager.Instance.PlaySeOneShot(se, scale);
        }
    }
}