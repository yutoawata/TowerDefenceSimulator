using UnityEngine;

public class AttackAreaController : MonoBehaviour
{
    GameObject bombPrefab;              //爆発オブジェクトのブレハブ
    EnemyPoolController enemyPool;      //敵オブジェクトの生成スクリプト
    MeshFilter meshFilter = null;       //攻撃範囲表示オブジェクトのMeshFilterコンポーネント
    MeshRenderer meshRenderer = null;   //攻撃範囲表示オブジェクトのMeshRendererコンポーネント
    float rayDistance = 60.0f;          //Rayの長さ
    float explosionLimit = 5.0f;        //爆発までの制限時間
    float hitDistance = 11.0f;          //爆発が当たる距離
    float timer = 0.0f;                 //タイマー
    bool isSet = false;                 //設置フラグ

    public GameObject BombPrefab { set => bombPrefab = value; }
    public EnemyPoolController EnemyPool { set => enemyPool = value; }
    public bool IsSet { set => isSet = value; }

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSet)
        {
            timer += Time.deltaTime;
            meshRenderer.materials[1].SetFloat("_FillAmount", timer / explosionLimit);

            if (timer >= explosionLimit)
            {
                Explosion();
                Destroy(gameObject);
            }
        }
        else
        {
            CheckTerrain();
        }
    }

    //爆発演出処理
    void Explosion()
    {
        //有効化された敵オブジェクトとの距離を算出
        for (int i = 0; i < enemyPool.PoolList.Count; i++)
        {
            if (enemyPool.PoolList[i].activeSelf)
            {
                Vector3 distance = enemyPool.PoolList[i].transform.position - transform.position;

                //爆発範囲内の敵を無効化し、有効化リストから除外
                if (distance.magnitude < hitDistance)
                {
                    GameObject enemy = enemyPool.PoolList[i];
                    enemyPool.PoolList.Remove(enemy);
                    Destroy(enemy);
                    i--;//要素番号を戻す
                }
            }
        }

        //爆発エフェクトオブジェクトをインスタンス
        Instantiate(bombPrefab, transform.position, Quaternion.identity);
    }

    //地形把握処理
    void CheckTerrain()
    {
        Vector3[] vertices = new Vector3[meshFilter.mesh.vertices.Length];

        for (int i = 0; i < meshFilter.mesh.vertices.Length; i++)
        {
            Vector3 worldPos = transform.position + meshFilter.mesh.vertices[i];
            if (Physics.Raycast(worldPos + Vector3.up * rayDistance / 2.0f, Vector3.down * rayDistance, out RaycastHit hit))
            {
                vertices[i] = transform.InverseTransformPoint(hit.point);
            }
        }

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateNormals();
    }
}
