using UnityEngine;

public class BombController : MonoBehaviour
{
    
    [SerializeField] float bombTime = 1.0f;     //爆発の時間
    [SerializeField] float maxScale = 3.0f;     //代々の大きさ
    [SerializeField] float grawSpeed = 15.0f;   //拡大スピード
    [SerializeField] float residualTime = 2.0f; //削除までの時間

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x <= maxScale && transform.localScale.y <= maxScale 
            && transform.localScale.z <= maxScale)
        {
            //拡大処理
            transform.localScale += new Vector3(maxScale, maxScale, maxScale) * Time.deltaTime;
        }
        else
        {
            //削除処理
            Destroy(gameObject, residualTime);
        }
    }
}
