using Cysharp.Threading.Tasks;
using DG.Tweening;
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
            await DOTween.ToAlpha(() => fadePanel.color, value => fadePanel.color = value, 1, 0.5f).ToUniTask();
            await UniTask.WhenAll(
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()).ToUniTask(),
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).ToUniTask()
            );
            await DOTween.ToAlpha(() => fadePanel.color, value => fadePanel.color = value, 0, 0.5f)
                .OnComplete(() => fadePanel.enabled = false)
                .ToUniTask();
        }
    }
}