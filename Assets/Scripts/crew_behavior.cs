using UnityEngine;
using System.Collections;

public class crew_behavior : MonoBehaviour {
	private NavMeshAgent nav;
	private string my_name;
	private GameObject[] home_points;
	private float last_move;
	private idiort_move master;
	// Use this for initialization
	void Start () {
		master = GameObject.FindGameObjectWithTag("Player").GetComponent<idiort_move>();
		nav = GetComponent<NavMeshAgent>();
		my_name = this.gameObject.name;
		last_move = Time.time;
		string room = master.where_should_I_be(my_name);
		if(room == null){
			Destroy(this.gameObject);
		} else {
			home_points = GameObject.FindGameObjectsWithTag(room);
			nav.SetDestination(pick_random_location());
		}
	}
	
	Vector3 pick_random_location(){
		int size = home_points.Length;
		int i = Random.Range(0, size);
		Transform point = home_points[i].GetComponent<Transform>();
		return point.position;
	}
	
	// Update is called once per frame
	void Update () {
		string room = master.where_should_I_be(my_name);
		if(room == null){
			Destroy(this.gameObject);
		} else {
			home_points = GameObject.FindGameObjectsWithTag(master.where_should_I_be(my_name));
			if(nav.remainingDistance < 5f && master.idling){
				if(Time.time - last_move > 10){
					nav.SetDestination(pick_random_location());
					last_move = Time.time;
				}
			}
		}
	}
}
