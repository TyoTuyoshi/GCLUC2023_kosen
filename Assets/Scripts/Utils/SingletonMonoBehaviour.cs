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
        private T _instance;
        public T Instance => _instance;

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