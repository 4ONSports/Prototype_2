using UnityEngine;
using System.Collections;

public class InputManagerThree : MonoBehaviour {

	[SerializeField] private BallTestThree ball = null;
	private InputHandler ih = null;
	
	void Start() {
		ih = this.GetComponent<InputHandler> ();
	}
	
	void Update() {
		if (InputHandler.swipeInfo[0].isTap == true) {
			ball.OnTap(Camera.main.ScreenToWorldPoint(new Vector3(InputHandler.swipeInfo[0].swipe_startPos.x,InputHandler.swipeInfo[0].swipe_startPos.y,-Camera.main.transform.position.z)));
			return;
		}
	}
}
