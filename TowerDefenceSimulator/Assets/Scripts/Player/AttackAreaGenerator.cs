using TMPro;
using UnityEngine;

public class AttackAreaGenerator : MonoBehaviour
{
    [SerializeField] GameObject bombPrefab;         //爆発エフェクトオブジェクト
    [SerializeField] EnemyPoolController enemyPool; //敵オブジェクトのオブジェクトプールスクリプト
    [SerializeField] InputManager inputer;          //入力判定オブジェクト
    [SerializeField] Material mainMaterial;         //攻撃範囲表示オブジェクトのマテリアル
    [SerializeField] Material subAreaMaterial;      //攻撃範囲表示オブジェクトのゲージ挙動マテリアル
    [SerializeField] TextMeshProUGUI timerText;     //生成インターバル表示テキスト
    [SerializeField] Vector2Int resolution;         //Meshのポリゴン解像度
    [SerializeField] float rayDisntance;            //Rayの長さ
    [SerializeField] float areaScale;               //攻撃範囲表示オブジェクトのスケール
    [SerializeField] float GenerateIntervalTime;    //再生成までの待機時間

    GameObject currentControllArea = null;  //現在操作している範囲表示オブジェクト
    float intervalTimer = 0.0f;             //待機時間計測タイマー
    int fieldLayerMaskNum = 3;              //フィールドオブジェクトのレイヤー番号
   

    void Start()
    {
        GenerateAreaMesh();
    }

    void Update()
    {
        CheckHitField();
        if (intervalTimer <= GenerateIntervalTime)
        {
            intervalTimer += Time.deltaTime;
            timerText.text = intervalTimer.ToString("F2");
        }
        else 
        {
            timerText.text = "Full Charge";

            if (inputer.IsInputConfirmButton())
            {
                GenerateAreaMesh();
                intervalTimer = 0.0f;
            }
        }
    }

    //攻撃範囲表示座標の判定処理
    void CheckHitField()
    {
        int targetLayer = LayerMask.NameToLayer("RayTargets");
        targetLayer = ~targetLayer;

        RaycastHit hit = inputer.IsCursorRayHitObject(fieldLayerMaskNum);

        if (hit.collider != null)
        {
            currentControllArea.SetActive(true);
            currentControllArea.transform.position = hit.point;
        }
        else
        {
            currentControllArea.SetActive(false);
        }
    }

    //攻撃範囲表示オブジェクト生成処理
    void GenerateAreaMesh()
    {
        if(currentControllArea != null)
        {
            AttackAreaController controller = currentControllArea.GetComponent<AttackAreaController>();
            controller.IsSet = true;
            controller.BombPrefab = bombPrefab;   
        }

        currentControllArea = new GameObject("AttackArea");
        Mesh mesh = MeshGenerator.GenerateSquareMesh(resolution, areaScale);
        AttackAreaController newController = currentControllArea.AddComponent<AttackAreaController>();
        newController.EnemyPool = enemyPool;

        MeshGenerator.SetMeshComponent(ref currentControllArea, mainMaterial, mesh);
        MeshRenderer renderer = currentControllArea.GetComponent<MeshRenderer>();
        renderer.materials = new Material[2] { mainMaterial, subAreaMaterial };
    }
}
