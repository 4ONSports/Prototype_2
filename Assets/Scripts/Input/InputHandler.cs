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
	public static int maxTouchFingers = 4;
	public static int mouse2TouchIndex = 0;
	public static float maxTapTime = 1.0f;
	public static float minSwipeLength = 3.0f;
	public static bool useTouch = false;
	public static SwipeInfo[] swipeInfo;
	public LineRenderer linerenderer;
	
	private static LineRenderer line;
	
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
		
		line = Instantiate(linerenderer, transform.position, Quaternion.identity) as LineRenderer;
		line.name = "InputHandler_Line";
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
				swipeInfo[ctrlIndex].swipe_length = (swipeInfo[ctrlIndex].swipe_endPos - swipeInfo[ctrlIndex].swipe_startPos).magnitude;
				swipeInfo[ctrlIndex].swipe_direction = swipeInfo[ctrlIndex].swipe_endPos - swipeInfo[ctrlIndex].swipe_startPos;
				swipeInfo[ctrlIndex].swipe_direction.Normalize();
				RenderLine ();

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
							swipeInfo[i].swipe_length = (swipeInfo[i].swipe_endPos - swipeInfo[i].swipe_startPos).magnitude;
							swipeInfo[i].swipe_direction = swipeInfo[i].swipe_endPos - swipeInfo[i].swipe_startPos;
							swipeInfo[i].swipe_direction.Normalize();
							RenderLine ();
							
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
	
	static void RenderLine () {
		for( int i=0; i<maxTouchFingers; ++i ) {
			if( line != null && GameDebug.enableDebugLines ) {
				Ray ray1 = Camera.main.ScreenPointToRay (swipeInfo[i].swipe_startPos);
				Ray ray2 = Camera.main.ScreenPointToRay (swipeInfo[i].swipe_endPos);
				RaycastHit hit1, hit2;
				
				Vector3 point1 = Vector3.zero, point2 = Vector3.zero;
				if( Physics.Raycast (ray1, out hit1, 200) && Physics.Raycast (ray2, out hit2, 200) ) {
					point1 = hit1.point;
					point2 = hit2.point;

					line.SetColors(Color.gray, Color.gray);
					line.SetWidth(0.05f, 0.05f);
					line.SetVertexCount(2);
					line.SetPosition(0, point1);
					line.SetPosition(1, point2);
				}
			}
		}
	}

	public static void RefreshStart ( int ctrlIndex ) {
		// Mouse Controls
		if (!useTouch) {
			if( swipeInfo[ctrlIndex].swipe_state == SwipeState.INPROGRESS ) {
				Vector2 mousePos;
				mousePos.x = Input.mousePosition.x;
				mousePos.y = Input.mousePosition.y;

				swipeInfo[ctrlIndex].swipe_state = SwipeState.BEGIN;
				swipeInfo[ctrlIndex].isSwiping = true;
				swipeInfo[ctrlIndex].swipe_startPos = mousePos;
			}
		}
		
		// Touch Controls
		if (useTouch){
			if( swipeInfo[ctrlIndex].swipe_state == SwipeState.INPROGRESS ) {
				Touch touch = Input.GetTouch(ctrlIndex);
				
				swipeInfo[ctrlIndex].swipe_state = SwipeState.BEGIN;
				swipeInfo[ctrlIndex].isSwiping = true;
				swipeInfo[ctrlIndex].swipe_startPos = touch.position;
			}
		}
	}
	
	private IEnumerator OnTapFrameReset( int _ctrlIndex ) {
		yield return null;
		swipeInfo[_ctrlIndex].isTap = false;
	}
}
