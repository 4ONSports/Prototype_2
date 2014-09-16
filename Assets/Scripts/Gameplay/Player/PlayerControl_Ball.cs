using UnityEngine;
using System.Collections;

public class PlayerControl_Ball : MonoBehaviour {
	
	[HideInInspector] public Ball ball = null;
	[SerializeField] public bool hasABall = false;
	[SerializeField] public bool disable = false;
	[SerializeField] public bool cannotReceiveBall = false;
	[SerializeField] public float minSwipeDistanceToShoot_Touch = 1.0f;
	[SerializeField] public float minSwipeDistanceToShoot_Mouse = 1.0f;

	[SerializeField] private float minSwipeDistanceToShoot = 1.0f;
	[SerializeField] private float coolDownShootTime = 0.2f;
	[SerializeField] private PlayerControl_Movement pc_mvmnt;

	// Use this for initialization
	void Start () {
		minSwipeDistanceToShoot = (InputHandler.useTouch) ? minSwipeDistanceToShoot_Touch : minSwipeDistanceToShoot_Mouse;
		pc_mvmnt = this.GetComponent<PlayerControl_Movement>();
	}
	
	// Update is called once per frame
	void Update () {
//		if( pc_mvmnt.playerCtrlIndex >= 0 ) {
//			CheckForShoot(pc_mvmnt.playerCtrlIndex);
//		}
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		if( hasABall || cannotReceiveBall ) {
			return;
		}

		if (coll.gameObject.tag == "Ball" ) {
			hasABall = true;
			ball = coll.gameObject.GetComponent<Ball>();
//			ball.OnPossession(this, coll);
			ball.OnPossession(this, transform.position);
			GameEventsHandler.BroadcastEvent(EGameEvent.EVT_PLAYER_POSSESS_BALL);

			if( pc_mvmnt.playerCtrlIndex >= 0 ) {
				InputHandler.RefreshStart(pc_mvmnt.playerCtrlIndex);
			}
		}
	}
	
	void Shoot(Vector2 _dir) {
		if(!disable) {
			ball.OnKick(_dir);
			GameEventsHandler.BroadcastEvent(EGameEvent.EVT_PLAYER_SHOOT);
			StartCoroutine (this.OnCoolDown ());
		}
	}
	
	public void CheckForShoot(int _index) {
		if( InputHandler.swipeInfo[_index].swipe_state == InputHandler.SwipeState.END && !disable ) {
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
		yield return new WaitForSeconds(coolDownShootTime);
		this.collider2D.enabled = true;
	}
}
