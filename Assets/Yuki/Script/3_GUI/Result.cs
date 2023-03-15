using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;
using Scene;
using TMPro;

public class Result : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI result;
    void Start()
    {
        result.text = $"Result:{GameManager.Instance.PrevStageTime}";
    }

    public void BackSelectScene()
    {
        SceneLoader.Instance.TransitionScene("MapSelect");
    }
}
