using UnityEngine;
using System.Collections;

public class crew_behavior : MonoBehaviour {
	private NavMeshAgent nav;
	private string name;
	private GameObject[] home_points;
	private Hashtable home;
	private float last_move;
	private idiort_move idling_remote;
	// Use this for initialization
	void Start () {
		nav = GetComponent<NavMeshAgent>();
		name = this.gameObject.name;
		home = new Hashtable();
		home["captain"] = "captains_quarters";
		home["parasite"] = "common_room";
		home["doctor"] = "med_bay";
		Debug.Log((string)home[name]);
		home_points = GameObject.FindGameObjectsWithTag((string)home[name]);
		last_move = Time.time;
		nav.SetDestination(pick_random_location());
		idling_remote = GameObject.FindGameObjectWithTag("Player").GetComponent<idiort_move>();
	}
	
	Vector3 pick_random_location(){
		int size = home_points.Length;
		int i = Random.Range(0, size);
		Transform point = home_points[i].GetComponent<Transform>();
		return point.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(nav.remainingDistance < 5f && idling_remote.idling){
			if(Time.time - last_move > 10){
				nav.SetDestination(pick_random_location());
				last_move = Time.time;
			}
		}
		
	}
}
