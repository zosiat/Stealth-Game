using UnityEngine;
using UnityEngine.AI;

public class MoveTest : MonoBehaviour
{
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(new Vector3(10, 0, 10));
    }
}