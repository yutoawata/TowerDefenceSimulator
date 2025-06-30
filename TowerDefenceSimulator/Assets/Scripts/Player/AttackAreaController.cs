using UnityEngine;

public class AttackAreaController : MonoBehaviour
{
    GameObject bombPrefab;              //�����I�u�W�F�N�g�̃u���n�u
    EnemyPoolController enemyPool;      //�G�I�u�W�F�N�g�̐����X�N���v�g
    MeshFilter meshFilter = null;       //�U���͈͕\���I�u�W�F�N�g��MeshFilter�R���|�[�l���g
    MeshRenderer meshRenderer = null;   //�U���͈͕\���I�u�W�F�N�g��MeshRenderer�R���|�[�l���g
    float rayDistance = 60.0f;          //Ray�̒���
    float explosionLimit = 5.0f;        //�����܂ł̐�������
    float hitDistance = 11.0f;          //�����������鋗��
    float timer = 0.0f;                 //�^�C�}�[
    bool isSet = false;                 //�ݒu�t���O

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

    //�������o����
    void Explosion()
    {
        //�L�������ꂽ�G�I�u�W�F�N�g�Ƃ̋������Z�o
        for (int i = 0; i < enemyPool.PoolList.Count; i++)
        {
            if (enemyPool.PoolList[i].activeSelf)
            {
                Vector3 distance = enemyPool.PoolList[i].transform.position - transform.position;

                //�����͈͓��̓G�𖳌������A�L�������X�g���珜�O
                if (distance.magnitude < hitDistance)
                {
                    GameObject enemy = enemyPool.PoolList[i];
                    enemyPool.PoolList.Remove(enemy);
                    Destroy(enemy);
                    i--;//�v�f�ԍ���߂�
                }
            }
        }

        //�����G�t�F�N�g�I�u�W�F�N�g���C���X�^���X
        Instantiate(bombPrefab, transform.position, Quaternion.identity);
    }

    //�n�`�c������
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
