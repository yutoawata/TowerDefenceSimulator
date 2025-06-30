using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] List<GameObject> LockObject;   //�J���������b�N����I�u�W�F�N�g
    [SerializeField] float moveSpeed = 20.0f;       //�J�����̈ړ����x
    [SerializeField] float rotateSpeed = 60.0f;     //�J�����̉�]���x
    
    Gamepad controllPad = null;         //�J�������������R���g���[���[
    Vector3 lateRotation = Vector3.zero;//1�t���[���O�̉�]���
    float rotationLimit = 180.0f;       //��]�p�̌��E�l
    bool canInput = false;              //�A�����͖h�~�t���O
    bool isLock = false;                //�J�����Œ�t���O
    bool isPlay = false;                //�Q�[���v���C�t���O

    public bool IsLock { get => isLock; }

    // Start is called before the first frame update
    void Start()
    {
        canInput = true;
        isLock = true;
        Cursor.visible = false;
        lateRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        controllPad = Gamepad.current;
        if (controllPad == null) return;

        if(Gamepad.current.buttonEast.isPressed)
        {
            if (canInput)
            {
                isLock = !isLock;
                canInput = false;
            }
        }
        else
        {
            canInput = true;
        }

        CheckIsLock();

        if (isLock == false)
        {
            Move();
            Scroll();
            Rotate();
        }
    }

    //�J�����̌Œ�t���O�`�F�b�N
    void CheckIsLock()
    {
        for (int i = 0; i < LockObject.Count; i++)
        {
            if (LockObject[i] != null && LockObject[i].activeSelf)
            {
                isLock = true;
            }
            else if (LockObject[i].activeSelf == false && isPlay == false)
            {
                isPlay = true;
                isLock = false;
            }
        }
    }
    //�J�����ړ�����
    private void Move()
    {
        Vector2 moveInput = -controllPad.leftStick.ReadValue();
        if (moveInput.x == 0.0f || moveInput.y == 0.0f) return;

        Quaternion rotation = transform.rotation;

        rotation.x = rotation.z = 0.0f;

        Vector3 moveDirection = rotation * new Vector3(-moveInput.x, 0.0f, -moveInput.y);

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    //�O��ړ�����
    void Scroll()
    {
        if (Gamepad.current.rightTrigger.isPressed)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime; 
        }
        else if (Gamepad.current.leftTrigger.isPressed)
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        }
    }

    //�J������]����
    void Rotate()
    {
        Vector2 rotateInput = controllPad.rightStick.ReadValue();

        transform.eulerAngles += new Vector3(-rotateInput.y, rotateInput.x, 0.0f) * rotateSpeed * Time.deltaTime;
        
        //��]�p�̏���l�ł���Ȃ��]���Ȃ�
        if (transform.rotation.x > rotationLimit || transform.rotation.x < -rotationLimit
            || transform.rotation.y > rotationLimit || transform.rotation.y < -rotationLimit)
        {
            transform.eulerAngles = lateRotation * Time.deltaTime;
        }
        else
        {
            lateRotation = transform.eulerAngles;
        }
    }
}
