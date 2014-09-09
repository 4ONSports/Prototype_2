using UnityEngine;
using System.Collections;

public class PlayerControl_Movement : MonoBehaviour {

	public bool playerSelect = false;
	public bool allowMovement_X = true;
	public bool allowMovement_Y = false;
	public bool noMovementWhenHasBall = true;
	public int playerCtrlIndex = -1;
	public float playerMovementSpeed = 5.0f;
	public float tempFriction = 10.0f;
	public bool forceAdded = false;
	public TeamSide playerTeamSide = TeamSide.INVALID;
	public int playerIndexPosOnTeam = -1;
	[SerializeField] public bool disable = false;

	private PlayerControl_Ball pcBall = null;
	private bool playerCtrlIndexReordered = false;
	[SerializeField] private GameObject movementLimitObj;
	[SerializeField] private Color movementLimitObj_DefaultColor = new Color(1,1,1,0.01f);
	[SerializeField] private Color movementLimitObj_NoMvmntColor = new Color(1,0,0,0.01f);
	[SerializeField] private float scaleDiff = 0.0f;
	[SerializeField] private Vector3 initMvmntTransform = Vector3.zero;
	[SerializeField] private bool movePlayer = false;
	[SerializeField] private Vector3 newPos = Vector3.zero;

	// Use this for initialization
	void Start () {
		pcBall = this.GetComponent<PlayerControl_Ball>();
		this.rigidbody2D.isKinematic = true;

		movementLimitObj = null;
		if( transform.Find ("MoveLimit")!=null ) {
			movementLimitObj = transform.Find ("MoveLimit").gameObject;
			movementLimitObj.renderer.enabled = false;
			scaleDiff = (movementLimitObj.transform.lossyScale.x - transform.lossyScale.x) / 2.0f;
		}
		
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
				if( movementLimitObj != null ) {
					movementLimitObj.transform.position = transform.position;
					movementLimitObj.transform.parent = transform;
					movementLimitObj.renderer.enabled = false;
				}
				if( movePlayer ) {
					int[] obj = {(int)playerTeamSide, playerIndexPosOnTeam};
					GameEvents.TriggerEvent(GameEvents.GameEvent.EVT_PLAYER_MOVED, obj);
					GameEvents_2.BroadcastPlayerMoved(new object[]{playerTeamSide,playerIndexPosOnTeam});
				}
				movePlayer = false;

				if( playerCtrlIndex == i ) {
					if( pcBall.hasABall && playerSelect ) {
						pcBall.CheckForShoot(playerCtrlIndex);
						playerCtrlIndex = -1;
						playerSelect = false;
					}
					else if( !pcBall.hasABall && playerSelect ) {
						playerSelect = false;
//						// Move 2
//						Debug.Log ("Moving " + playerCtrlIndex);
//						this.rigidbody2D.angularVelocity *= 0;
//						this.rigidbody2D.isKinematic = false;
//						forceAdded = true;
//						this.rigidbody2D.AddForce (InputHandler.swipeInfo[playerCtrlIndex].swipe_direction * playerMovementSpeed);
					}
				}
				else if( i==0 && playerCtrlIndex>0 ) {
					--playerCtrlIndex;
					playerCtrlIndexReordered = true;
				}
			}
			else if( InputHandler.swipeInfo[i].swipe_state == InputHandler.SwipeState.BEGIN && !playerSelect ) {
				Ray ray = Camera.main.ScreenPointToRay (GetTouchPosition(i));
				RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
				
				if ( hit.transform != null && hit.collider != null )
				{
					if( hit.transform.gameObject.name == gameObject.name ||
					   (pcBall.hasABall && hit.transform.tag == "Ball") ) {
						playerSelect = true;
						playerCtrlIndex = i;
						playerCtrlIndexReordered = false;
						initMvmntTransform.x = transform.position.x;
						initMvmntTransform.y = transform.position.y;
						initMvmntTransform.z = transform.position.z;
						movementLimitObj.renderer.enabled = true;
						if( disable ) {
							movementLimitObj.renderer.material.color = movementLimitObj_NoMvmntColor;
						}
						else {
							movementLimitObj.renderer.material.color = movementLimitObj_DefaultColor;
						}
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

		if( playerSelect ) {
			//Move 1
			if( InputHandler.swipeInfo[playerCtrlIndex].swipe_state == InputHandler.SwipeState.INPROGRESS ) {
				Ray ray = Camera.main.ScreenPointToRay (GetTouchPosition(playerCtrlIndex));
				RaycastHit hit;

				if( Physics.Raycast (ray, out hit, 200) ) {
					Vector3 tempPos = transform.position;
					tempPos.x = (allowMovement_X)? hit.point.x: tempPos.x;
					tempPos.y = (allowMovement_Y)? hit.point.y: tempPos.y;


					if( movementLimitObj != null ) {
						movementLimitObj.transform.parent = null;
						
						Vector2 moveVector = Vector2.zero;
						moveVector.x = (tempPos-initMvmntTransform).x;
						moveVector.y = (tempPos-initMvmntTransform).y;
						if( moveVector.magnitude < scaleDiff ) {
							//transform.position = tempPos;
							movePlayer = true;
							newPos = tempPos;
						}
						else {
							movePlayer = true;
							tempPos.x = (tempPos.x - initMvmntTransform.x > 0)? initMvmntTransform.x+scaleDiff: initMvmntTransform.x-scaleDiff;
							newPos = tempPos;
							newPos.x = initMvmntTransform.x + (moveVector).normalized.x * scaleDiff;
							newPos.y = initMvmntTransform.y + (moveVector).normalized.y * scaleDiff;
						}
					}
				}
			}
		}
	}

	void FixedUpdate () {
		if( movePlayer ) {
			rigidbody2D.MovePosition(newPos);
		}
	}
}
