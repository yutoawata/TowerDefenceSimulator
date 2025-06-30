using TMPro;
using UnityEngine;

public class BasePointController : MonoBehaviour
{
    [SerializeField] EnemyPoolController enemyPool; //敵オブジェクトのオブジェクトプール
    [SerializeField] TextMeshProUGUI hitPointText;  //Hitpoint表示Text
    [SerializeField] float damegeDinstance = 5.0f;  //ダメージを受ける距離
    [SerializeField] int maxHitPoint = 10;          //HitPointの最大値

    int hitPoint = 0;   //体力値

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
        //有効化された敵オブジェクトとの距離を算出
        for (int i = 0; i < enemyPool.PoolList.Count; i++)
        {
            if (enemyPool.PoolList[i].activeSelf)
            {
                Vector3 distance = enemyPool.PoolList[i].transform.position - transform.position;

                //爆発範囲内の敵を無効化し、有効化リストから除外
                if (distance.magnitude < damegeDinstance)
                {
                    GameObject enemy = enemyPool.PoolList[i];
                    enemyPool.PoolList.Remove(enemy);
                    Destroy(enemy);
                    i--;//要素番号を戻す
                    hitPoint--;
                }
            }
        }

        hitPointText.text = hitPoint.ToString() + "/" + maxHitPoint.ToString();
    }
}
