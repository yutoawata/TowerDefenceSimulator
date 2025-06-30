using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;     //navMeshAgent
    GameObject target;      //�ǐՑΏۃI�u�W�F�N�g

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookPosition = agent.desiredVelocity.normalized;
        lookPosition.y = 0.0f;
        transform.LookAt(target.transform.position);

        
    }

    /// <summary>
    /// �L�������̏���
    /// </summary>
    /// <param name="target_">NavMesh�ł̈ړ���̃I�u�W�F�N�g</param>
    public void Activate(GameObject target_)
    {
       target = target_;
       agent = GetComponent<NavMeshAgent>();
       agent.enabled = true;
       agent.SetDestination(target.transform.position);
    }
}
