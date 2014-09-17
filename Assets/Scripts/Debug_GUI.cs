using UnityEngine;
using System.Collections;

public class Debug_GUI : MonoBehaviour {
	
	public string userName = "";
	
	void Start() {
	}
	
	void OnGUI () {
		GUI.Label (new Rect (10,10,100,150), "Username:");
		userName = GUI.TextField(new Rect(100, 10, 150, 125), userName, 25);
		
		if (GUI.Button (new Rect (320, 50, 100, 125), "Join Game")) {
			JoinGame();
		}
//		if (GUI.Button (new Rect (320, 180, 100, 125), "Load Game")) {
//			//Leave();
////			Application.LoadLevel ("AppWarpTest");
//		}
	}
	
	void JoinGame() {
		if( userName != null ) {
			AppWarp.InitializeConnection (userName, "scene_gameplay");
		}
	}
	
	void Leave() {
		AppWarp.DeInitializeConnection ();
	}
}
