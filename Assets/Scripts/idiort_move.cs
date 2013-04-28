using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class idiort_move : MonoBehaviour {
	private GameObject[] idiorts;
	private GameObject[] navs;
	public bool idling;
	public int day;
	private Dictionary<string, string> home;

	// Use this for initialization
	void Start () {
		idiorts = GameObject.FindGameObjectsWithTag("idiort");
		navs = GameObject.FindGameObjectsWithTag("bridge");
		idling = true;
		day = 1;
		home = new Dictionary<string, string>();
		home["captain"] = "captains_quarters";
		home["parasite"] = "common_room";
		home["doctor"] = "med_bay";
	}
	
	public string where_should_I_be(string who){
		return home[who];
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.B)){
			if(idling){
				idling = !idling;
				int i = 0;
				foreach(GameObject idiort in idiorts){
					NavMeshAgent n = idiort.GetComponent<NavMeshAgent>();
					Transform dest;
					if(idiort.name == "captain"){
						dest = GameObject.Find("captains_chair").GetComponent<Transform>();
					} else {
						dest = navs[i].GetComponent<Transform>();
						i++;
					}
					n.SetDestination(dest.position);
				}
			} else {
				idling = !idling;	
			}
		}
	}
}
