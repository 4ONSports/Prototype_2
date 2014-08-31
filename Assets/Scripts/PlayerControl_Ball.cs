using UnityEngine;
using System.Collections;

public class PlayerControl_Ball : MonoBehaviour {
	
	[HideInInspector] public Ball ball = null;
	[SerializeField] public bool hasABall = false;
	[SerializeField] private float coolDownPassTime = 0.2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( InputHandler.swipe_state == InputHandler.SwipeState.END && !InputHandler.isTap ) {
			if( ball != null ) {
				Shoot (InputHandler.swipe_direction);
				hasABall = false;
				ball = null;
			}
		}


//		// Input Handler Test
//		if (InputHandler.isTap == true) {
//			print ("Tap");
//		}
//		if (InputHandler.swipe_state == InputHandler.SwipeState.END && !InputHandler.isTap) {
//			print ("Shoot");
//		}
//		if (InputHandler.isSwiping) {
//			print ("Swiping");
//		}
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Ball" && !hasABall) {
			hasABall = true;
			ball = coll.gameObject.GetComponent<Ball>();
			ball.OnPossession(this, coll);
		}
	}

	void Shoot(Vector2 _dir) {
		ball.OnKick(_dir);
		StartCoroutine (this.OnCoolDown ());
	}

	private IEnumerator OnCoolDown() {
		this.collider2D.enabled = false;
		yield return new WaitForSeconds(coolDownPassTime);
		this.collider2D.enabled = true;
	}
}
