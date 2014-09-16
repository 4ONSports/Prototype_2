using UnityEngine;
using System.Collections;

public class GameDebug : MonoBehaviour {
	
	public GameMode activeGameMode;
	public static bool enableDebugLines = false;
	[SerializeField] private PlayerControl_Movement[] pc_mvmnts;

	void Start() {
	}

	void OnGUI() {
		if (GUI.Button (new Rect (Screen.width - 150, 0, 150, 150), "GameDebug.cs Restart()")) {
			Restart ();
		}

		if(GUI.Button(new Rect(20,120,160,40), "Toggle DebugLines")) {
			enableDebugLines = !enableDebugLines;
		}

		GUI.Box(new Rect(10,10,200,210), "Debug Menu");
		
		if(GUI.Button(new Rect(20,40,160,40), "Toggle Y Move")) {
			foreach( PlayerControl_Movement pc in pc_mvmnts ) {
				pc.allowMovement_Y = !pc.allowMovement_Y;
			}
		}
		
		if(GUI.Button(new Rect(20,80,160,40), "Toggle Move when with ball")) {
			foreach( PlayerControl_Movement pc in pc_mvmnts ) {
				pc.noMovementWhenHasBall = !pc.noMovementWhenHasBall;
			}
		}
	}
	
	public void Restart() {
		if( activeGameMode != null ) {
			activeGameMode.Restart();
		}
	}
}
