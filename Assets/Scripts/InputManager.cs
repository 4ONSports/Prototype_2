using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	[SerializeField] private Players players = null;
	private InputHandler ih = null;

	void Start() {
		ih = this.GetComponent<InputHandler> ();
	}

	void Update() {
		if (InputHandler.swipeInfo[0].isTap == true) {
//			print ("Tap");
			players.OnTap();
			return;
		}

		if (InputHandler.swipeInfo[0].swipe_state == InputHandler.SwipeState.END && !InputHandler.swipeInfo[0].isTap) {
//			print ("Shoot");
			players.OnSwipe (InputHandler.swipeInfo[0].swipe_direction);
		}

		if (InputHandler.swipeInfo[0].isSwiping) {
//			print ("Swiping");
		}
	}
}
