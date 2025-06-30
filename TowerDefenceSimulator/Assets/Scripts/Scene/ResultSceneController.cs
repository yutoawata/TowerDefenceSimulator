using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultSceneController : MonoBehaviour
{
    [SerializeField] List<GameObject> disableList;      //����������I�u�W�F�N�g�̃��X�g
    [SerializeField] TextMeshProUGUI resultText;        //���U���g��ʃe�L�X�g
    [SerializeField] BasePointController bPController;  //���_�̃X�N���v�g

    void OnEnable()
    {
        for (int i = 0; i < disableList.Count; i++)
        {
            disableList[i].SetActive(false);

        }

        //���ʂ��ƂɃe�L�X�g��ύX
        if (bPController.HitPoint <= 0)
        {
            resultText.text = "GameOver";
        }
        else
        {
            resultText.text = "GameCrear";
        }
    }
}
