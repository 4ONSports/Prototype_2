using UnityEngine;
using System.Collections;

public enum EOnlineGameEvent{
	ONL_EVT_START_MATCH,
	ONL_EVT_SWITCH_TURN,
	ONL_EVT_PLAYER_MOVED,
	COUNT
}

public class OnlineGameEvent {
	public EOnlineGameEvent gameEvent;
	public object gameEventProperty;
	
	public OnlineGameEvent(EOnlineGameEvent _event) {
		gameEvent = _event;
		gameEventProperty = null;
	}
	
	public OnlineGameEvent(EOnlineGameEvent _event, object _evtProperty) {
		gameEvent = _event;
		gameEventProperty = _evtProperty;
	}
}
