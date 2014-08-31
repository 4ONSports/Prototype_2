using UnityEngine;
using System.Collections;

public class PlayerControl_Movement : MonoBehaviour {

	public bool playerSelect = false;
	public bool allowMovement_X = true;
	public bool allowMovement_Y = false;
	public bool noMovementWhenHasBall = true;

	private PlayerControl_Ball pcBall = null;

	// Use this for initialization
	void Start () {
		pcBall = this.GetComponent<PlayerControl_Ball>();
	}

	void Update () {
		if( pcBall.hasABall && noMovementWhenHasBall ) {
			return;
		}

		if( InputHandler.swipe_state == InputHandler.SwipeState.BEGIN ) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
			
			if ( hit != null && hit.collider != null )
			{
				if( hit.transform.gameObject.name == gameObject.name ) {
					playerSelect = true;
				}
			}
		}

		if ( InputHandler.swipe_state == InputHandler.SwipeState.END ) {
			playerSelect = false;
		}

		if( playerSelect ) {
			if( InputHandler.swipe_state == InputHandler.SwipeState.INPROGRESS ) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;

				if( Physics.Raycast (ray, out hit, 200) ) {
					Vector3 tempPos = transform.position;
					tempPos.x = (allowMovement_X)? hit.point.x: tempPos.x;
					tempPos.y = (allowMovement_Y)? hit.point.y: tempPos.y;
					transform.position = tempPos;
				}
			}
		}
	}
	
	void OnGUI() {
		GUI.Box(new Rect(10,10,200,210), "Debug Menu");
		
		if(GUI.Button(new Rect(20,40,160,40), "Toggle Y Move")) {
			allowMovement_Y = !allowMovement_Y;
		}
		
		if(GUI.Button(new Rect(20,80,160,40), "Toggle Move when with ball")) {
			noMovementWhenHasBall = !noMovementWhenHasBall;
		}
	}
}
