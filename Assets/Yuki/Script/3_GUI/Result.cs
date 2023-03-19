using DG.Tweening;
using Game;
using Scene;
using TMPro;
using UnityEngine;

public class Result : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI result;
    [SerializeField] private TextMeshProUGUI gameOverText;

    private void Start()
    {
        const string gameOver = "ゲームオーバー";
        gameOverText.text = "";
        result.text = "";
        if (GameManager.Instance.IsGameOver)
        {
            gameOverText.text = gameOver;
            gameOverText.maxVisibleCharacters = 0;
            DOTween.Sequence()
                .SetDelay(1f)
                .Append(DOTween
                    .To(() => gameOverText.maxVisibleCharacters, v => gameOverText.maxVisibleCharacters = v,
                        gameOver.Length, 0.3f))
                .Play()
                .SetLink(gameObject);
        }
        else
        {
            result.text = $"Result：{GameManager.Instance.PrevStageTime}";
        }
    }

    public void BackSelectScene()
    {
        SceneLoader.Instance.TransitionScene("MapSelect");
    }
}