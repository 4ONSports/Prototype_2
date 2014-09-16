using UnityEngine;
using System.Collections;

public class SwipeInfo {
	public bool isSwiping = false;
	public bool isTap = false;
	public InputHandler.SwipeState swipe_state  = InputHandler.SwipeState.NONE;
	public float swipe_startTime  = 0.0f;
	public Vector2 swipe_startPos  = Vector2.zero;
	public Vector2 swipe_endPos  = Vector2.zero;
	public Vector2 swipe_direction  = Vector2.zero;
	public float swipe_duration  = 0.0f;
	public float swipe_length  = 0.0f;
}
