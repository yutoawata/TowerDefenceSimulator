using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultSceneController : MonoBehaviour
{
    [SerializeField] List<GameObject> disableList;      //無効化するオブジェクトのリスト
    [SerializeField] TextMeshProUGUI resultText;        //リザルト画面テキスト
    [SerializeField] BasePointController bPController;  //拠点のスクリプト

    void OnEnable()
    {
        for (int i = 0; i < disableList.Count; i++)
        {
            disableList[i].SetActive(false);

        }

        //結果ごとにテキストを変更
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
