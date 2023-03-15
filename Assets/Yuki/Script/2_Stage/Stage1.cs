using MapSelection;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using Game;
using Scene;
using UnityEngine.InputSystem;

public class Stage1 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp_timer;

    private float time = 0;

    private MapStateJson msjson = new MapStateJson();

    //デバッグ用スクリプト
    void Start()
    {
        //Invoke("BackMapSelect", 2.0f);
    }
    
    void Update()
    {
        time += Time.deltaTime;
        GameManager.Instance.PrevStageTime = time;
        tmp_timer.text = time.ToString("f2");
        //Debug.Log(GameManager.Instance.PrevStageTime);
        //if(死亡)
        {
            //Invoke("BackMapSelect", 2.0f);
        }
        //if(ボス撃破)
        {
            //StageClear();
            //Invoke("BackMapSelect", 2.0f);
        }
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            SceneLoader.Instance.TransitionScene("ResultScene");
        }
    }

    private void StageClear()
    {
        //ステージクリア扱い
        {
            msjson.statusFlag = new bool[] { true, true, false, false };//クリア前{true,false,false,false}
            string jsonString = JsonUtility.ToJson(msjson);
            File.WriteAllText(Application.dataPath + "/Resources/StageStatus.json", jsonString);
        }
    }

    private void BackMapSelect()
    {
        //SceneLoader.Instance.TransitionScene("TitleScene");
        SceneManager.LoadScene("MapSelect");
    }
}
