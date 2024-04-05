using UnityEngine;
using UnityEngine.AI;

public class FollowerController : MonoBehaviour
{
    private NavMeshAgent followedAgent;

    private Transform followedPoint;

    public NavMeshAgent MyFollowedAgent
    {
        get
        {
            return followedAgent;
        }
        set
        {
            followedAgent = value;
        }
    }

    public Transform MyFollowedPoint
    {
        get
        {
            return followedPoint;
        }
        set
        {
            followedPoint = value;
        }
    }

    private void Update()
    {
        if (followedAgent != null)
        {
            if (followedAgent.hasPath)
                GetComponent<NavMeshAgent>().destination = followedPoint.position;
        }
    }
}
