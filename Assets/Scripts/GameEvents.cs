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
	static private int[] eventPropertyArray_Int;
	static private string[] eventPropertyArray_String;

	// Use this for initialization
	void Start () {
		eventState = new bool[(int)GameEvent.COUNT];
		eventStateBuffer = new bool[(int)GameEvent.COUNT];
		eventPropertyArray_Int = new int[(int)GameEvent.COUNT];
		eventPropertyArray_String = new string[(int)GameEvent.COUNT];
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
			eventPropertyArray_Int[i] = -1;
			eventPropertyArray_String[i] = "";
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
		}
	}
	
	public static void TriggerEvent( GameEvent _evt ) {
		// assert _evt < GameEvent.COUNT
		eventStateBuffer[(int)_evt] = true;
	}
	
	public static void TriggerEvent<T>( GameEvent _evt, T eventProperty ) {
//		// assert _evt < GameEvent.COUNT
//		eventStateBuffer[(int)_evt] = true;
//		if( typeof(T) == typeof(int) ) {
//			eventPropertyArray_Int[(int)_evt] = (int)BitConverter.GetBytes((int)this._value);;
//		}
//		if( typeof(T) == typeof(string) ) {
//			eventPropertyArray_String[(int)_evt] = (string)eventProperty;
//		}
	}

	public static void TriggerEvent<T, U>( GameEvent _evt, T eventProperty1, U eventProperty2 ) {
//		// assert _evt < GameEvent.COUNT
//		// assert typeof T != type of U
//		eventStateBuffer[(int)_evt] = true;
//
//		if( typeof(T) == typeof(int) ) {
//			eventPropertyArray_Int[(int)_evt] = (int)eventProperty1;
//		}
//		if( typeof(T) == typeof(string) ) {
//			eventPropertyArray_String[(int)_evt] = (string)eventProperty1;
//		}
//		
//		if( typeof(U) == typeof(int) ) {
//			eventPropertyArray_Int[(int)_evt] = (int)eventProperty2;
//		}
//		if( typeof(U) == typeof(string) ) {
//			eventPropertyArray_String[(int)_evt] = (string)eventProperty2;
//		}
	}
	
	public static bool GetEvent( GameEvent _evt ) {
		return eventState[(int)_evt];
	}
	
	public static void GetEventProperty<T>( GameEvent _evt ) {
//		T returnVal;

//		if( eventState[(int)_evt] ) {
//			if( typeof(T) == typeof(int) ) {
//				returnVal = eventPropertyArray_Int[(int)_evt];
//			}
//			if( typeof(T) == typeof(int) ) {
//				returnVal = eventPropertyArray_String[(int)_evt];
//			}
//		}
//
//		return returnVal;
	}
}
