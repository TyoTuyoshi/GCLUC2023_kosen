using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SceneAttacher : EditorWindow
{
    [MenuItem("Window/Original_UTL/SceneAttache")]
    static void Open()
    {
        var window = GetWindow<SceneAttacher>();
        window.titleContent = new GUIContent("SceneAttacher");
    }

    //public GameObject[] Objects;
    void OnGUI()
    {
        minSize = new Vector2(350, 250);
        maxSize = new Vector2(500, 350);

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.LabelField("SceneAttacher");
        EditorGUILayout.LabelField("ボタンとシーンを登録します。");

        //var Objects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        //Objects = 
        
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("ボタンにシーンを適応"))
        {
          
        }
        EditorGUILayout.EndVertical();
    }
}