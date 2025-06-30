using UnityEngine;

public class TitleController : MonoBehaviour
{
    [SerializeField] CameraController cameraController; //�J��������X�N���v�g
    [SerializeField] GameObject cursor;                 //���z�J�[�\��
    [SerializeField] GameObject nextPanel;              //�J�ڐ��Panel
    [SerializeField] FeildGenerator feildGenerator;     //�t�B�[���h�����X�N���v�g
    [SerializeField] float rotateSpeed = 30.0f;         //��]���x
   
    GameObject feildObject = null;      //�t�B�[���h�I�u�W�F�N�g
    Vector3 rotateAngle = Vector3.zero; //��]�x�N�g��

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

    //�t�B�[���h�I�u�W�F�N�g��]����
    void RotateField()
    {
        feildObject.transform.Rotate(rotateAngle * Time.deltaTime);
    }

    /// <summary>
    /// �^�C�g����ʐ؂�ւ�����
    /// </summary>
    public void ChangePanel()
    {
        gameObject.SetActive(false);
        nextPanel.SetActive(true);
        feildObject.transform.rotation = Quaternion.identity;
    }
}
