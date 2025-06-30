using UnityEngine;

public class FeildGenerator : MonoBehaviour
{
    [SerializeField] GameObject enemyGanerater;     //�G�I�u�W�F�N�g�𐶐�����I�u�W�F�N�g
    [SerializeField] Material terrainMaterial;      //�n�`���b�V���ɐݒ肷��}�e���A��
    [SerializeField] Texture2D heightMap;           //�����}�b�v�摜
    [SerializeField] float heightMultiplier = 10f;  //�����̔{��
    [SerializeField] float unitScale = 1.0f;        //1�s�N�Z���̃X�P�[��
    [SerializeField] int textureSpriteNum = 1;       //�n�`�ɑ΂��ăe�N�X�`�������ԗ�
    [SerializeField] int fieldLayerMask = 3;        //�t�B�[���h�̃��C���[�}�X�N

    GameObject fieldObject = null;
    float unitSize = 0.0f;

    public GameObject FieldObject { get => fieldObject; }

    private void Awake()
    {
        //�G�f�B�^�[���[�h�Ő�������Field���폜����
        GameObject editorField = GameObject.Find("Field");
        GameObject.Destroy(editorField);

        //Field���Đ���
        GenerateField();
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void GenerateField()
    {
        if(fieldObject == null)
        {
            if (unitScale != 0.0f)
            {
                unitSize = unitScale / 2.0f;
            }
            //��������I�u�W�F�N�g��Z�߂�e�I�u�W�F�N�g�𐶐�
            fieldObject = new GameObject("Field");
            fieldObject.layer = 3;
            //���b�V���𐶐�
            GenerateTerrain();
        }
    }

    /// <summary>
    /// �폜����
    /// </summary>
    public void DeleteField()
    {
        if (fieldObject != null)
        {
            DestroyImmediate(fieldObject);
            fieldObject = null;
            enemyGanerater.GetComponent<EnemyPoolController>().DeleteNavMesh();
        }
    }

    //�n�`���b�V����������
    void GenerateTerrain()
    {
        int width = heightMap.width;
        int height = heightMap.height;
        Vector2Int heightMapSize = new Vector2Int(heightMap.width, heightMap.height);

        Mesh fieldMesh = MeshGenerator.GenerateSquareMesh(heightMapSize, heightMultiplier, unitScale, textureSpriteNum, heightMap);

        fieldObject.layer = fieldLayerMask;

        MeshGenerator.SetMeshComponent(ref fieldObject, terrainMaterial, fieldMesh);

        //NavMesh�ɐ�������Mesh��o�^���邽�߂̏���n��
        enemyGanerater.GetComponent<EnemyPoolController>().GenerateNavMesh(fieldMesh, gameObject.transform);
    }
}
