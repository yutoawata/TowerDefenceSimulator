using TMPro;
using UnityEngine;

public class BasePointController : MonoBehaviour
{
    [SerializeField] EnemyPoolController enemyPool; //�G�I�u�W�F�N�g�̃I�u�W�F�N�g�v�[��
    [SerializeField] TextMeshProUGUI hitPointText;  //Hitpoint�\��Text
    [SerializeField] float damegeDinstance = 5.0f;  //�_���[�W���󂯂鋗��
    [SerializeField] int maxHitPoint = 10;          //HitPoint�̍ő�l

    int hitPoint = 0;   //�̗͒l

    public int MaxHitPoint { get => maxHitPoint; }
    public int HitPoint { get => hitPoint; }

    // Start is called before the first frame update
    void Start()
    {
        hitPoint = maxHitPoint;
    }

    // Update is called once per frame
    void Update()
    {
        //�L�������ꂽ�G�I�u�W�F�N�g�Ƃ̋������Z�o
        for (int i = 0; i < enemyPool.PoolList.Count; i++)
        {
            if (enemyPool.PoolList[i].activeSelf)
            {
                Vector3 distance = enemyPool.PoolList[i].transform.position - transform.position;

                //�����͈͓��̓G�𖳌������A�L�������X�g���珜�O
                if (distance.magnitude < damegeDinstance)
                {
                    GameObject enemy = enemyPool.PoolList[i];
                    enemyPool.PoolList.Remove(enemy);
                    Destroy(enemy);
                    i--;//�v�f�ԍ���߂�
                    hitPoint--;
                }
            }
        }

        hitPointText.text = hitPoint.ToString() + "/" + maxHitPoint.ToString();
    }
}
