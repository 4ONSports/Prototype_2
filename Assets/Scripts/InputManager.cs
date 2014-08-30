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
			players.OnTap();
			return;
		}

		if (InputHandler.swipe_state == InputHandler.SwipeState.END)
						players.OnSwipe (InputHandler.swipe_direction);

		/*TODO: Tunde must fix this
		 * if (InputHandler.isSwiping)
			print ("Shooting" + InputHandler.swipe_direction);
			*/
	}
}
