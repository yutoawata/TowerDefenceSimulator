using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FeildGenerator))]
public class WindowGenerator : Editor
{
    FeildGenerator script = null;//�n�`�����X�N���v�g

    /// <summary>
    /// �C���X�y�N�^�[�ɐ����E�폜�������s�{�^����\��
    /// </summary>
    public override void OnInspectorGUI()
    {
        if (script == null)
        {
            script = (FeildGenerator)target;
        }

        //��{�̃C���X�y�N�^�[��\��
        DrawDefaultInspector();

        //���������{�^��
        if (GUILayout.Button("Generate Field"))
        {
            script.GenerateField();
        }
        //�폜�����{�^��
        else if (GUILayout.Button("Delete Field"))
        {
            script.DeleteField();
        }
    }
}
