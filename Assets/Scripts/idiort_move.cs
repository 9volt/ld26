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
	public bool idling;
	public string day;
	private Converstaion c;
	private State current_state;
	private Dictionary<string, State> states;
	private bool playing;
	private List<string> rooms_visited;
	public bool walking_to_brig;
	private string next_day;
	private AudioSource sound;
	private bool day_starting;
	// Use this for initialization
	void Start () {
		idiorts = GameObject.FindGameObjectsWithTag("idiort");
		rooms_visited = new List<string>();
		idling = true;
		day = "1";
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
		walking_to_brig = false;
		next_day = null;
		day_starting = false;
		sound = this.GetComponent<AudioSource>();
		create_states();
	}
	
	private void create_states(){
 		//Day 1.0 //no brigging at end of day 1
		Dictionary<string, string> locations = new Dictionary<string, string>(){
			{"doctor", "med_bay"},
			{"leftenant", "med_bay"},
			{"engineer", "engineering"},
			{"cook", "common_room"},
			{"cargo_man", "cargo_hold"},
			{"cargo_man_mate", "engineering"},
            {"navigator", "common_room"},
            {"cabin_boy", "cargo_hold"},
            {"first_mate", "bridge"},
			{"captain", "bridge"}
		};
		Dictionary<string, Converstaion> convos = new Dictionary<string, Converstaion>(){
			{"cargo_hold", new Converstaion(new file_loc[2]{
					new file_loc("cargo_man", "4"),
					new file_loc("cabin_boy", "169")})},
			{"med_bay", new Converstaion(new file_loc[3]{
					new file_loc("leftenant", "186"),
					new file_loc("doctor", "187") ,
					new file_loc("leftenant", "188")})},
			{"engineering", new Converstaion(new file_loc[5]{
					new file_loc("cargo_man_mate", "2"),
                    new file_loc("engineer", "182"),
					new file_loc("cargo_man_mate", "183") ,
					new file_loc("engineer", "184") ,
					new file_loc("engineer", "185")})},  //player
			{"common_room", new Converstaion(new file_loc[3]{
					new file_loc("navigator", "3"),
                    new file_loc("cook", "180"),
					new file_loc("navigator", "181")})}, 
			{"bridge", new Converstaion(new file_loc[4]{
					new file_loc("captain", "1"),
                    new file_loc("captain", "161"), //player
                    new file_loc("captain", "162"),
					new file_loc("captain", "163")})}, //player
		};
		states["1.0"] = new State("1.0", null, null, convos, locations);
        
        //Day 2.0
		locations = new Dictionary<string, string>(){
			{"doctor", "med_bay"},
			{"leftenant", "cargo_hold"},
			{"engineer", "engineering"},
			{"cook", "common_room"},
			{"cargo_man", "cargo_hold"},
			{"cargo_man_mate", "common_room"},
            {"navigator", "common_room"},
            {"first_mate", "engineering"},
			{"captain", "captains_quarters"}
		};
		convos = new Dictionary<string, Converstaion>(){
			{"cargo_hold", new Converstaion(new file_loc[2]{
					new file_loc("cargo_man", "9"),
					new file_loc("leftenant", "192")})},
			{"med_bay", new Converstaion(new file_loc[1]{
					new file_loc("doctor", "6")})},
			{"engineering", new Converstaion(new file_loc[2]{
					new file_loc("first_mate", "7") ,
					new file_loc("engineer", "191")})},  //player
			{"common_room", new Converstaion(new file_loc[4]{
					new file_loc("navigator", "193"),
                    new file_loc("cook", "194"),
                    new file_loc("navigator", "195"),
					new file_loc("cargo_man_mate", "196")})}, 
			{"captains_quarters", new Converstaion(new file_loc[2]{
					new file_loc("captain", "8"),
					new file_loc("captain", "197")})}, //player
		};
		states["2.0"] = new State("2.0", "171", "11", convos, locations);
    
    
		//Day 3.1
		locations = new Dictionary<string, string>(){
			{"doctor", "engineering"},
			{"leftenant", "med_bay"},
			{"engineer", "engineering"},
			{"cook", "common_room"},
			{"cargo_man", "cargo_hold"},
			{"cargo_man_mate", "cargo_hold"},
			{"captain", "captains_quarters"}
		};
		convos = new Dictionary<string, Converstaion>(){
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
			{"leftenant", "security"},
			{"cook", "security"},
			{"cargo_man", "security"},
			{"cargo_man_mate", "security"},
			{"engineer", "security"}
		};
		convos = new Dictionary<string, Converstaion>(){
			
			{"security", new Converstaion(new file_loc[1]{
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
			{"leftenant", "security"},
			{"doctor", "security"},
			{"cook", "brig"},
			{"cargo_man", "security"},
			{"cargo_man_mate", "security"},
			{"captain", "security"}
		};
		convos = new Dictionary<string, Converstaion>(){
			{"security", new Converstaion(new file_loc[2]{
					new file_loc("captain", "82"),
					new file_loc("cook", "277")})}
		
		};
		states["5.0"] = new State("5.0", null , "279", convos, locations);
		
		//Day 5.1 - nodeath
		 locations = new Dictionary<string, string>(){
			{"leftenant", "security"},
			{"doctor", "brig"},
			{"cook", "security"},
			{"cargo_man", "security"},
			{"cargo_man_mate", "security"},
			{"captain", "security"}
		};
		convos = new Dictionary<string, Converstaion>(){
			{"security", new Converstaion(new file_loc[2]{
					new file_loc("captain", "82"),
					new file_loc("doctor", "278")})}
		
		};
		states["5.1"] = new State("5.1", null , "279", convos, locations);
		
		//Day 5.2
		 locations = new Dictionary<string, string>(){
			{"leftenant", "security"},
			{"cook", "security"},
			{"cargo_man", "security"},
			{"cargo_man_mate", "security"},
			{"captain", "security"}
		};
		convos = new Dictionary<string, Converstaion>(){
		};
		states["5.2"] = new State("5.2", "222" , "53", convos, locations);
		
		//Day 5.3 and 5.12  //mutiny state (differennt form 4.3 cause doctor is alive)
		 locations = new Dictionary<string, string>(){
			{"leftenant", "security"},
			{"cook", "security"},
			{"doctor", "security"},
			{"cargo_man", "security"},
			{"cargo_man_mate", "security"},
			{"engineer", "security"}
		};
		convos = new Dictionary<string, Converstaion>(){
			
			{"security", new Converstaion(new file_loc[1]{
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
			{"leftenant", "security"},
			{"doctor", "security"},
			{"cook", "brig"},
			{"cargo_man", "security"},
			{"cargo_man_mate", "security"},
			{"captain", "security"}
		};
		convos = new Dictionary<string, Converstaion>(){
			{"security", new Converstaion(new file_loc[2]{
					new file_loc("captain", "82"),
					new file_loc("cook", "321")})}
		
		};
		states["5.10"] = new State("5.10", null , "279", convos, locations);
		
		//Day 5.11 - nodeath
		 locations = new Dictionary<string, string>(){
			{"leftenant", "security"},
			{"doctor", "brig"},
			{"cook", "security"},
			{"cargo_man", "security"},
			{"cargo_man_mate", "security"},
			{"captain", "security"}
		};
		convos = new Dictionary<string, Converstaion>(){
			{"security", new Converstaion(new file_loc[2]{
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
		
        //FAKE ENDING STATES
        //7.0
		locations = new Dictionary<string, string>();
		convos = new Dictionary<string, Converstaion>();
		states["7.0"] = new State("7.0", "good_ending", null, convos, locations);
        //7.1
		locations = new Dictionary<string, string>();
		convos = new Dictionary<string, Converstaion>();
		states["7.1"] = new State("7.1", "bad_ending", null, convos, locations);
              
        //adding child states
        states["1.0"].add_child("day_one", states["2.0"]);
        
        states["2.0"].add_child("cook", states["7.1"]);
        states["2.0"].add_child("cargo_man", states["3.1"]);
        states["2.0"].add_child("engineer", states["3.1"]);
        states["2.0"].add_child("doctor", states["3.1"]);
        states["2.0"].add_child("leftenant", states["3.1"]);
        states["2.0"].add_child("cargo_man_mate", states["3.2"]);
        states["2.0"].add_child("navigator", states["3.2"]);
        
        states["3.1"].add_child("cook", states["4.0"]);
        states["3.1"].add_child("engineer", states["4.1"]);
        states["3.1"].add_child("doctor", states["4.3"]);
        states["3.1"].add_child("leftenant", states["4.2"]);
        states["3.1"].add_child("cargo_man", states["4.2"]);
        states["3.1"].add_child("cargo_man_mate", states["4.2"]);
        
        states["3.2"].add_child("cook", states["4.5"]);
        states["3.2"].add_child("engineer", states["4.5"]);
        states["3.2"].add_child("doctor", states["4.4"]);
        states["3.2"].add_child("leftenant", states["4.4"]);
        states["3.2"].add_child("cargo_man", states["4.6"]);
        states["3.2"].add_child("cargo_man_mate", states["4.4"]);
        states["3.2"].add_child("navigator", states["4.6"]);
        
        states["4.0"].add_child("cook", states["5.0"]);
        states["4.0"].add_child("doctor", states["5.1"]);
        states["4.0"].add_child("cargo_man", states["5.2"]);
        states["4.0"].add_child("cargo_man_mate", states["5.2"]);
        states["4.0"].add_child("navigator", states["5.2"]);
        
        states["4.1"].add_child("cook", states["5.3"]);
        states["4.1"].add_child("engineer", states["7.1"]);
        states["4.1"].add_child("leftenant", states["5.5"]);
        states["4.1"].add_child("cargo_man", states["5.5"]);
        states["4.1"].add_child("cargo_man_mate", states["5.5"]);
        
        states["4.2"].add_child("cook", states["7.0"]);
        states["4.2"].add_child("leftenant", states["7.1"]);
        states["4.2"].add_child("cargo_man", states["7.1"]);
        states["4.2"].add_child("cargo_man_mate", states["7.1"]);
        
        states["4.3"].add_child("cook", states["7.0"]);
        states["4.3"].add_child("leftenant", states["7.1"]);
        states["4.3"].add_child("doctor", states["7.1"]);
        states["4.3"].add_child("cargo_man", states["7.1"]);
        states["4.3"].add_child("cargo_man_mate", states["7.1"]);
		states["4.3"].add_child("engineer", states["7.1"]);
        
        states["4.4"].add_child("cook", states["5.10"]);
        states["4.4"].add_child("leftenant", states["5.9"]);
        states["4.4"].add_child("doctor", states["5.11"]);
        states["4.4"].add_child("cargo_man", states["5.9"]);
        states["4.4"].add_child("cargo_man_mate", states["5.9"]);
        
        states["4.5"].add_child("cook", states["5.3"]);
        states["4.5"].add_child("leftenant", states["5.3"]);
        states["4.5"].add_child("doctor", states["5.3"]);
        states["4.5"].add_child("cargo_man", states["5.3"]);
        states["4.5"].add_child("cargo_man_mate", states["5.3"]);
        states["4.5"].add_child("engineer", states["7.1"]);
        
        states["4.6"].add_child("cook", states["5.17"]);
        states["4.6"].add_child("leftenant", states["5.17"]);
        states["4.6"].add_child("doctor", states["5.13"]);
        states["4.6"].add_child("cargo_man", states["7.1"]);
        states["4.6"].add_child("cargo_man_mate", states["7.1"]);
        states["4.6"].add_child("navigator", states["7.1"]);
        
        states["5.0"].add_child("cook", states["7.0"]);
        states["5.0"].add_child("leftenant", states["7.1"]);
        states["5.0"].add_child("doctor", states["7.1"]);
        states["5.0"].add_child("cargo_man", states["7.1"]);
        states["5.0"].add_child("cargo_man_mate", states["7.1"]);
        
        states["5.1"].add_child("cook", states["7.0"]);
        states["5.1"].add_child("leftenant", states["7.1"]);
        states["5.1"].add_child("doctor", states["7.1"]);
        states["5.1"].add_child("cargo_man", states["7.1"]);
        states["5.1"].add_child("cargo_man_mate", states["7.1"]);
        
        states["5.2"].add_child("cook", states["7.0"]);
        states["5.2"].add_child("leftenant", states["7.1"]);
        states["5.2"].add_child("doctor", states["7.1"]);
        states["5.2"].add_child("cargo_man", states["7.1"]);
        states["5.2"].add_child("cargo_man_mate", states["7.1"]);
		
		states["5.3"].add_child("cook", states["7.0"]);
        states["5.3"].add_child("leftenant", states["7.1"]);
        states["5.3"].add_child("doctor", states["7.1"]);
        states["5.3"].add_child("cargo_man", states["7.1"]);
        states["5.3"].add_child("cargo_man_mate", states["7.1"]);	
		
        states["5.5"].add_child("cook", states["7.0"]);
        states["5.5"].add_child("leftenant", states["7.1"]);
        states["5.5"].add_child("cargo_man", states["7.1"]);
        states["5.5"].add_child("cargo_man_mate", states["7.1"]);
        
        states["5.9"].add_child("cook", states["7.0"]);
        states["5.9"].add_child("leftenant", states["7.1"]);
        states["5.9"].add_child("cargo_man", states["7.1"]);
        states["5.9"].add_child("cargo_man_mate", states["7.1"]);
        
        states["5.10"].add_child("cook", states["7.0"]);
        states["5.10"].add_child("leftenant", states["7.1"]);
        states["5.10"].add_child("doctor", states["7.1"]);
        states["5.10"].add_child("cargo_man", states["7.1"]);
        states["5.10"].add_child("cargo_man_mate", states["7.1"]);
        
        states["5.11"].add_child("cook", states["7.0"]);
        states["5.11"].add_child("leftenant", states["7.1"]);
        states["5.11"].add_child("doctor", states["7.1"]);
        states["5.11"].add_child("cargo_man", states["7.1"]);
        states["5.11"].add_child("cargo_man_mate", states["7.1"]);
        
        states["5.13"].add_child("cook", states["7.0"]);
        states["5.13"].add_child("navigator", states["7.1"]);
        states["5.13"].add_child("doctor", states["7.1"]);
        
        //states["5.16"].add_child("cook", states["7.0"]);
        //states["5.16"].add_child("navigator", states["7.1"]);
        //states["5.16"].add_child("doctor", states["7.1"]);
        
        states["5.17"].add_child("cook", states["7.0"]);
        states["5.17"].add_child("navigator", states["7.1"]);
        states["5.17"].add_child("doctor", states["7.1"]);
        states["5.17"].add_child("leftenant", states["7.1"]);
		
		
		current_state = states["2.0"];
	}
	
	public void brig(string s){
		walking_to_brig = true;
		next_day = s;
		NavMeshAgent i = GameObject.Find(s).GetComponent<NavMeshAgent>();
		i.SetDestination(GameObject.FindGameObjectWithTag("brig").GetComponent<Transform>().position);
	}
	
	void end_day(){
		current_state = current_state.get_child(next_day);
		day = current_state.day;
		Debug.Log(day);
		current_state.play_intro();
		day_starting = true;
		rooms_visited = new List<string>();
		walking_to_brig = false;
	}
	
	public string where_should_I_be(string who){
		return current_state.go_forth(who);
	}
	
	public void play_audio(string room){
		if(walking_to_brig){
			if(room == "security_office"){
				end_day();
			}
		} else {
			Converstaion conv = current_state.play_room(room);
			if(conv != null && !rooms_visited.Contains(room)){
				c = conv;
				playing = true;
				rooms_visited.Add(room);
			}
		}
	}
	
	void move_to_bridge(){
		idling = !idling;
		playing = false;
		c = null;
		this.GetComponent<red_alert>().emergency();
		int i = 0;
		GameObject[] navs = GameObject.FindGameObjectsWithTag("bridge");
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
	
	
	void emergency(){
		if(idling){
			move_to_bridge();
		} else {
			idling = !idling;
			rooms_visited = new List<string>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(day_starting && !sound.isPlaying){
			idling = true;
			day_starting = false;
			this.GetComponent<red_alert>().end_alert();
		}
		if(playing){
			playing = c.play();
		} else if(rooms_visited.Count >= 2 && idling){
			move_to_bridge();
		}
		if (Input.GetKeyDown(KeyCode.B)){
			if(idling){
				emergency();
			} else {
				this.GetComponent<red_alert>().end_alert();
				idling = true;	
			}
		}
	}
}

public class Converstaion{
	private Dictionary<string, AudioSource> sources;
	private List<conv> convo;
	public string AUDIO_PATH = "Assets/Audio/";
	private AudioSource current_speaker;
	private int next_speaker;
	
	public Converstaion(file_loc[] dialog){
		sources = new Dictionary<string, AudioSource>();
		convo = new List<conv>();
		next_speaker = 0;
		foreach(file_loc t in dialog){
			AudioClip a = Resources.Load(t.file_name) as AudioClip;
			convo.Add(new conv(t.speaker, a));
			if(!sources.ContainsKey(t.speaker)){
				AudioSource s = GameObject.Find(t.speaker).GetComponent<AudioSource>();
				sources[t.speaker] = s;
			}
		}
	}
	
	private bool set_speaker(){
		current_speaker = sources[convo[next_speaker].speaker];
		if(convo[next_speaker].noise == null){
			Debug.LogError("Missing Audio " + convo[next_speaker]);
		}
		current_speaker.clip = convo[next_speaker].noise;
		current_speaker.Play();
		next_speaker++;
		return true;
	}
	
	public bool play(){
		if(next_speaker == 0){
			return set_speaker();
		}
		if(current_speaker.isPlaying){
			return true;
		} else {
			if(next_speaker >= convo.Count){
				return false;
			} else {
				return set_speaker();
			}
		}
	}
}

public class State {
	private Dictionary<string, State> children;
	// Dictionary { brigged_name -> states }
	private Dictionary<string, string> room_assignments;
	// Dictionary { name -> room }
	private Dictionary<string, Converstaion> dialog;
	// Dictionary{ room -> conversation }
	public string day;
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
		if(opening_speech != null){
			if(day == "7.1" || day == "7.0"){
				GameObject.Find("bgm").GetComponent<AudioSource>().Stop();	
			}
			AudioClip a = Resources.Load(opening_speech) as AudioClip;
			audio.clip = a;
			audio.Play();
		}
	}
	
	public void play_outro(){
		if(closing_speech != null){
			AudioClip a = Resources.Load(closing_speech) as AudioClip;
			audio.clip = a;
			audio.Play();
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