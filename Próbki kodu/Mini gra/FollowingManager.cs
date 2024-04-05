using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowingManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> players = new List<GameObject>();

    [SerializeField]
    private Material leaderMaterial, followingMaterial;

    private Stack<GameObject> followingStack = new Stack<GameObject>();

    [Header("Player Attributes")]

    [SerializeField]
    private int minSpeed = 1;
    [SerializeField]
    private int maxSpeed = 15;
    [SerializeField]
    private int minAngularSpeed = 100;
    [SerializeField]
    private int maxAngularSpeed = 400;
    [SerializeField]
    private int minAcceleration = 10;
    [SerializeField]
    private int maxAcceleration = 30;

    private void Start()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<NavMeshAgent>().speed = Random.Range(minSpeed, maxSpeed);
            player.GetComponent<NavMeshAgent>().angularSpeed = Random.Range(minAngularSpeed, maxAngularSpeed);
            player.GetComponent<NavMeshAgent>().acceleration = Random.Range(minAcceleration, maxAcceleration);
        }
    }

    public void OnPlayerButtonClicked(GameObject leader)
    {
        leader.GetComponent<LeaderController>().enabled = true;
        leader.GetComponent<FollowerController>().enabled = false;
        leader.GetComponent<MeshRenderer>().material = leaderMaterial;

        if(followingStack.Count > 0)
            followingStack.Clear();

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == leader)
            {
                followingStack.Push(players[i]);
                for (int j = i + 1; j < players.Count; j++)
                {
                    players[j].GetComponent<LeaderController>().enabled = false;
                    players[j].GetComponent<FollowerController>().enabled = true;
                    players[j].GetComponent<FollowerController>().MyFollowedAgent = followingStack.Peek().GetComponent<NavMeshAgent>();
                    players[j].GetComponent<FollowerController>().MyFollowedPoint = followingStack.Peek().transform.Find("FollowPoint"); ;
                    players[j].GetComponent<MeshRenderer>().material = followingMaterial;
                    followingStack.Push(players[j]);
                }
                for (int k = 0; k < players.Count; k++)
                {
                    if (players[k] == leader)
                    {
                        break;
                    }
                    else
                    {
                        players[k].GetComponent<LeaderController>().enabled = false;
                        players[k].GetComponent<FollowerController>().enabled = true;
                        players[k].GetComponent<FollowerController>().MyFollowedAgent = followingStack.Peek().GetComponent<NavMeshAgent>();
                        players[k].GetComponent<FollowerController>().MyFollowedPoint = followingStack.Peek().transform.Find("FollowPoint"); ;
                        players[k].GetComponent<MeshRenderer>().material = followingMaterial;
                        followingStack.Push(players[k]);
                    }
                }
                break;
            }
        }
    }
}
