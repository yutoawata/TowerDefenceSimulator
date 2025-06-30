using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public enum ButtonType//遷移先の種類
    {
        NONE,
        TITLE,
        RLAY,
        END
    }

    [SerializeField] GameObject cursor;     //仮想カーソル
    [SerializeField] GameObject titlePanel; //タイトルUI
    [SerializeField] ButtonType type;       //ボタンの種類

    Material material = null;   //UIのマテリアル
    
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
    /// クリック時の処理
    /// </summary>
    /// <param name="input_type">入力されたボタンの遷移先</param>
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
    /// 選択時の処理
    /// </summary>
    /// <param name="input_type">入力されたボタンの遷移先</param>
    public void Select(ButtonType input_type)
    {
        material.SetColor("_BaseColor", Color.red);
        material.SetColor("_EmissionColor", Color.red);
    }

    /// <summary>
    /// 離れた時の処理
    /// </summary>
    /// <param name="input_type">入力されたボタンの遷移先</param>
    public void Exit(ButtonType input_type)
    {
        material.SetColor("_BaseColor", Color.blue);
        material.SetColor("_EmissionColor", Color.blue);
    }
}
