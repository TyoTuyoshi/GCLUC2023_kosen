using System.IO;
using Game;
using MapSelection;
using Scene;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Stage1 : MonoBehaviour, IStage
{
    [SerializeField] private TextMeshProUGUI tmp_timer;

    private readonly MapStateJson msjson = new();

    private float time;
    public float Elapsed => time;

    //デバッグ用スクリプト
    // private void Start()
    // {
    //     //Invoke("BackMapSelect", 2.0f);
    // }

    private void Update()
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
        // if (Keyboard.current.aKey.wasPressedThisFrame) SceneLoader.Instance.TransitionScene("ResultScene");
    }

    private void StageClear()
    {
        //ステージクリア扱い
        {
            msjson.statusFlag = new[] { true, true, false, false }; //クリア前{true,false,false,false}
            var jsonString = JsonUtility.ToJson(msjson);
            File.WriteAllText(Application.dataPath + "/Resources/StageStatus.json", jsonString);
        }
    }

    private void BackMapSelect()
    {
        //SceneLoader.Instance.TransitionScene("TitleScene");
        SceneManager.LoadScene("MapSelect");
    }
}