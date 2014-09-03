using UnityEngine;
using System.Collections;

public class InputManagerTwo : MonoBehaviour {
	
	private InputHandler ih = null;
	
	void Start() {
		ih = this.GetComponent<InputHandler> ();
	}
	
	void Update() {
		if (InputHandler.isTap == true) {
			//ball.OnTap(Camera.main.ScreenToWorldPoint(new Vector3(InputHandler.swipe_startPos.x,InputHandler.swipe_startPos.y,-Camera.main.transform.position.z)));
			return;
		}
	}

	void OnGUI() {
		foreach(Touch t in Input.touches){
			GUI.Label(new Rect(t.position.x,t.position.y,100,100), t.fingerId.ToString());
		}
	}
}
