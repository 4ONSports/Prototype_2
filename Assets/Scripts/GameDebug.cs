using UnityEngine;
using System.Collections;

public class GameDebug : MonoBehaviour {

	void OnGUI() {
		if (GUI.Button (new Rect (Screen.width - 150, 0, 150, 150), "InputListener.cs Restart()"))
			Restart ();
	}
	
	public static void Restart() {
		Application.LoadLevel (Application.loadedLevel);
	}
}
