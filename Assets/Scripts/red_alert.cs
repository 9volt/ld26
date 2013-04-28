using UnityEngine;
using System.Collections;

public class red_alert : MonoBehaviour {
	private GameObject[] lights;
	private bool alert;
	private bool rising;
	
	
	// Use this for initialization
	void Start () {
		lights = GameObject.FindGameObjectsWithTag("lights");
		alert = false;
		rising = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.B)){
			alert = !alert;
		}
		foreach(GameObject l in lights){
			Light light = l.GetComponent<Light>();
			if (alert){
				if (rising){
					light.intensity = light.intensity + .01f;
				}else{
					light.intensity = light.intensity - .01f;
				}
			}
			if (light.intensity > 2f){
				rising = false;	
			}
			if (light.intensity < 1f){
				rising = true;
			}
			if(!alert){
				light.intensity = 1f;
				light.color = Color.white;
			}else {
				light.color = Color.red;
			}
		}
	}
}
