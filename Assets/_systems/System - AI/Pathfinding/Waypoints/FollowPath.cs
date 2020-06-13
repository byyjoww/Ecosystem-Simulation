using UnityEngine;
using UnityEngine.AI;

public class FollowPath : MonoBehaviour {

    // Waypoint manager
    public GameObject wpManager;
    // Array of waypoints
    GameObject[] wps;
    // Agent
    NavMeshAgent agent;

    // Use this for initialization
    void Start() {
        wps = wpManager.GetComponent<WPManager>().waypoints;
        agent = this.GetComponent<NavMeshAgent>();
    }

    public void GoToHeli() {
        agent.SetDestination(wps[4].transform.position);
    }

    public void GoToRuin() {
        agent.SetDestination(wps[0].transform.position);
    }

    public void GoBehindHeli() {

    }

    // Update is called once per frame
    void LateUpdate() 
    {
       
    }    
}