using UnityEngine;
using System.Collections;

public class idiort_move : MonoBehaviour {
	private GameObject[] idiorts;
	private GameObject[] navs;
	// Use this for initialization
	void Start () {
		idiorts = GameObject.FindGameObjectsWithTag("idiort");
		navs = GameObject.FindGameObjectsWithTag("bridge");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.B)){
			int i = 0;
			foreach(GameObject idiort in idiorts){
				NavMeshAgent n = idiort.GetComponent<NavMeshAgent>();
				Transform dest = navs[i].GetComponent<Transform>();
				i++;
				n.SetDestination(dest.position);
			}
		}
	}
}
