using System.Collections;
using System.Collections.Generic;
using MapSelection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene;
using System.IO;

public class Stage1 : MonoBehaviour
{
    //デバッグ用スクリプト
    void Start()
    {
        //ステージクリア扱い
        {
            MapStateJson msjson = new MapStateJson();
            msjson.statusFlag = new bool[] { true, true, false, false };
            string jsonString = JsonUtility.ToJson(msjson);
            File.WriteAllText(Application.dataPath + "/Resources/StageStatus.json", jsonString);
        }
        Invoke("BackMapSelect", 2.0f);
    }

    private void BackMapSelect()
    {
        //SceneLoader.Instance.TransitionScene("TitleScene");
        SceneManager.LoadScene("TitleScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
