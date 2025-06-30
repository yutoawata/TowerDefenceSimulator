using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] GameObject cursor;                 //仮想カーソル
    [SerializeField] CameraController cameraController; //カメラ制御スクリプト
    [SerializeField] GraphicRaycaster raycaster;        //仮想カーソルとUIのInput判定を行うRaycast
    [SerializeField] EventSystem eventSystem;           //イベントシステムオブジェクト
    [SerializeField] float cursorSpeed = 2.0f;          //カーソルの移動速度
    [SerializeField] float maxDistance = 60.0f;         //カーソルとオブジェクトの判定距離

    struct ButtonState
    {
        ButtonController button;
        bool isSelect;

        public ButtonState(ButtonController button_, bool is_select)
        {
            button = button_;
            isSelect = is_select;
        }

        public ButtonController Button { get => button; set => button = value; }
        public bool IsSelect { get => isSelect; set => isSelect = value; }
    }

    ButtonState button = new ButtonState(null, false);
    Gamepad controllPad = null;         //カメラ操作をするコントローラー

    // Update is called once per frame
    void FixedUpdate()
    {
        if (cameraController.IsLock)
        {
            cursor.SetActive(true);
            MoveCursor();
            IsCursorRayHitUI();
        }
        else
        {
            cursor.SetActive(false);
        }
    }

    ///カーソル移動処理
    void MoveCursor()
    {
        controllPad = Gamepad.current;
        if (controllPad == null) return;

        Vector2 inputPadAxis = controllPad.leftStick.ReadValue();

        Vector3 moveVector = new Vector3(inputPadAxis.x, inputPadAxis.y, 0.0f) * cursorSpeed * Time.deltaTime;

        //moveVector = transform.rotation * moveVector;
        cursor.transform.position += moveVector;

    }

    /// <summary>
    /// 決定ボタンのInput判定
    /// </summary>
    /// <returns></returns>
    public bool IsInputConfirmButton()
    {
        if (controllPad == null) return false;

        return controllPad.buttonSouth.isPressed;
    }

    /// <summary>
    /// 仮想カーソルとGameObjectとのRaycast処理
    /// </summary>
    /// <param name="target_layer_mask_num">Rayとの衝突相手のレイヤー</param>
    /// <returns></returns>
    public RaycastHit IsCursorRayHitObject(int target_layer_mask_num)
    {
        Ray ray = Camera.main.ScreenPointToRay(cursor.transform.position);
        RaycastHit hit;
        target_layer_mask_num = 1 << target_layer_mask_num;

        Physics.Raycast(ray, out hit, maxDistance, target_layer_mask_num);

        return hit;
    }

    
    //仮想カーソルとUIとのRaycast処理
    bool IsCursorRayHitUI()
    {
        // 仮想カーソルのスクリーン座標取得
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, cursor.transform.position);

        PointerEventData eventData = new PointerEventData(eventSystem)
        {
            position = screenPos
        };

        //Raycastが当たった相手のリスト
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast
        raycaster.Raycast(eventData, results);

        button.IsSelect = false;

        for (int i = 0; i < results.Count; i++)
        {
            //あたった相手にButtonがあるかをチェック
            ButtonController hitButton = results[i].gameObject.GetComponent<ButtonController>();

            if (hitButton != null)
            {
                button.Button = hitButton;
                button.IsSelect = true;
                break;
            }
        }

        //Rayが当たってiいる
        if (button.Button != null) 
        {
            //ボタンと仮想カーソルが重なっている状態
            if (button.IsSelect)
            {
                //重なっている状態の処理
                button.Button.Select(button.Button.Type);

                //決定ボタンを入力されたら
                if (IsInputConfirmButton())
                {
                    //入力時の処理
                    button.Button.Click(button.Button.Type);
                }
                return true;
            }
            else
            {
                button.Button.Exit(button.Button.Type);
            }
        }
        return false;
    }
}
