using UnityEngine;

public class BombController : MonoBehaviour
{
    
    [SerializeField] float bombTime = 1.0f;     //�����̎���
    [SerializeField] float maxScale = 3.0f;     //��X�̑傫��
    [SerializeField] float grawSpeed = 15.0f;   //�g��X�s�[�h
    [SerializeField] float residualTime = 2.0f; //�폜�܂ł̎���

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x <= maxScale && transform.localScale.y <= maxScale 
            && transform.localScale.z <= maxScale)
        {
            //�g�又��
            transform.localScale += new Vector3(maxScale, maxScale, maxScale) * Time.deltaTime;
        }
        else
        {
            //�폜����
            Destroy(gameObject, residualTime);
        }
    }
}
