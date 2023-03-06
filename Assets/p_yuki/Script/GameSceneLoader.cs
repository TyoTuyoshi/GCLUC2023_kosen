using System;
using System.Collections;
using System.Collections.Generic;
using Scene;
using UnityEngine;

public class GameSceneLoader : MonoBehaviour
{
    [SerializeField] private List<GameObject> Stage = new List<GameObject>();
    
    private void Start()
    {
        //未開放エリアは不可視
        //前のステージでクリアフラグが立ったら次のステージが可視
        for (int i = 1; i < Stage.Count; i++)
        {
            Stage[i].SetActive(false);
        }
    }
    /// <summary>
    /// numを直にシーン名にしてもいいかなとは思ってるけど混乱さけたいのでひとつづつ書いてる。
    /// </summary>
    /// <param name="num">ボタンの番号の引数</param>
    public void JumpScene(int num)
    {
        switch (num)
        {
            case 0:
                SceneLoader.Instance.TransitionScene("Stage1");
                break;
            case 1:
                SceneLoader.Instance.TransitionScene("Stage2");
                break;
            case 2:
                SceneLoader.Instance.TransitionScene("Stage3");
                break;
            case 3:
                SceneLoader.Instance.TransitionScene("Stage4");
                break;
        }
    }
}
