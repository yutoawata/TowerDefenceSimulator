using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;     //navMeshAgent
    GameObject target;      //追跡対象オブジェクト

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
    /// 有効化時の処理
    /// </summary>
    /// <param name="target_">NavMeshでの移動先のオブジェクト</param>
    public void Activate(GameObject target_)
    {
       target = target_;
       agent = GetComponent<NavMeshAgent>();
       agent.enabled = true;
       agent.SetDestination(target.transform.position);
    }
}
