using TMPro;
using UnityEngine;

public class AttackAreaGenerator : MonoBehaviour
{
    [SerializeField] GameObject bombPrefab;         //�����G�t�F�N�g�I�u�W�F�N�g
    [SerializeField] EnemyPoolController enemyPool; //�G�I�u�W�F�N�g�̃I�u�W�F�N�g�v�[���X�N���v�g
    [SerializeField] InputManager inputer;          //���͔���I�u�W�F�N�g
    [SerializeField] Material mainMaterial;         //�U���͈͕\���I�u�W�F�N�g�̃}�e���A��
    [SerializeField] Material subAreaMaterial;      //�U���͈͕\���I�u�W�F�N�g�̃Q�[�W�����}�e���A��
    [SerializeField] TextMeshProUGUI timerText;     //�����C���^�[�o���\���e�L�X�g
    [SerializeField] Vector2Int resolution;         //Mesh�̃|���S���𑜓x
    [SerializeField] float rayDisntance;            //Ray�̒���
    [SerializeField] float areaScale;               //�U���͈͕\���I�u�W�F�N�g�̃X�P�[��
    [SerializeField] float GenerateIntervalTime;    //�Đ����܂ł̑ҋ@����

    GameObject currentControllArea = null;  //���ݑ��삵�Ă���͈͕\���I�u�W�F�N�g
    float intervalTimer = 0.0f;             //�ҋ@���Ԍv���^�C�}�[
    int fieldLayerMaskNum = 3;              //�t�B�[���h�I�u�W�F�N�g�̃��C���[�ԍ�
   

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

    //�U���͈͕\�����W�̔��菈��
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

    //�U���͈͕\���I�u�W�F�N�g��������
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
