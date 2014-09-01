using UnityEngine;
using System.Collections;

public class BallTestTwo : MonoBehaviour {

	[SerializeField] float tapDistance = 0.6f;
	[SerializeField] float force = 450f;

	public void OnTap(Vector2 _tap) {
		if (_tap.y < 0) {
			if(Vector2.Distance(transform.position,_tap) <= tapDistance) {
				this.rigidbody2D.velocity *= 0;
				this.rigidbody2D.AddForce(new Vector2(transform.position.x-_tap.x,1) * force);
			}
		}
	}
}
