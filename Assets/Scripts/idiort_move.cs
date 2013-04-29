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
					new file_loc("cargo_man_mate", "212")})},
			{"med_bay", new Converstaion(new file_loc[3]{
					new file_loc("leftenant", "202"),
					new file_loc("leftenant", "36") ,//player
					new file_loc("leftenant", "207")})},
			{"engineering", new Converstaion(new file_loc[2]{
					new file_loc("engineer", "33"),
					new file_loc("doctor", "209")})},
			{"common_room", new Converstaion(new file_loc[2]{
					new file_loc("cook", "210"),
					new file_loc("cook", "34")})}, //player
			{"captains_quarters", new Converstaion(new file_loc[2]{
					new file_loc("captain", "37"),
					new file_loc("captain", "211")})}, //player
		};
		states["3.1"] = new State("3.1", "200", "38", convos, locations);
		
		
		//Day 3.2
		 locations = new Dictionary<string, string>(){
			{"doctor", "med_bay"},
			{"leftenant", "common_room"},
			{"engineer", "engineering"},
			{"cook", "med_bay"},
			{"cargo_man", "engineering"},
			{"cargo_man_mate", "cargo_hold"},
			{"captain", "captains_quarters"},
			{"navigator", "common_room"}
		};
		convos = new Dictionary<string, Converstaion>(){
			
			{"med_bay", new Converstaion(new file_loc[3]{
					new file_loc("cook", "113"),
					new file_loc("doctor", "236") ,
					new file_loc("cook", "237")})},
			{"engineering", new Converstaion(new file_loc[2]{
					new file_loc("engineer", "14"),
					new file_loc("cargo_man", "234")})},
			{"common_room", new Converstaion(new file_loc[3]{
					new file_loc("navigator", "114"),
					new file_loc("leftenant", "226"),
					new file_loc("navigator", "227")})},
			{"captains_quarters", new Converstaion(new file_loc[1]{
					new file_loc("captain", "115")})}
					
		};
		states["3.2"] = new State("3.2", "228", "116", convos, locations);
		
		//Day 4.0
		 locations = new Dictionary<string, string>(){
			{"doctor", "med_bay"},
			{"leftenant", "captains_quarters"},
			{"cook", "common_room"},
			{"cargo_man", "cargo_hold"},
			{"cargo_man_mate", "cargo_hold"},
			{"captain", "captains_quarters"}
		};
		convos = new Dictionary<string, Converstaion>(){
			
			{"med_bay", new Converstaion(new file_loc[1]{
					new file_loc("doctor", "72")})},
			{"cargo_hold", new Converstaion(new file_loc[2]{
					new file_loc("cargo_man", "73"),
					new file_loc("cargo_man_mate", "274")})},
			{"common_room", new Converstaion(new file_loc[1]{
					new file_loc("cook", "275")})},
			{"captains_quarters", new Converstaion(new file_loc[2]{
					new file_loc("leftenant", "74"),
					new file_loc("captain", "75")})}
					
		};
		states["4.0"] = new State("4.0", "156", "76", convos, locations);
		
		//Day 4.1
		 locations = new Dictionary<string, string>(){
			{"leftenant", "common_room"},
			{"cook", "common_room"},
			{"cargo_man", "captains_quarters"},
			{"cargo_man_mate", "engineering"},
			{"engineer", "engineering"},
			{"captain", "captains_quarters"}
		};
		convos = new Dictionary<string, Converstaion>(){
			
			{"engineering", new Converstaion(new file_loc[2]{
					new file_loc("cargo_man_mate", "86"),
					new file_loc("engineer", "285")})},
			{"common_room", new Converstaion(new file_loc[2]{
					new file_loc("leftenant", "286"),
					new file_loc("cook", "287")})},
			{"captains_quarters", new Converstaion(new file_loc[2]{
					new file_loc("cargo_man", "288"),
					new file_loc("captain", "289")})}
					
		};
		states["4.1"] = new State("4.1", "156", "291", convos, locations);
		
		//Day 4.2 (different from 5.5  and 5.9 in the intro clip)
		 locations = new Dictionary<string, string>(){
			{"leftenant", "common_room"},
			{"cook", "common_room"},
			{"cargo_man", "cargo_hold"},
			{"cargo_man_mate", "cargo_hold"},
			{"captain", "captains_quarters"}
		};
		convos = new Dictionary<string, Converstaion>(){
			
			{"cargo_hold", new Converstaion(new file_loc[2]{
					new file_loc("cargo_man", "73"),
					new file_loc("cargo_man_mate", "274")})},
			{"common_room", new Converstaion(new file_loc[2]{
					new file_loc("cook", "52"),
					new file_loc("leftenant", "220")})},
			{"captains_quarters", new Converstaion(new file_loc[3]{
					new file_loc("captain", "51"),
					new file_loc("captain", "221"), //player
					new file_loc("captain", "222")})}
					
		};
		states["4.2"] = new State("4.2", "214", "53", convos, locations);
		
		
		//Day 4.3  //mutiny state
		 locations = new Dictionary<string, string>(){
			{"leftenant", "bridge"},
			{"cook", "bridge"},
			{"cargo_man", "bridge"},
			{"cargo_man_mate", "bridge"},
			{"engineer", "bridge"}
		};
		convos = new Dictionary<string, Converstaion>(){
			
			{"bridge", new Converstaion(new file_loc[1]{
					new file_loc("leftenant", "61")})}//player
					
		};
		states["4.3"] = new State("4.3", "224", "225", convos, locations);
		
		
		//Day 4.4
		 locations = new Dictionary<string, string>(){
			{"leftenant", "common_room"},
			{"doctor", "med_bay"},
			{"cook", "common_room"},
			{"cargo_man", "cargo_hold"},
			{"cargo_man_mate", "cargo_hold"},
			{"captain", "captains_quarters"}
		};
		convos = new Dictionary<string, Converstaion>(){
			{"med_bay", new Converstaion(new file_loc[1]{
					new file_loc("doctor", "307")})},
			{"cargo_hold", new Converstaion(new file_loc[2]{
					new file_loc("cargo_man", "73"),
					new file_loc("cargo_man_mate", "274")})},
			{"common_room", new Converstaion(new file_loc[2]{
					new file_loc("cook", "52"),
					new file_loc("leftenant", "220")})},
			{"captains_quarters", new Converstaion(new file_loc[1]{
					new file_loc("captain", "309")})}
					
		};
		states["4.4"] = new State("4.4", "110", "309", convos, locations);
		
		//Day 4.5
		 locations = new Dictionary<string, string>(){
			{"leftenant", "captains_quarters"},
			{"doctor", "med_bay"},
			{"cook", "cargo_hold"},
			{"cargo_man", "cargo_hold"},
			{"cargo_man_mate", "cargo_hold"},
			{"engineer", "med_bay"},
			{"captain", "captains_quarters"}
		};
		convos = new Dictionary<string, Converstaion>(){
			{"med_bay", new Converstaion(new file_loc[2]{
					new file_loc("engineer", "102"),
					new file_loc("doctor", "94")})},
			{"cargo_hold", new Converstaion(new file_loc[3]{
					new file_loc("cargo_man", "288"),
					new file_loc("cook", "100"),
					new file_loc("cargo_man_mate", "101")})},
			{"captains_quarters", new Converstaion(new file_loc[4]{
					new file_loc("leftenant", "96"),
					new file_loc("captain", "97"),
					new file_loc("captain", "98"), //player
					new file_loc("leftenant", "99")})}
					
		};
		states["4.5"] = new State("4.5", "26", "104", convos, locations);
		
		//Day 4.6
		 locations = new Dictionary<string, string>(){
			{"leftenant", "captains_quarters"},
			{"doctor", "med_bay"},
			{"cook", "med_bay"},
			{"cargo_man", "cargo_hold"},
			{"cargo_man_mate", "cargo_hold"},
			{"navigator", "med_bay"},
			{"captain", "captains_quarters"}
		};
		convos = new Dictionary<string, Converstaion>(){
			{"med_bay", new Converstaion(new file_loc[4]{
					new file_loc("doctor", "128"),
					new file_loc("navigator", "240"),
					new file_loc("doctor", "246"), 
					new file_loc("navigator", "247")})},
			{"cargo_hold", new Converstaion(new file_loc[3]{
					new file_loc("cargo_man", "127"),
					new file_loc("cargo_man_mate", "239"),
					new file_loc("cargo_man", "248")})},
			{"captains_quarters", new Converstaion(new file_loc[4]{
					new file_loc("leftenant", "96"),
					new file_loc("captain", "97"),
					new file_loc("captain", "98"), //player
					new file_loc("leftenant", "99")})}
					
		};
		states["4.6"] = new State("4.6", "26", "104", convos, locations);
		
		//Day 5.0 - nodeath
		 locations = new Dictionary<string, string>(){
			{"leftenant", "bridge"},
			{"doctor", "bridge"},
			{"cook", "bridge"},
			{"cargo_man", "bridge"},
			{"cargo_man_mate", "bridge"},
			{"captain", "bridge"}
		};
		convos = new Dictionary<string, Converstaion>(){
			{"bridge", new Converstaion(new file_loc[2]{
					new file_loc("captain", "82"),
					new file_loc("cook", "277")})}
		
		};
		states["5.0"] = new State("5.0", null , "279", convos, locations);
		
		//Day 5.1 - nodeath
		 locations = new Dictionary<string, string>(){
			{"leftenant", "bridge"},
			{"doctor", "bridge"},
			{"cook", "bridge"},
			{"cargo_man", "bridge"},
			{"cargo_man_mate", "bridge"},
			{"captain", "bridge"}
		};
		convos = new Dictionary<string, Converstaion>(){
			{"bridge", new Converstaion(new file_loc[2]{
					new file_loc("captain", "82"),
					new file_loc("doctor", "278")})}
		
		};
		states["5.1"] = new State("5.1", null , "279", convos, locations);
		
		//Day 5.2
		 locations = new Dictionary<string, string>(){
			{"leftenant", "bridge"},
			{"cook", "bridge"},
			{"cargo_man", "bridge"},
			{"cargo_man_mate", "bridge"},
			{"captain", "bridge"}
		};
		convos = new Dictionary<string, Converstaion>(){
		};
		states["5.2"] = new State("5.2", "222" , "53", convos, locations);
		
		//Day 5.3 and 5.12  //mutiny state (differennt form 4.3 cause doctor is alive)
		 locations = new Dictionary<string, string>(){
			{"leftenant", "bridge"},
			{"cook", "bridge"},
			{"doctor", "bridge"},
			{"cargo_man", "bridge"},
			{"cargo_man_mate", "bridge"},
			{"engineer", "bridge"}
		};
		convos = new Dictionary<string, Converstaion>(){
			
			{"bridge", new Converstaion(new file_loc[1]{
					new file_loc("leftenant", "61")})}//player
					
		};
		states["5.3"] = new State("5.3", "224", "225", convos, locations);
		
		//5.4 is an ending
		
		//Day 5.5 (different from 4.2 and 5.9 in the intro clip)
		 locations = new Dictionary<string, string>(){
			{"leftenant", "common_room"},
			{"cook", "common_room"},
			{"cargo_man", "cargo_hold"},
			{"cargo_man_mate", "cargo_hold"},
			{"captain", "captains_quarters"}
		};
		convos = new Dictionary<string, Converstaion>(){
			
			{"cargo_hold", new Converstaion(new file_loc[2]{
					new file_loc("cargo_man", "73"),
					new file_loc("cargo_man_mate", "274")})},
			{"common_room", new Converstaion(new file_loc[2]{
					new file_loc("cook", "52"),
					new file_loc("leftenant", "220")})},
			{"captains_quarters", new Converstaion(new file_loc[3]{
					new file_loc("captain", "51"),
					new file_loc("captain", "221"), //player
					new file_loc("captain", "222")})}
					
		};
		states["5.5"] = new State("5.5", "300", "53", convos, locations);
		
		//5.6 is an ending
		//5.7 is an ending
		//5.8 is an ending
		
		
		//Day 5.9 (different from 4.2  and 5.5 in the intro clip)
		 locations = new Dictionary<string, string>(){
			{"leftenant", "common_room"},
			{"cook", "common_room"},
			{"cargo_man", "cargo_hold"},
			{"cargo_man_mate", "cargo_hold"},
			{"captain", "captains_quarters"}
		};
		convos = new Dictionary<string, Converstaion>(){
			
			{"cargo_hold", new Converstaion(new file_loc[2]{
					new file_loc("cargo_man", "73"),
					new file_loc("cargo_man_mate", "274")})},
			{"common_room", new Converstaion(new file_loc[2]{
					new file_loc("cook", "52"),
					new file_loc("leftenant", "220")})},
			{"captains_quarters", new Converstaion(new file_loc[3]{
					new file_loc("captain", "51"),
					new file_loc("captain", "221"), //player
					new file_loc("captain", "222")})}
					
		};
		states["5.9"] = new State("5.9", "320", "53", convos, locations);
		
		//Day 5.10 - nodeath
		 locations = new Dictionary<string, string>(){
			{"leftenant", "bridge"},
			{"doctor", "bridge"},
			{"cook", "bridge"},
			{"cargo_man", "bridge"},
			{"cargo_man_mate", "bridge"},
			{"captain", "bridge"}
		};
		convos = new Dictionary<string, Converstaion>(){
			{"bridge", new Converstaion(new file_loc[2]{
					new file_loc("captain", "82"),
					new file_loc("cook", "321")})}
		
		};
		states["5.10"] = new State("5.10", null , "279", convos, locations);
		
		//Day 5.11 - nodeath
		 locations = new Dictionary<string, string>(){
			{"leftenant", "bridge"},
			{"doctor", "bridge"},
			{"cook", "bridge"},
			{"cargo_man", "bridge"},
			{"cargo_man_mate", "bridge"},
			{"captain", "bridge"}
		};
		convos = new Dictionary<string, Converstaion>(){
			{"bridge", new Converstaion(new file_loc[2]{
					new file_loc("captain", "82"),
					new file_loc("doctor", "319")})}
		
		};
		states["5.11"] = new State("5.11", null , "279", convos, locations);
		
		//5.12 = 5.3
		
		//Day 5.13
		 locations = new Dictionary<string, string>(){
			{"doctor", "med_bay"},
			{"cook", "common_room"},
			{"navigator", "cargo_hold"},
			{"captain", "captains_quarters"}
		};
		convos = new Dictionary<string, Converstaion>(){
			{"med_bay", new Converstaion(new file_loc[1]{
					new file_loc("doctor", "140")})},
			{"common_room", new Converstaion(new file_loc[1]{
					new file_loc("cook", "142")})},
			{"cargo_hold", new Converstaion(new file_loc[1]{
					new file_loc("navigator", "143")})},
			{"captains_quarters", new Converstaion(new file_loc[1]{
					new file_loc("captain", "141")})}
					
		};
		states["5.13"] = new State("5.13", "255", "114", convos, locations);
		
		//5.14 is an ending
		//5.15 is an ending
		
		//5.16 was skipped
		
		//Day 5.17
		 locations = new Dictionary<string, string>(){
			{"doctor", "captains_quarters"},
			{"cook", "common_room"},
			{"navigator", "cargo_hold"},
			{"leftenant", "common_room"},
			{"captain", "captains_quarters"}
		};
		convos = new Dictionary<string, Converstaion>(){
			{"common_room", new Converstaion(new file_loc[2]{
					new file_loc("cook", "153"),
					new file_loc("leftenant", "268")})},
			{"cargo_hold", new Converstaion(new file_loc[1]{
					new file_loc("navigator", "152")})},
			{"captains_quarters", new Converstaion(new file_loc[2]{
					new file_loc("captain", "154"),
					new file_loc("doctor", "140")})}
					
		};
		states["5.17"] = new State("5.17", "263", "141", convos, locations);
		
		//6.0 is an ending
		//6.1 is an ending
		  //6.2 was skipped
		//6.3 is an ending (same as 6.0 except Doctor dead) is same as 5.6?
		//6.4 is an ending  = 5.7
		   //6.5 is an ending  = 5.8
		//6.6 is an ending (same as 6.0 except navigator is alive, leftenant and cargomen dead)
		//6.7 is an ending (same as 6.6 except leftenant is alive)
		current_state = states["3.1"];
		current_state.add_child("engineer", states["4.1"]);
	}
	
	public void brig(string s){
		Debug.Log(s);
		current_state = current_state.get_child(s);
		current_state.play_intro();
		emergency();
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
			if(convo[current_speaker].noise == null){
				Debug.LogError("Missing Audio " + convo[current_speaker]);
			}
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
	private AudioSource audio;

	
	
	public State(string d, string opening, string close, Dictionary<string, Converstaion> convos, Dictionary<string, string> rooms){
		day = d;
		opening_speech = opening;
		closing_speech = close;
		children = new Dictionary<string, State>();
		room_assignments = rooms;
		dialog = convos;
		audio = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
	}
	
	public string go_forth(string who){
		if(room_assignments.ContainsKey(who)){
			return room_assignments[who];
		} else {
			return null;
		}
	}
	
	public void play_intro(){
		AudioClip a = Resources.Load("opening") as AudioClip;
		audio.clip = a;
		audio.Play();
	}
	
	public void play_outro(){
		AudioClip a = Resources.Load("closing") as AudioClip;
		audio.clip = a;
		audio.Play();
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