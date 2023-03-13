using System.Collections;
using System.Collections.Generic;
using MapSelection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene;
using System.IO;
using TMPro;

public class Stage1 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp_timer;

    private float time = 0;
    //デバッグ用スクリプト
    void Start()
    {
        //ステージクリア扱い
        /*{
            MapStateJson msjson = new MapStateJson();
            msjson.statusFlag = new bool[] { true, true, false, false };
            string jsonString = JsonUtility.ToJson(msjson);
            File.WriteAllText(Application.dataPath + "/Resources/StageStatus.json", jsonString);
        }*/
        //Invoke("BackMapSelect", 2.0f);
    }
    
    void Update()
    {
        time += Time.deltaTime;
        tmp_timer.text = time.ToString("f1");
    }
    
    private void BackMapSelect()
    {
        //SceneLoader.Instance.TransitionScene("TitleScene");
        SceneManager.LoadScene("TitleScene");
    }

  
}
