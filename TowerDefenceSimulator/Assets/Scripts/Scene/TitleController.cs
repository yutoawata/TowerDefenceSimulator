using UnityEngine;

public class TitleController : MonoBehaviour
{
    [SerializeField] CameraController cameraController; //カメラ制御スクリプト
    [SerializeField] GameObject cursor;                 //仮想カーソル
    [SerializeField] GameObject nextPanel;              //遷移先のPanel
    [SerializeField] FeildGenerator feildGenerator;     //フィールド生成スクリプト
    [SerializeField] float rotateSpeed = 30.0f;         //回転速度
   
    GameObject feildObject = null;      //フィールドオブジェクト
    Vector3 rotateAngle = Vector3.zero; //回転ベクトル

    private void Awake()
    {
        nextPanel.SetActive(false);
        rotateAngle.y = rotateSpeed;
        feildObject = feildGenerator.FieldObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (feildObject != null)
        {
            RotateField();
        }
        else
        {
            feildObject = feildGenerator.FieldObject;
        }
    }

    //フィールドオブジェクト回転処理
    void RotateField()
    {
        feildObject.transform.Rotate(rotateAngle * Time.deltaTime);
    }

    /// <summary>
    /// タイトル画面切り替え処理
    /// </summary>
    public void ChangePanel()
    {
        gameObject.SetActive(false);
        nextPanel.SetActive(true);
        feildObject.transform.rotation = Quaternion.identity;
    }
}
