using UnityEngine;
using System.Collections;

public enum EGameEvent{
	EVT_GOAL_SCORED,
	EVT_PLAYER_SHOOT,
	EVT_PLAYER_POSSESS_BALL,
	EVT_PLAYER_MOVED,
	EVT_PLAYER_SELECTED,
	EVT_PLAYER_DESELECTED,
	COUNT
}

public class GameEvent {
	public EGameEvent gameEvent;
	public object gameEventProperty;
	
	public GameEvent(EGameEvent _event) {
		gameEvent = _event;
		gameEventProperty = null;
	}
	
	public GameEvent(EGameEvent _event, object _evtProperty) {
		gameEvent = _event;
		gameEventProperty = _evtProperty;
	}
}
