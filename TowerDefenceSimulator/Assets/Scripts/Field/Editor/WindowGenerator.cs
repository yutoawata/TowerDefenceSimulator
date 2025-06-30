using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FeildGenerator))]
public class WindowGenerator : Editor
{
    FeildGenerator script = null;//地形生成スクリプト

    /// <summary>
    /// インスペクターに生成・削除処理実行ボタンを表示
    /// </summary>
    public override void OnInspectorGUI()
    {
        if (script == null)
        {
            script = (FeildGenerator)target;
        }

        //基本のインスペクターを表示
        DrawDefaultInspector();

        //生成処理ボタン
        if (GUILayout.Button("Generate Field"))
        {
            script.GenerateField();
        }
        //削除処理ボタン
        else if (GUILayout.Button("Delete Field"))
        {
            script.DeleteField();
        }
    }
}
