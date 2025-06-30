using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaySceneController : MonoBehaviour
{
    [SerializeField] List<GameObject> objectList;   //Play�i�K���̂ݎg�p����I�u�W�F�N�g���X�g
    [SerializeField] GameObject nextPanel;          //�J�ڐ�PanelUI
    [SerializeField] TextMeshProUGUI waveTimer;     //�eWave�̌o�ߎ��ԃ^�C�}�[�e�L�X�g
    [SerializeField] TextMeshProUGUI enemyConter;   //�G�I�u�W�F�N�g�̃J�E���^�[�e�L�X�g
    [SerializeField] EnemyPoolController enemyPool; //�G�I�u�W�F�N�g�̐����X�N���v�g
    [SerializeField] BasePointController basePoint; //���_�I�u�W�F�N�g�̃X�N���v�g

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

    //Play�i�K���ɕK�v�ȃI�u�W�F�N�g��L����
    void OnEnable()
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].SetActive(true);
        }
    }

    //Play�i�K�I�����ɕs�K�v�ȃI�u�W�F�N�g�𖳌���
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

    //�Q�[���v���C���ʉ�ʑJ�ڏ���
    void ChangeResult()
    {
        if (basePoint.HitPoint == 0 || enemyPool.PoolList.Count == 0)
        {
            nextPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
