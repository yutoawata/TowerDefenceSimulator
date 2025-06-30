using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPoolController : MonoBehaviour
{
    [SerializeField] List<GameObject> spawnerList;  //�G���Ԃ��N�g�̔����ʒu���X�g
    [SerializeField] List<int> waveEnemyValueList;  //�E�F�[�u���Ƃ̓G�I�u�W�F�N�g�̏o�������X�g
    [SerializeField] List<float> waveIngervalList;  //�E�F�[�u���Ƃ̊Ԋu�̃��X�g
    [SerializeField] GameObject enemyPrefab;        //�G�I�u�W�F�N�g�̃u���n�u
    [SerializeField] GameObject attackPoint;        //�G�I�u�W�F�N�g�̖ړI�n

    List<GameObject> poolList;             //���������G�I�u�W�F�N�g�̃��X�g
    NavMeshData navMeshData = null;         //�i�r���b�V�����
    NavMeshDataInstance navMeshDataInstance;//�i�r���b�V���̃C���X�^���X
    float timer = 0.0f;                     //�o�ߎ���
    int boundsBoxValue = 1000;              //�i�r���b�V����Bake�͈�
    int currentWaveNum = 0;                 //���݂̃E�F�[�u��
    int nextIndex = 0;                      //���ɗL����������v�f�ԍ� 
    int allEnemyValue = 0;                  //�G�I�u�W�F�N�g�̑���

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

            //�e�E�F�[�u�̑ҋ@���Ԃ��߂��邲�ƂɓG�I�u�W�F�N�g���o��������
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

    //�G�I�u�W�F�N�g�𐶐�
    void GenerateEnemys()
    {
        //Null�`�F�b�N
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
            //�g�p����G�I�u�W�F�N�g���C���X�^���X���A�񊈐���Ԃőҋ@
            GameObject enemy = null;
            enemy = GameObject.Instantiate(enemyPrefab);
            enemy.transform.parent = gameObject.transform;
            poolList.Add(enemy);
            enemy.SetActive(false);
        }
    }

    //�G�I�u�W�F�N�g�̗L��������
    void ActivateEnemy()
    {
        //�e�����n�_�̓G���������X�g
        List<int> spawnerEnemyValueList = new List<int>();
        
        //���X�g���̑S�v�f��0�ŏ�����
        for(int i = 0; i < spawnerList.Count; i++)
        {
            spawnerEnemyValueList.Add(0);
        }

        for (int i = 0; i < waveEnemyValueList[currentWaveNum]; i++)
        {
            int spawnerNum = Random.Range(0, currentWaveNum);//�G�l�~�[���o������ꏊ�̔ԍ�
            
            //�w��̏o���ꏊ�I�u�W�F�N�g�̎q�I�u�W�F�N�g�Ɉړ�
            poolList[nextIndex].transform.parent = spawnerList[spawnerNum].transform;

            spawnerEnemyValueList[spawnerNum]++;

            float angle = (Mathf.PI * 2.0f / waveEnemyValueList[currentWaveNum]) * spawnerEnemyValueList[spawnerNum];
            poolList[nextIndex].transform.position = spawnerList[spawnerNum].transform.position
                                                + new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle));

            //�w��̓G�I�u�W�F�N�g��������
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

    //�G�I�u�W�F�N�g�̃i�r���b�V�����쐬
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

        // NavMesh�̃o�E���f�B���O�{�b�N�X���w��
        Bounds bounds = new Bounds(field_transform.position, new Vector3(boundsBoxValue, boundsBoxValue, boundsBoxValue));

        // NavMeshData���쐬
        navMeshData = new NavMeshData();
        navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData);

        NavMeshBuilder.UpdateNavMeshData(navMeshData, NavMesh.GetSettingsByID(0), navMeshBuildSouces, bounds);
    }

    //�쐬�����i�r���b�V�����폜
    public void DeleteNavMesh()
    {
        NavMesh.RemoveNavMeshData(navMeshDataInstance);
    }

    //�I�u�W�F�N�g�v�[���폜����
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
