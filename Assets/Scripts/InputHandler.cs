using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {
	
	public enum SwipeState{
		NONE,
		BEGIN,
		INPROGRESS,
		END,
	}
	
	public static int fingerTouchIndex = 1;
	public static int maxTouchFingers = 2;
	public static int mouse2TouchIndex = 0;
	public static float maxTapTime = 1.0f;
	public static float minSwipeLength = 3.0f;
	public static bool useTouch = false;
	public static SwipeInfo[] swipeInfo;
	
	//	private bool useTouch = false;
	//	private bool isSwipe = false;
	//	private float swipeStartTime  = 0.0f;
	
	
	// Use this for initialization
	void Start () {
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			useTouch = true;
		}
		else {
			maxTouchFingers = 1;
		}

		swipeInfo = new SwipeInfo[maxTouchFingers];
		for( int i=0; i<maxTouchFingers; ++i ) {
			swipeInfo[i] = new SwipeInfo();
			swipeInfo[i].swipe_state = SwipeState.NONE;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for( int i=0; i<maxTouchFingers; ++i ) {
			if( swipeInfo[i].swipe_state == SwipeState.END )  {
				swipeInfo[i].swipe_state = SwipeState.NONE;

				swipeInfo[i].swipe_startTime = 0.0f;
				swipeInfo[i].swipe_startPos = Vector2.zero;
				swipeInfo[i].swipe_direction = Vector2.zero;
				swipeInfo[i].swipe_duration = 0.0f;
				swipeInfo[i].swipe_length = 0.0f;
			}
		}
		
		// Mouse Controls
		if (!useTouch) {
			Vector2 mousePos;
			mousePos.x = Input.mousePosition.x;
			mousePos.y = Input.mousePosition.y;
			int ctrlIndex = 0;
			
			if( Input.GetMouseButtonDown(mouse2TouchIndex) ) {
				swipeInfo[ctrlIndex].swipe_state = SwipeState.BEGIN;
				swipeInfo[ctrlIndex].isSwiping = true;
				swipeInfo[ctrlIndex].swipe_startTime = Time.time;
				swipeInfo[ctrlIndex].swipe_startPos = mousePos;
			}
			else if( Input.GetMouseButtonUp(mouse2TouchIndex) ) {
				swipeInfo[ctrlIndex].swipe_state = SwipeState.END;
				swipeInfo[ctrlIndex].isSwiping = false;
				swipeInfo[ctrlIndex].swipe_endPos = mousePos;
				
				swipeInfo[ctrlIndex].swipe_duration = Time.time - swipeInfo[ctrlIndex].swipe_startTime;
				swipeInfo[ctrlIndex].swipe_length = (mousePos - swipeInfo[ctrlIndex].swipe_startPos).magnitude;
				swipeInfo[ctrlIndex].swipe_direction = mousePos - swipeInfo[ctrlIndex].swipe_startPos;
				swipeInfo[ctrlIndex].swipe_direction.Normalize();

				if( swipeInfo[ctrlIndex].swipe_length < minSwipeLength && swipeInfo[ctrlIndex].swipe_duration < maxTapTime ) {
					swipeInfo[ctrlIndex].isTap = true;
					StartCoroutine(this.OnTapFrameReset(ctrlIndex));
				}
				else {
					swipeInfo[ctrlIndex].isTap = false;
				}
			}
			else if( Input.GetMouseButton(mouse2TouchIndex) ) {
				swipeInfo[ctrlIndex].swipe_state = SwipeState.INPROGRESS;
				swipeInfo[ctrlIndex].swipe_direction = mousePos - swipeInfo[ctrlIndex].swipe_startPos;
				swipeInfo[ctrlIndex].swipe_direction.Normalize();
			}
		}
		
		// Touch Controls
		if (useTouch && Input.touchCount > 0){
			//foreach (Touch touch in Input.touches)
			for( int i=0; i<maxTouchFingers; ++i ) {
				if( i < Input.touchCount ) {
					Touch touch = Input.GetTouch(i);
					{
						switch (touch.phase)
						{
						case TouchPhase.Began :
							swipeInfo[i].swipe_state = SwipeState.BEGIN;
							print ("Touch Index = "+i);
							swipeInfo[i].isSwiping = true;
							swipeInfo[i].swipe_startTime = Time.time;
							swipeInfo[i].swipe_startPos = touch.position;
							break;
							
						case TouchPhase.Canceled :
							/* The touch is being canceled */
							swipeInfo[i].isSwiping = false;
							swipeInfo[i].swipe_state = SwipeState.NONE;
							break;
							
						case TouchPhase.Ended :
							swipeInfo[i].swipe_state = SwipeState.END;
							swipeInfo[i].isSwiping = false;
							
							swipeInfo[i].swipe_duration = Time.time - swipeInfo[i].swipe_startTime;
							swipeInfo[i].swipe_endPos = touch.position;
							swipeInfo[i].swipe_length = (touch.position - swipeInfo[i].swipe_startPos).magnitude;
							swipeInfo[i].swipe_direction = touch.position - swipeInfo[i].swipe_startPos;
							swipeInfo[i].swipe_direction.Normalize();
							
							if( swipeInfo[i].swipe_length < minSwipeLength && swipeInfo[i].swipe_duration < maxTapTime ) {
								swipeInfo[i].isTap = true;
								StartCoroutine(this.OnTapFrameReset(i));
							}
							else {
								swipeInfo[i].isTap = false;
							}
							break;
							
						case TouchPhase.Moved :
							swipeInfo[i].swipe_state = SwipeState.INPROGRESS;
							swipeInfo[i].swipe_direction = touch.position - swipeInfo[i].swipe_startPos;
							swipeInfo[i].swipe_direction.Normalize();
							break;
						}
					}
				}
			}
		}
	}

	public static void RefreshStart ( int ctrlIndex ) {
		// Mouse Controls
		if (!useTouch) {
			Vector2 mousePos;
			mousePos.x = Input.mousePosition.x;
			mousePos.y = Input.mousePosition.y;

			swipeInfo[ctrlIndex].swipe_state = SwipeState.BEGIN;
			swipeInfo[ctrlIndex].isSwiping = true;
			swipeInfo[ctrlIndex].swipe_startPos = mousePos;
		}
		
		// Touch Controls
		if (useTouch){
			Touch touch = Input.GetTouch(ctrlIndex);
			
			swipeInfo[ctrlIndex].swipe_state = SwipeState.BEGIN;
			swipeInfo[ctrlIndex].isSwiping = true;
			swipeInfo[ctrlIndex].swipe_startPos = touch.position;
		}
	}
	
	public static void ReorderSwipeInfo () {
		if( Input.touchCount>0 ) {
			SwipeInfo tempSwipeInfo = swipeInfo [0];
			swipeInfo[0] = swipeInfo [1];
			swipeInfo[1] = tempSwipeInfo;
		}
	}
	
	private IEnumerator OnTapFrameReset( int _ctrlIndex ) {
		yield return null;
		swipeInfo[_ctrlIndex].isTap = false;
	}
}
