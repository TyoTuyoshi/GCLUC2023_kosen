using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Scene;

namespace Selection
{
    //[System.Serializable]
    //public class SceneButton : MonoBehaviour 
    //{
    //    public Button num;
    //    public string scene;
    //}

    public class Selection : MonoBehaviour
    {
        //[SerializeField] private List<SceneButton> SceneButton = new List<SceneButton>();

        private void Start()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                Debug.Log(scene.name);
            }
        }

        public void SceneChange(string sceneName)
        {
            try
            {
                //SceneManager.LoadScene(sceneName);
                SceneLoader.Instance.TransitionScene(sceneName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}