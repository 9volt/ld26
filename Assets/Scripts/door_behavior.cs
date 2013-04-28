using UnityEngine;
using System.Collections;

public class door_behavior : MonoBehaviour {
	private float last_up;
	private Transform door;
	private bool open;
	// Use this for initialization
	void Start () {
		last_up = 0f;
		door = this.transform.parent.GetComponent<Transform>();
		open = false;
	}
	
	void OnTriggerEnter(Collider collider){
		if(!open){
			Vector3 up = new Vector3(0, 5, 0);
			door.Translate(up);
			last_up = Time.time;
			open = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - last_up > 3f && open){
			Vector3 down = new Vector3(0, -5, 0);
			door.Translate(down);
			open = false;
		}
	}
}
