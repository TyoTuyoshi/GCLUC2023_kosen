using MapSelection;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

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
        tmp_timer.text = "Time : " + time.ToString("f1");
        
        //if(死亡)
        {
            //Invoke("BackMapSelect", 2.0f);
        }
        //if(ボス撃破)
        {
            //StageClear();
            //Invoke("BackMapSelect", 2.0f);
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
