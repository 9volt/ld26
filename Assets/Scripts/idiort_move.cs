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
	private Converstaion c;
	private State current_state;
	private Dictionary<string, State> states;
	private bool playing;
	private int rooms_visited;
	// Use this for initialization
	void Start () {
		idiorts = GameObject.FindGameObjectsWithTag("idiort");
		navs = GameObject.FindGameObjectsWithTag("bridge");
		rooms_visited = 0;
		idling = true;
		day = 1;
//		home["captain"] = "captains_quarters";
//		home["first_mate"] = "captain_chair";
//		home["cook"] = "common_room";
//		home["doctor"] = "med_bay";
//		home["engineer"] = "engineering";
//		home["leftenant"] = "engineering";
//		home["navigator"] = "navigation";
//		home["cargo_man"] = "cargo_hold";
//		home["cargo_man_mate"] = "cargo_hold";
		states = new Dictionary<string, State>();
		playing = false;
		create_states();
	}
	
	private void create_states(){
		//Day 3.1
		Dictionary<string, string> locations = new Dictionary<string, string>(){
			{"doctor", "engineering"},
			{"leftenant", "med_bay"},
			{"engineer", "engineering"},
			{"cook", "common_room"},
			{"cargo_man", "cargo_hold"},
			{"cargo_man_mate", "cargo_hold"},
			{"captain", "captains_quarters"}
		};
		Dictionary<string, Converstaion> convos = new Dictionary<string, Converstaion>(){
			{"cargo_hold", new Converstaion(new file_loc[2]{
					new file_loc("cargo_man", "35"),
					new file_loc("cargo_man_mate", "212")})}
		};
		states["3.1"] = new State("3.1", "none", "none", convos, locations);
		current_state = states["3.1"];
	}
	
	public string where_should_I_be(string who){
		return current_state.go_forth(who);
	}
	
	public void play_audio(string room){
		Converstaion conv = current_state.play_room(room);
		if(conv != null){
			c = conv;
			playing = true;
			rooms_visited++;
		}
	}
	
	void emergency(){
		idling = !idling;
		this.GetComponent<red_alert>().emergency();
		int i = 0;
		idiorts = GameObject.FindGameObjectsWithTag("idiort");
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
	}
	
	// Update is called once per frame
	void Update () {
		if(playing){
			playing = c.play();
		} else if(rooms_visited > 1 && idling){
			//emergency();
		}
		if (Input.GetKeyDown(KeyCode.B)){
			if(idling){
				emergency();
			} else {
				this.GetComponent<red_alert>().emergency();
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
		if(current_speaker >= convo.Count){
			return false;	
		}
		
		AudioSource speaker;
		if(current_speaker == 0){
			speaker = sources[convo[current_speaker].speaker];
		} else {
			speaker = sources[convo[current_speaker - 1].speaker];
		}
		if(speaker.isPlaying){
			return true;
		} else {
			speaker.clip = convo[current_speaker].noise;
			speaker.Play();
			current_speaker++;
			return true;
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
	private string day;
	private string opening_speech;
	private string closing_speech;
	
	public State(string d, string opening, string close, Dictionary<string, Converstaion> convos, Dictionary<string, string> rooms){
		day = d;
		opening_speech = opening;
		closing_speech = close;
		children = new Dictionary<string, State>();
		room_assignments = rooms;
		dialog = convos;
	}
	
	public string go_forth(string who){
		if(room_assignments.ContainsKey(who)){
			return room_assignments[who];
		} else {
			return null;
		}
	}
	
	public Converstaion play_room(string room){
		if(dialog.ContainsKey(room)){
			return dialog[room];
		} else {
			return null;
		}
	}
	
	public void add_child(string name, State child){
		children[name] = child;
	}
	
	public State get_child(string brigged){
		return children[brigged];	
	}
}