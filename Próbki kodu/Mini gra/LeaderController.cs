using UnityEngine;
using UnityEngine.AI;

public class LeaderController : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitPoint;


            if (Physics.Raycast(ray, out hitPoint))
            {
                if (hitPoint.collider.tag == "Ground")
                {
                    agent.SetDestination(hitPoint.point);
                }
            }

        }
    }
}
