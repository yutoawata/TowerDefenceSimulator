using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] GameObject cursor;                 //���z�J�[�\��
    [SerializeField] CameraController cameraController; //�J��������X�N���v�g
    [SerializeField] GraphicRaycaster raycaster;        //���z�J�[�\����UI��Input������s��Raycast
    [SerializeField] EventSystem eventSystem;           //�C�x���g�V�X�e���I�u�W�F�N�g
    [SerializeField] float cursorSpeed = 2.0f;          //�J�[�\���̈ړ����x
    [SerializeField] float maxDistance = 60.0f;         //�J�[�\���ƃI�u�W�F�N�g�̔��苗��

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
    Gamepad controllPad = null;         //�J�������������R���g���[���[

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

    ///�J�[�\���ړ�����
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
    /// ����{�^����Input����
    /// </summary>
    /// <returns></returns>
    public bool IsInputConfirmButton()
    {
        if (controllPad == null) return false;

        return controllPad.buttonSouth.isPressed;
    }

    /// <summary>
    /// ���z�J�[�\����GameObject�Ƃ�Raycast����
    /// </summary>
    /// <param name="target_layer_mask_num">Ray�Ƃ̏Փˑ���̃��C���[</param>
    /// <returns></returns>
    public RaycastHit IsCursorRayHitObject(int target_layer_mask_num)
    {
        Ray ray = Camera.main.ScreenPointToRay(cursor.transform.position);
        RaycastHit hit;
        target_layer_mask_num = 1 << target_layer_mask_num;

        Physics.Raycast(ray, out hit, maxDistance, target_layer_mask_num);

        return hit;
    }

    
    //���z�J�[�\����UI�Ƃ�Raycast����
    bool IsCursorRayHitUI()
    {
        // ���z�J�[�\���̃X�N���[�����W�擾
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, cursor.transform.position);

        PointerEventData eventData = new PointerEventData(eventSystem)
        {
            position = screenPos
        };

        //Raycast��������������̃��X�g
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast
        raycaster.Raycast(eventData, results);

        button.IsSelect = false;

        for (int i = 0; i < results.Count; i++)
        {
            //�������������Button�����邩���`�F�b�N
            ButtonController hitButton = results[i].gameObject.GetComponent<ButtonController>();

            if (hitButton != null)
            {
                button.Button = hitButton;
                button.IsSelect = true;
                break;
            }
        }

        //Ray����������i����
        if (button.Button != null) 
        {
            //�{�^���Ɖ��z�J�[�\�����d�Ȃ��Ă�����
            if (button.IsSelect)
            {
                //�d�Ȃ��Ă����Ԃ̏���
                button.Button.Select(button.Button.Type);

                //����{�^������͂��ꂽ��
                if (IsInputConfirmButton())
                {
                    //���͎��̏���
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
