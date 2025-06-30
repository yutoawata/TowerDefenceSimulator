using UnityEngine;

public class FeildGenerator : MonoBehaviour
{
    [SerializeField] GameObject enemyGanerater;     //敵オブジェクトを生成するオブジェクト
    [SerializeField] Material terrainMaterial;      //地形メッシュに設定するマテリアル
    [SerializeField] Texture2D heightMap;           //高さマップ画像
    [SerializeField] float heightMultiplier = 10f;  //高さの倍率
    [SerializeField] float unitScale = 1.0f;        //1ピクセルのスケール
    [SerializeField] int textureSpriteNum = 1;       //地形に対してテクスチャが並ぶ列数
    [SerializeField] int fieldLayerMask = 3;        //フィールドのレイヤーマスク

    GameObject fieldObject = null;
    float unitSize = 0.0f;

    public GameObject FieldObject { get => fieldObject; }

    private void Awake()
    {
        //エディターモードで生成したFieldを削除する
        GameObject editorField = GameObject.Find("Field");
        GameObject.Destroy(editorField);

        //Fieldを再生成
        GenerateField();
    }

    /// <summary>
    /// 生成処理
    /// </summary>
    public void GenerateField()
    {
        if(fieldObject == null)
        {
            if (unitScale != 0.0f)
            {
                unitSize = unitScale / 2.0f;
            }
            //生成するオブジェクトを纏める親オブジェクトを生成
            fieldObject = new GameObject("Field");
            fieldObject.layer = 3;
            //メッシュを生成
            GenerateTerrain();
        }
    }

    /// <summary>
    /// 削除処理
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

    //地形メッシュ生成処理
    void GenerateTerrain()
    {
        int width = heightMap.width;
        int height = heightMap.height;
        Vector2Int heightMapSize = new Vector2Int(heightMap.width, heightMap.height);

        Mesh fieldMesh = MeshGenerator.GenerateSquareMesh(heightMapSize, heightMultiplier, unitScale, textureSpriteNum, heightMap);

        fieldObject.layer = fieldLayerMask;

        MeshGenerator.SetMeshComponent(ref fieldObject, terrainMaterial, fieldMesh);

        //NavMeshに生成したMeshを登録するための情報を渡す
        enemyGanerater.GetComponent<EnemyPoolController>().GenerateNavMesh(fieldMesh, gameObject.transform);
    }
}
