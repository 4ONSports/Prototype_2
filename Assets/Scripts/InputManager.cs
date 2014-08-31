using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	[SerializeField] private Players players = null;
	private InputHandler ih = null;

	void Start() {
		ih = this.GetComponent<InputHandler> ();
	}

	void Update() {
		if (InputHandler.isTap == true) {
//			print ("Tap");
			players.OnTap();
			return;
		}

		if (InputHandler.swipe_state == InputHandler.SwipeState.END && !InputHandler.isTap) {
//			print ("Shoot");
			players.OnSwipe (InputHandler.swipe_direction);
		}

		if (InputHandler.isSwiping) {
//			print ("Swiping");
		}
	}
}
