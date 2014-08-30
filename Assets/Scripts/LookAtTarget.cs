using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour {

	public Transform targetToLookAt = null;

	void Start() {
		Screen.orientation = ScreenOrientation.Portrait;
	}
	
	void Update () {
		if(!targetToLookAt) return;
		transform.LookAt (targetToLookAt);
		transform.position = Vector3.Lerp(transform.position, new Vector3(Input.acceleration.x,Input.acceleration.y,transform.position.z),10);
	
	}

	void OnGUI() {
		GUI.Button (new Rect (0, 0, Screen.width * 0.2f, Screen.height * 0.2f), Input.acceleration.ToString ());

	}
}
