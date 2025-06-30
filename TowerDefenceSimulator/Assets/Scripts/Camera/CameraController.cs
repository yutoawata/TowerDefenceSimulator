using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] List<GameObject> LockObject;   //カメラをロックするオブジェクト
    [SerializeField] float moveSpeed = 20.0f;       //カメラの移動速度
    [SerializeField] float rotateSpeed = 60.0f;     //カメラの回転速度
    
    Gamepad controllPad = null;         //カメラ操作をするコントローラー
    Vector3 lateRotation = Vector3.zero;//1フレーム前の回転情報
    float rotationLimit = 180.0f;       //回転角の限界値
    bool canInput = false;              //連続入力防止フラグ
    bool isLock = false;                //カメラ固定フラグ
    bool isPlay = false;                //ゲームプレイフラグ

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

    //カメラの固定フラグチェック
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
    //カメラ移動処理
    private void Move()
    {
        Vector2 moveInput = -controllPad.leftStick.ReadValue();
        if (moveInput.x == 0.0f || moveInput.y == 0.0f) return;

        Quaternion rotation = transform.rotation;

        rotation.x = rotation.z = 0.0f;

        Vector3 moveDirection = rotation * new Vector3(-moveInput.x, 0.0f, -moveInput.y);

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    //前後移動処理
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

    //カメラ回転処理
    void Rotate()
    {
        Vector2 rotateInput = controllPad.rightStick.ReadValue();

        transform.eulerAngles += new Vector3(-rotateInput.y, rotateInput.x, 0.0f) * rotateSpeed * Time.deltaTime;
        
        //回転角の上限値であるなら回転しない
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
