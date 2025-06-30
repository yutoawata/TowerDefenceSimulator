using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public enum ButtonType//�J�ڐ�̎��
    {
        NONE,
        TITLE,
        RLAY,
        END
    }

    [SerializeField] GameObject cursor;     //���z�J�[�\��
    [SerializeField] GameObject titlePanel; //�^�C�g��UI
    [SerializeField] ButtonType type;       //�{�^���̎��

    Material material = null;   //UI�̃}�e���A��
    
    public ButtonType Type { get => type; }

    void Start()
    {
        material = GetComponent<Image>().material;
    }

    void OnDisable()
    {
        material.SetColor("_BaseColor", Color.blue);
        material.SetColor("_EmissionColor", Color.blue);
    }

    /// <summary>
    /// �N���b�N���̏���
    /// </summary>
    /// <param name="input_type">���͂��ꂽ�{�^���̑J�ڐ�</param>
    public void Click(ButtonType input_type)
    {
        switch (input_type)
        {
            case ButtonType.TITLE:
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
            case ButtonType.RLAY:
                titlePanel.GetComponent<TitleController>().ChangePanel();
                cursor.SetActive(false);
                break;
            case ButtonType.END:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
                break;
        }
    }

    /// <summary>
    /// �I�����̏���
    /// </summary>
    /// <param name="input_type">���͂��ꂽ�{�^���̑J�ڐ�</param>
    public void Select(ButtonType input_type)
    {
        material.SetColor("_BaseColor", Color.red);
        material.SetColor("_EmissionColor", Color.red);
    }

    /// <summary>
    /// ���ꂽ���̏���
    /// </summary>
    /// <param name="input_type">���͂��ꂽ�{�^���̑J�ڐ�</param>
    public void Exit(ButtonType input_type)
    {
        material.SetColor("_BaseColor", Color.blue);
        material.SetColor("_EmissionColor", Color.blue);
    }
}
