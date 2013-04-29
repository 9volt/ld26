using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class brigger : MonoBehaviour {
	Transform loc;
	public idiort_move master;
	private string gui;
	private Dictionary<string, string> names;
	// Use this for initialization
	void Start () {
		loc = this.gameObject.GetComponent<Transform>();
		master = this.GetComponent<idiort_move>();
		gui = null;
		names = new Dictionary<string, string>(){
			{"captain","the Captain"},
			{"first_mate","the First Mate"},
			{"cargo_man","the Cargoman"},
			{"cargo_man_mate","Hotdog"},
			{"navigator","the Navigator"},
			{"doctor","the Doctor"},
			{"cook","the Cook"},
			{"engineer","the Engineer"},
			{"leftenant","the Leftenant"},
			{"cabin_boy","the Cabin Boy"}
		};
	}
	
	void OnGUI(){
		if(gui != null){
			GUI.Label(new Rect((Screen.width / 2) - 100, (Screen.height /2) - 30, 200, 60), gui);
		}
	}
	
	string find_closest(){
		string closest = null;
		float dist = 1000f;
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("idiort")){
			Vector3 d = g.GetComponent<Transform>().position;
			if(g.name == "captain" || g.name == "first_mate"){
				//can't brig these two
			}else{
				float new_dist = Vector3.Distance(loc.position, d);
				if(new_dist < dist){
					closest = g.name;
					dist = new_dist;
				}
			}
		}
		return closest;
	}
	
	void print_gui(string s){
		gui = "Click to put " + names[s] + " in the brig";
	}
	
	void print_instructions(){
		gui = "Follow the prisoner to the brig";
	}
	
	// Update is called once per frame
	void Update () {
		if(master.walking_to_brig){
			print_instructions();	
		} else if(!master.idling){
			string closest = find_closest();
			if(closest != null){
				print_gui(closest);
				if(Input.GetButton("Fire1")){
					master.brig(closest);
				}
			}	
		} else {
			gui = null;	
		}
	}
}
