using UnityEngine;
using System.Collections;

public class PlayerControl_Movement : MonoBehaviour {

	public bool playerSelect = false;
	public bool allowMovement_X = true;
	public bool allowMovement_Y = false;
	public bool noMovementWhenHasBall = true;
	public int playerCtrlIndex = -1;
	public float playerMovementSpeed = 5.0f;
	[SerializeField] public bool disable = false;

	private PlayerControl_Ball pcBall = null;
	private bool playerCtrlIndexReordered = false;

	// Use this for initialization
	void Start () {
		pcBall = this.GetComponent<PlayerControl_Ball>();

		if( !InputHandler.useTouch ) {
			playerCtrlIndex = 0;
		}
	}

	Vector3 GetTouchPosition( int index ) {
		Vector3 returnPos = Vector3.zero;
		if( !InputHandler.useTouch ) { 
			returnPos = Input.mousePosition;
		}
		else {
			Touch touch = Input.GetTouch(index);
			returnPos.x = touch.position.x;
			returnPos.y = touch.position.y;
		}
		
		return returnPos;
	}

	void Update () {
		for( int i=0; i<InputHandler.maxTouchFingers; ++i ) {
			// bug fix for when touch 0 is removed
			if( InputHandler.swipeInfo[i].swipe_state == InputHandler.SwipeState.END ) {
				if( playerCtrlIndex == i ) {
					if( pcBall.hasABall /*&& playerSelect*/ ) {
						Debug.Log ("Shooting");
						pcBall.CheckForShoot(playerCtrlIndex);
						playerCtrlIndex = -1;
						playerSelect = false;
					}
					else if( !pcBall.hasABall /*&& playerSelect*/ ) {
						Debug.Log ("Moving");
						this.rigidbody2D.velocity *= 0;
						this.rigidbody2D.angularVelocity *= 0;
						this.rigidbody2D.AddForce (InputHandler.swipeInfo[playerCtrlIndex].swipe_direction * playerMovementSpeed);
					}
				}
				else if( i==0 && playerCtrlIndex>0 ) {
					--playerCtrlIndex;
					playerCtrlIndexReordered = true;
					//InputHandler.ReorderSwipeInfo();
				}
			}
			else if( InputHandler.swipeInfo[i].swipe_state == InputHandler.SwipeState.BEGIN && !playerSelect ) {
				Ray ray = Camera.main.ScreenPointToRay (GetTouchPosition(i));
				RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
				
				if ( hit.transform != null && hit.collider != null )
				{
					if( hit.transform.gameObject.name == gameObject.name ) {
						playerSelect = true;
						playerCtrlIndex = i;
						playerCtrlIndexReordered = false;
					}
				}
			}
			else if( i == 0 && InputHandler.swipeInfo[i].swipe_state == InputHandler.SwipeState.BEGIN && playerSelect && playerCtrlIndexReordered ) {
				playerCtrlIndex++;
				playerCtrlIndexReordered = false;
			}
		}
		
		if( disable || (pcBall.hasABall && noMovementWhenHasBall) ) {
			return;
		}
		
		if( playerCtrlIndex<0 ) {
			return;
		}

//		if ( InputHandler.swipeInfo[playerCtrlIndex].swipe_state == InputHandler.SwipeState.END ) {
//			playerSelect = false;
//		}

		if( playerSelect ) {
//			//Move
//			if( InputHandler.swipeInfo[playerCtrlIndex].swipe_state == InputHandler.SwipeState.INPROGRESS ) {
//				Ray ray = Camera.main.ScreenPointToRay (GetTouchPosition(playerCtrlIndex));
//				RaycastHit hit;
//
//				if( Physics.Raycast (ray, out hit, 200) ) {
//					Vector3 tempPos = transform.position;
//					tempPos.x = (allowMovement_X)? hit.point.x: tempPos.x;
//					tempPos.y = (allowMovement_Y)? hit.point.y: tempPos.y;
//					transform.position = tempPos;
//				}
//			}
		}
	}
}
