using UnityEngine;
using System.Collections;

public class PlayerControl_Ball : MonoBehaviour {
	
	[HideInInspector] public Ball ball = null;
	[SerializeField] public bool hasABall = false;
	[SerializeField] public float minSwipeDistanceToShoot_Touch = 1.0f;
	[SerializeField] public float minSwipeDistanceToShoot_Mouse = 1.0f;

	[SerializeField] private float minSwipeDistanceToShoot = 1.0f;
	[SerializeField] private float coolDownPassTime = 0.2f;
	[SerializeField] private PlayerControl_Movement pc_mvmnt;

	// Use this for initialization
	void Start () {
		minSwipeDistanceToShoot = (InputHandler.useTouch) ? minSwipeDistanceToShoot_Touch : minSwipeDistanceToShoot_Mouse;
		pc_mvmnt = this.GetComponent<PlayerControl_Movement>();
	}
	
	// Update is called once per frame
	void Update () {
//		if( pc_mvmnt.playerCtrlIndex<0 ) {
//			return;
//		}

//		int index = pc_mvmnt.playerCtrlIndex;
//		if( InputHandler.swipeInfo[index].swipe_state == InputHandler.SwipeState.END ) {
//			if ( pc_mvmnt.playerSelect && !InputHandler.swipeInfo[index].isTap ) {
//				if( ball != null && InputHandler.swipeInfo[index].swipe_length >= minSwipeDistanceToShoot ) {
//					Vector3 tempStartPos = Camera.main.ScreenToWorldPoint(InputHandler.swipeInfo[index].swipe_startPos);
//					Vector3 tempEndPos = Camera.main.ScreenToWorldPoint(InputHandler.swipeInfo[index].swipe_endPos);
//
//					Debug.DrawLine(tempStartPos, tempEndPos, Color.yellow, 2.0f);
//					Shoot (InputHandler.swipeInfo[index].swipe_direction);
//					hasABall = false;
//					pc_mvmnt.Reset();
//					ball = null;
//				}
//			}
//		}


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

			if( pc_mvmnt.playerCtrlIndex >= 0 ) {
				InputHandler.RefreshStart(pc_mvmnt.playerCtrlIndex);
			}
		}
	}
	
	void Shoot(Vector2 _dir) {
		ball.OnKick(_dir);
		StartCoroutine (this.OnCoolDown ());
	}
	
	public void CheckForShoot(int _index) {
		if( InputHandler.swipeInfo[_index].swipe_state == InputHandler.SwipeState.END ) {
			if ( pc_mvmnt.playerSelect && !InputHandler.swipeInfo[_index].isTap ) {
				if( ball != null && InputHandler.swipeInfo[_index].swipe_length >= minSwipeDistanceToShoot ) {
					Vector3 tempStartPos = Camera.main.ScreenToWorldPoint(InputHandler.swipeInfo[_index].swipe_startPos);
					Vector3 tempEndPos = Camera.main.ScreenToWorldPoint(InputHandler.swipeInfo[_index].swipe_endPos);
					
					Debug.DrawLine(tempStartPos, tempEndPos, Color.yellow, 2.0f);
					Shoot (InputHandler.swipeInfo[_index].swipe_direction);
					hasABall = false;
					ball = null;
				}
			}
		}
	}

	private IEnumerator OnCoolDown() {
		this.collider2D.enabled = false;
		yield return new WaitForSeconds(coolDownPassTime);
		this.collider2D.enabled = true;
	}
}
