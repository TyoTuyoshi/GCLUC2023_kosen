using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Scene;
using System.IO;

namespace MapSelection
{
    //jsonからの受け取り専用クラス
    //受け取り後は破棄
   [System.Serializable]
    public class MapStateJson
    {
        public bool[] statusFlag;
    }
    
    public class MapSelection : MonoBehaviour
    {
        [SerializeField] private List<GameObject> stageButton = new List<GameObject>(); 
        private List<bool> stageStatus = new List<bool>();//ステージの状態(ステージクリアフラグ)

        private void Start()
        {
            //json読み込み
            LoadStageStatus();
            
            for (int i = 0; i < stageStatus.Count; i++)
            {
                if (stageStatus[i])
                { stageButton[i].SetActive(true); }
                else
                { stageButton[i].SetActive(false); }
            }
        }

        //jsonファイルからのステージクリアフラグの読み取り関数
        private void LoadStageStatus()
        {
            string jsonTxt = File.ReadAllText(Application.dataPath + "/Resources/StageStatus.json");
            var stageData = JsonUtility.FromJson<MapStateJson>(jsonTxt);
            for (int i = 0; i < stageData.statusFlag.Length; i++)
            {
                //Debug.Log(stageData.statusFlag[i]);
                stageStatus.Add(stageData.statusFlag[i]);
            }
        }

        public void SceneChange(string sceneName)
        {
            try
            {
                SceneManager.LoadScene(sceneName);
                //SceneLoader.Instance.TransitionScene(sceneName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}