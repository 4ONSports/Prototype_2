using UnityEngine;
using System.Collections;

public class GameEvents : MonoBehaviour {
	public enum GameEvent{
		EVT_GOAL_SCORED,
		EVT_PLAYER_SHOOT,
		EVT_PLAYER_POSSESS_BALL,
		EVT_PLAYER_MOVED,
		COUNT
	}
	static private bool[] eventState;
	static private bool[] eventStateBuffer;
	static private object[] eventPropertyArray_Obj;

	// Use this for initialization
	void Start () {
		eventState = new bool[(int)GameEvent.COUNT];
		eventStateBuffer = new bool[(int)GameEvent.COUNT];
		eventPropertyArray_Obj = new object[(int)GameEvent.COUNT];
		DisableEvents ();
		DisableEventsBuffer ();
	}

	void LateUpdate () {
		// disable all events
		DisableEvents ();

		// if there is an event to be triggered enable it
		CopyFromBuffer ();
		DisableEventsBuffer ();
	}
	
	static void DisableEvents () {
		for( int i=0; i<eventState.Length; ++i ) {
			eventState[i] = false;
		}
	}
	
	static void DisableEventsBuffer () {
		for( int i=0; i<eventStateBuffer.Length; ++i ) {
			eventStateBuffer[i] = false;
		}
	}
	
	static void CopyFromBuffer () {
		for( int i=0; i<eventState.Length; ++i ) {
			eventState[i] = eventStateBuffer[i];
			if( !eventState[i] ) {
				eventPropertyArray_Obj[i] = null;
			}
		}
	}
	
	public static void TriggerEvent( GameEvent _evt ) {
		// assert _evt < GameEvent.COUNT
		eventStateBuffer[(int)_evt] = true;
	}
	
	public static void TriggerEvent( GameEvent _evt, int eventProperty ) {
		// assert _evt < GameEvent.COUNT
		eventStateBuffer[(int)_evt] = true;
		eventPropertyArray_Obj[(int)_evt] = eventProperty;
	}
	
	public static void TriggerEvent( GameEvent _evt, string eventProperty ) {
		// assert _evt < GameEvent.COUNT
		eventStateBuffer[(int)_evt] = true;
		eventPropertyArray_Obj[(int)_evt] = eventProperty;
	}
	
	public static void TriggerEvent( GameEvent _evt, int[] eventProperty ) {
		// assert _evt < GameEvent.COUNT
		eventStateBuffer[(int)_evt] = true;
		eventPropertyArray_Obj[(int)_evt] = eventProperty;
	}
	
	public static bool GetEvent( GameEvent _evt ) {
		return eventState[(int)_evt];
	}
	
	public static object GetEventProperty( GameEvent _evt ) {
		object returnObject = null;

		if( eventState[(int)_evt] ) {
			returnObject = eventPropertyArray_Obj[(int)_evt];
		}

		return returnObject;
	}
}
