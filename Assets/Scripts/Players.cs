using UnityEngine;
using System.Collections;

public class Players : MonoBehaviour {

	[SerializeField] private Player leftPlayer = null;
	[SerializeField] private Player rightPlayer = null;

	public void OnTap() {
		Pass ();
	}

	public void OnSwipe(Vector2 _dir) {
		Shoot (_dir);
	}

	private void Pass ()	{
		rightPlayer.Pass (leftPlayer.transform.position);
		leftPlayer.Pass (rightPlayer.transform.position);
	}

	private void Shoot (Vector2 _dir) {
		rightPlayer.Shoot (_dir);
		leftPlayer.Shoot (_dir);
	}

}
