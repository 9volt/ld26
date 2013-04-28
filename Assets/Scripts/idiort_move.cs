using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct conv{
	public string speaker;
	public AudioClip noise;
	
	public conv(string s, AudioClip n){
		speaker = s;
		noise = n;
	}
}

public struct file_loc {
	public string speaker;
	public string file_name;
	
	public file_loc(string s, string fn){
		speaker = s;
		file_name = fn;
	}
}

public class idiort_move : MonoBehaviour {
	private GameObject[] idiorts;
	private GameObject[] navs;
	public bool idling;
	public int day;
	private Dictionary<string, string> home;
	public Converstaion c;

	// Use this for initialization
	void Start () {
		idiorts = GameObject.FindGameObjectsWithTag("idiort");
		navs = GameObject.FindGameObjectsWithTag("bridge");
		idling = true;
		day = 1;
		home = new Dictionary<string, string>();
		home["captain"] = "captains_quarters";
		home["first_mate"] = "captain_chair";
		home["cook"] = "common_room";
		home["doctor"] = "med_bay";
		home["engineer"] = "engineering";
		home["engineer_mate"] = "engineering";
		home["navigator"] = "navigation";
		home["cargo_man"] = "cargo_hold";
		home["cargo_man_mate"] = "cargo_hold";
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

public class Converstaion{
	private Dictionary<string, AudioSource> sources;
	private List<conv> convo;
	public string AUDIO_PATH = "Assets/Audio/";
	private int current_speaker;
	
	public Converstaion(file_loc[] dialog){
		sources = new Dictionary<string, AudioSource>();
		convo = new List<conv>();
		current_speaker = 0;
		foreach(file_loc t in dialog){
			AudioClip a = Resources.Load(t.file_name) as AudioClip;
			convo.Add(new conv(t.speaker, a));
			if(!sources.ContainsKey(t.speaker)){
				AudioSource s = GameObject.Find(t.speaker).GetComponent<AudioSource>();
				sources[t.speaker] = s;
			}
		}
	}
	
	public bool play(){
		if(current_speaker > convo.Count){
			return false;	
		}
		AudioSource speaker;
		if(current_speaker == 0){
			speaker = sources[convo[current_speaker].speaker];
		}else{
			speaker = sources[convo[current_speaker - 1].speaker];
		}
		if(speaker.isPlaying){
			return true;
		} else {
			speaker.clip = convo[current_speaker].noise;
			speaker.Play();
			current_speaker++;
		}
		return false;
	}
}

public class State {
	private Dictionary<string, State> children;
	// Dictionary { brigged_name -> states }
	private Dictionary<string, string> room_assignments;
	// Dictionary { name -> room }
	private Dictionary<string, Converstaion> dialog;
	// Dictionary{ room -> conversation }
	private int day;
	
	public State(int d){
		day = d;
		children = new Dictionary<string, State>();
		room_assignments = new Dictionary<string, string>();
		dialog = new Dictionary<string, Converstaion>();
	}
	
	public void add_child(string name, State child){
		children[name] = child;
	}
	
	public State get_child(string brigged){
		return children[brigged];	
	}
}