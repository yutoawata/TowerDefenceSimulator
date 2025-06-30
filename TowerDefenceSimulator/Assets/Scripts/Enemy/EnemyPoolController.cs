using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPoolController : MonoBehaviour
{
    [SerializeField] List<GameObject> spawnerList;  //敵負ぶえクトの発生位置リスト
    [SerializeField] List<int> waveEnemyValueList;  //ウェーブごとの敵オブジェクトの出現数リスト
    [SerializeField] List<float> waveIngervalList;  //ウェーブごとの間隔のリスト
    [SerializeField] GameObject enemyPrefab;        //敵オブジェクトのブレハブ
    [SerializeField] GameObject attackPoint;        //敵オブジェクトの目的地

    List<GameObject> poolList;             //生成した敵オブジェクトのリスト
    NavMeshData navMeshData = null;         //ナビメッシュ情報
    NavMeshDataInstance navMeshDataInstance;//ナビメッシュのインスタンス
    float timer = 0.0f;                     //経過時間
    int boundsBoxValue = 1000;              //ナビメッシュのBake範囲
    int currentWaveNum = 0;                 //現在のウェーブ数
    int nextIndex = 0;                      //月に有効化させる要素番号 
    int allEnemyValue = 0;                  //敵オブジェクトの総数

    public List<GameObject> PoolList { get => poolList; }

    public int CurrentWaveNum { get => currentWaveNum; }
    public int AllEnemyVlaue { get => allEnemyValue; }

    public float WaveIntervalTime { get => waveIngervalList[currentWaveNum]; }
    public float WaveTimer { get => timer; }
    
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < waveEnemyValueList.Count; i++)
        {
            allEnemyValue += waveEnemyValueList[i];
        }

        poolList = new List<GameObject>();
        GenerateEnemys();
        timer = waveIngervalList[currentWaveNum];
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf)
        {
            timer -= Time.deltaTime;

            //各ウェーブの待機時間が過ぎるごとに敵オブジェクトを出現させる
            if (timer <= 0.0f)
            {
                if (currentWaveNum < waveEnemyValueList.Count)
                {
                    ActivateEnemy();
                    timer = waveIngervalList[currentWaveNum];
                    currentWaveNum++;
                }
            }
        }
    }

    //敵オブジェクトを生成
    void GenerateEnemys()
    {
        //Nullチェック
        if (poolList != null)
        {
            DeleteEnemyPool();
        }
        else
        {
            poolList = new List<GameObject>(allEnemyValue);
        }

        for (int i = 0; i < allEnemyValue; i++)
        {
            //使用する敵オブジェクトをインスタンスし、非活性状態で待機
            GameObject enemy = null;
            enemy = GameObject.Instantiate(enemyPrefab);
            enemy.transform.parent = gameObject.transform;
            poolList.Add(enemy);
            enemy.SetActive(false);
        }
    }

    //敵オブジェクトの有効化処理
    void ActivateEnemy()
    {
        //各生成地点の敵生成数リスト
        List<int> spawnerEnemyValueList = new List<int>();
        
        //リスト内の全要素を0で初期化
        for(int i = 0; i < spawnerList.Count; i++)
        {
            spawnerEnemyValueList.Add(0);
        }

        for (int i = 0; i < waveEnemyValueList[currentWaveNum]; i++)
        {
            int spawnerNum = Random.Range(0, currentWaveNum);//エネミーが出現する場所の番号
            
            //指定の出現場所オブジェクトの子オブジェクトに移動
            poolList[nextIndex].transform.parent = spawnerList[spawnerNum].transform;

            spawnerEnemyValueList[spawnerNum]++;

            float angle = (Mathf.PI * 2.0f / waveEnemyValueList[currentWaveNum]) * spawnerEnemyValueList[spawnerNum];
            poolList[nextIndex].transform.position = spawnerList[spawnerNum].transform.position
                                                + new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle));

            //指定の敵オブジェクトを活性化
            poolList[nextIndex].SetActive(true);
            EnemyController controller = poolList[nextIndex].GetComponent<EnemyController>();
            controller.Activate(attackPoint);
            nextIndex++;

            if (nextIndex >= poolList.Count)
            {
                nextIndex = 0;
            }
        }
    }

    //敵オブジェクトのナビメッシュを作成
    public void GenerateNavMesh(Mesh field_mesh, Transform field_transform)
    {
        List<NavMeshBuildSource> navMeshBuildSouces = new List<NavMeshBuildSource>();

        NavMeshBuildSource navMeshBuildSouce = new NavMeshBuildSource
        {
            shape = NavMeshBuildSourceShape.Mesh,
            sourceObject = field_mesh,
            transform = field_transform.localToWorldMatrix,
            area = 0
        };

        navMeshBuildSouces.Add(navMeshBuildSouce);

        // NavMeshのバウンディングボックスを指定
        Bounds bounds = new Bounds(field_transform.position, new Vector3(boundsBoxValue, boundsBoxValue, boundsBoxValue));

        // NavMeshDataを作成
        navMeshData = new NavMeshData();
        navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData);

        NavMeshBuilder.UpdateNavMeshData(navMeshData, NavMesh.GetSettingsByID(0), navMeshBuildSouces, bounds);
    }

    //作成したナビメッシュを削除
    public void DeleteNavMesh()
    {
        NavMesh.RemoveNavMeshData(navMeshDataInstance);
    }

    //オブジェクトプール削除処理
    void DeleteEnemyPool()
    {
        for(int i = 0; i < poolList.Count; i++)
        {
            if(poolList[i] != null)
            {
                GameObject.Destroy(poolList[i]);
            }
        }
    }
}
