using System;
using Game;
using UnityEngine;

namespace Utils
{
    /// <summary>
    ///     シングルトンなMonoBehaviour
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>乱用はやめよう</remarks>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
#if UNITY_EDITOR
                if (_instance != null) return _instance;
                throw new NullReferenceException($"{typeof(T).Name} は現在存在しないかAwake前に取得されようとしています");
#else
                return _instance;
#endif
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null) Destroy(gameObject);
            else TryGetComponent(out _instance);
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}