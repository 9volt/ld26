using UnityEngine;
using System.Collections;

public class door_behavior : MonoBehaviour {
	private float last_up;
	private Transform door;
	private bool open;
	private AudioSource audio;
	public string name;
	public idiort_move player;
	// Use this for initialization
	void Start () {
		last_up = 0f;
		door = this.transform.parent.GetComponent<Transform>();
		audio = this.transform.parent.GetComponent<AudioSource>();
		open = false;
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<idiort_move>();
	}
	
	void OnTriggerEnter(Collider collider){
		if(collider.gameObject.tag == "Player"){
			player.play_audio(name);
		}
		if(!open){
			Vector3 up = new Vector3(0, 4, 0);
			door.Translate(up);
			last_up = Time.time;
			audio.Play();
			open = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - last_up > 2f && open){
			Vector3 down = new Vector3(0, -4, 0);
			door.Translate(down);
			open = false;
		}
	}
}
