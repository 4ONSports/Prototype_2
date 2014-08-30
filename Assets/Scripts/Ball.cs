using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	void Start() {
		this.rigidbody2D.AddForce (new Vector2(-100,0));
	}

	public void OnPossession (Vector3 _ownerPosition) {
		this.rigidbody2D.velocity *= 0;
		this.rigidbody2D.isKinematic = true;
		transform.position = _ownerPosition;
	}

	public void OnKick (Vector2 _direction) {
		this.rigidbody2D.isKinematic = false;
		this.rigidbody2D.AddForce (_direction * 500);
	}
}
