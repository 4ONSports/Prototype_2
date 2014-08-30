using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	[SerializeField] private Players players = null;

	void OnMouseUp() {
		players.OnTap ();
	}

	void OnMouseDrag() {

	}
}
