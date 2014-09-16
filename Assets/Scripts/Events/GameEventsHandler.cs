using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameEventsHandler : MonoBehaviour {

	private static List<MonoBehaviour>[] evtSubscribers = new List<MonoBehaviour>[(int)EGameEvent.COUNT];

	void Awake() {
		for( int i=0; i<(int)EGameEvent.COUNT; ++i ) {
			evtSubscribers[i] = new List<MonoBehaviour>();
			evtSubscribers[i].Clear();
		}
	}

	public static void SubscribeToEvent(EGameEvent _e, MonoBehaviour _subscriber) {
		// assert _gameEvent >= GameEvent.EVT_GOAL_SCORED && _gameEvent < GameEvent.COUNT
		evtSubscribers [(int)_e].Add (_subscriber);
	}
	
	public static void BroadcastEvent(EGameEvent _e) {
		GameEvent ge = new GameEvent (_e);
		foreach (MonoBehaviour _subscriber in evtSubscribers [(int)_e]) {
			_subscriber.SendMessage ("HandleEvent", ge);
		}
	}
	
	public static void BroadcastEvent(EGameEvent _e, object _eventProperty) {
		GameEvent ge = new GameEvent (_e, _eventProperty);
		foreach (MonoBehaviour _subscriber in evtSubscribers [(int)_e]) {
			_subscriber.SendMessage ("HandleEvent", ge);
		}
	}
}