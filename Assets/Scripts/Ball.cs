using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	private PlayerControl_Ball ballOwner = null;

	void Start() {
		this.rigidbody2D.AddForce (new Vector2(-100,0));
	}
	
	public void OnPossession (Vector3 _ownerPosition) {
		this.rigidbody2D.velocity *= 0;
		this.rigidbody2D.isKinematic = true;
		transform.position = _ownerPosition;
	}
	
	public void OnPossession (PlayerControl_Ball _pcBall, Collision2D _coll) {
		this.rigidbody2D.velocity *= 0;
		this.rigidbody2D.isKinematic = true;
		ballOwner = _pcBall;
		this.transform.parent = _pcBall.transform;
		Vector3 contactPos = new Vector3(_coll.contacts [0].point.x, _coll.contacts [0].point.y, 0.0f);
		this.transform.position = contactPos;
	}

	public void OnKick (Vector2 _direction) {
		this.rigidbody2D.isKinematic = false;
		this.rigidbody2D.AddForce (_direction * 500);
		if( ballOwner != null ) {
			this.transform.parent = null;
			ballOwner = null;
		}
	}
}
