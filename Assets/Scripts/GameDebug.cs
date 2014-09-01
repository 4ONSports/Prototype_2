using UnityEngine;
using System.Collections;

public class GameDebug : MonoBehaviour {

	public static bool enableDebugLines = false;

	void OnGUI() {
		if (GUI.Button (new Rect (Screen.width - 150, 0, 150, 150), "InputListener.cs Restart()")) {
			Restart ();
		}

		if(GUI.Button(new Rect(20,120,160,40), "Toggle DebugLines")) {
			enableDebugLines = !enableDebugLines;
		}
	}
	
	public static void Restart() {
		Application.LoadLevel (Application.loadedLevel);
	}
}
