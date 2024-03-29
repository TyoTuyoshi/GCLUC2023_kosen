using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scene
{
    /// <summary>
    ///     シーンのロードやフェードインフェードアウトを行う
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        private const string BaseSceneName = "BaseScene";
        [SerializeField] private Image fadePanel;

        public static SceneLoader Instance { get; private set; }

        private void Start()
        {
            Instance = GetComponent<SceneLoader>();

            fadePanel.enabled = false;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void LoadBaseScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.name == BaseSceneName)
            {
                Debug.LogError("BaseSceneが初めから読み込まれています。");
                return;
            }

            SceneManager.LoadScene(BaseSceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(activeScene);
        }

        /// <summary>
        ///     シーンを遷移
        /// </summary>
        /// <param name="sceneName">遷移先のシーンの名前</param>
        public async void TransitionScene(string sceneName)
        {
            fadePanel.enabled = true;
            await DOTween.ToAlpha(() => fadePanel.color, value => fadePanel.color = value, 1, 0.5f)
                .SetLink(gameObject)
                .ToUniTask();
            await UniTask.WhenAll(
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).ToUniTask(),
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()).ToUniTask()
            );
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            await DOTween.ToAlpha(() => fadePanel.color, value => fadePanel.color = value, 0, 0.5f)
                .OnComplete(() => fadePanel.enabled = false)
                .SetLink(gameObject)
                .ToUniTask();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SceneLoader))]
    public class SceneLoaderEditor : Editor
    {
        private string _selectedScene;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying) return;

            _selectedScene = EditorGUILayout.TextField("Scene", _selectedScene);
            if (GUILayout.Button("Transition")) SceneLoader.Instance.TransitionScene(_selectedScene);
        }
    }
#endif
}