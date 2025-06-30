using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaySceneController : MonoBehaviour
{
    [SerializeField] List<GameObject> objectList;   //Play段階時のみ使用するオブジェクトリスト
    [SerializeField] GameObject nextPanel;          //遷移先PanelUI
    [SerializeField] TextMeshProUGUI waveTimer;     //各Waveの経過時間タイマーテキスト
    [SerializeField] TextMeshProUGUI enemyConter;   //敵オブジェクトのカウンターテキスト
    [SerializeField] EnemyPoolController enemyPool; //敵オブジェクトの生成スクリプト
    [SerializeField] BasePointController basePoint; //拠点オブジェクトのスクリプト

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeResult();
        waveTimer.text = enemyPool.CurrentWaveNum.ToString() + "Wave / " + enemyPool.WaveTimer.ToString("F2");
        enemyConter.text = enemyPool.PoolList.Count.ToString() + " / " + enemyPool.AllEnemyVlaue.ToString(); 
    }

    //Play段階時に必要なオブジェクトを有効化
    void OnEnable()
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].SetActive(true);
        }
    }

    //Play段階終了時に不必要なオブジェクトを無効化
    void OnDisable()
    {
        for(int i = 0;i < objectList.Count; i++)
        {
            if(objectList[i] != null)
            {
                objectList[i].SetActive(false);
            }
        }
    }

    //ゲームプレイ結果画面遷移処理
    void ChangeResult()
    {
        if (basePoint.HitPoint == 0 || enemyPool.PoolList.Count == 0)
        {
            nextPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
