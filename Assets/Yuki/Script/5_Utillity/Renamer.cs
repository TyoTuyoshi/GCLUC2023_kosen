using System.Linq;
using UnityEngine;
using UnityEditor;

public class Renamer : EditorWindow
{
    [MenuItem("Window/Original_UTL/RE;NAMER")]
    static void Open()
    {
        var window = GetWindow<Renamer>();
        window.titleContent = new GUIContent("RE;NAMER");
    }

    private string text = "name";
    private int check_index = 0;
    private int num2 = 0;//インデックス用

    void OnGUI()
    {
        minSize = new Vector2(350, 250);
        maxSize = new Vector2(500, 350);

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.LabelField("RE;NAMER");
        EditorGUILayout.LabelField("ヒエラルキーで選択したオブジェクトの名前を一括変更できます。");

        EditorGUILayout.Space();
        text = EditorGUILayout.TextField("変更後の名前", text);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("変更後のインデックス");
        string[] enum_name = { "ナンバー(推奨)", "なし(非推奨)" };
        GUIStyle style_radio = new GUIStyle(EditorStyles.radioButton);
        check_index = GUILayout.SelectionGrid(check_index, enum_name, 1, style_radio);
        
        //インデックス用
        if (check_index == 0)
        { num2 = EditorGUILayout.IntField("先頭インデックス", num2); }
        
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("階層表示"))
        {
            foreach (GameObject select in UnityEditor.Selection.gameObjects)
            {
                Debug.Log($"{select.name} : {select.transform.GetSiblingIndex()}");
            }
        }

        if (GUILayout.Button("オブジェクトの名前を変更"))
        {
            int num = 0;
            var selectObjects 
                = UnityEditor.Selection.gameObjects.OrderBy(n => n.transform.GetSiblingIndex());

            foreach (GameObject select in selectObjects)
            {
                select.transform.SetSiblingIndex(num);
                switch (check_index)
                {
                    case 0: //ナンバー
                        select.name = text + num2;
                        num++;
                        num2++;
                        break;
                    case 1: //なし
                        select.name = text;
                        break;
                }
                Debug.Log(select.name);
            }
        }

        EditorGUILayout.EndVertical();
    }
}