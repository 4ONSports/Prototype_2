using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnlineGameEventsHandler : MonoBehaviour {
	
	private static List<MonoBehaviour>[] evtSubscribers = new List<MonoBehaviour>[(int)EOnlineGameEvent.COUNT];
	private static bool isInitialized = false;

	void Awake() {
		Initialize();
	}
	
	private static void Initialize() {
		if( !isInitialized ) {
			for( int i=0; i<(int)EGameEvent.COUNT; ++i ) {
				evtSubscribers[i] = new List<MonoBehaviour>();
				evtSubscribers[i].Clear();
			}
			isInitialized = true;
		}
	}

	public static void SubscribeToEvent(EOnlineGameEvent _e, MonoBehaviour _subscriber) {
		Initialize();
		// assert _gameEvent >= GameEvent.EVT_GOAL_SCORED && _gameEvent < GameEvent.COUNT
		evtSubscribers [(int)_e].Add (_subscriber);
	}
	
	public static void BroadcastEvent(EOnlineGameEvent _e) {
		Initialize();
		OnlineGameEvent ge = new OnlineGameEvent (_e);
		foreach (MonoBehaviour _subscriber in evtSubscribers [(int)_e]) {
			_subscriber.SendMessage ("HandleOnlineEvent", ge);
		}
	}
	
	public static void BroadcastEvent(EOnlineGameEvent _e, object _eventProperty) {
		Initialize();
		OnlineGameEvent ge = new OnlineGameEvent (_e, _eventProperty);
		foreach (MonoBehaviour _subscriber in evtSubscribers [(int)_e]) {
			_subscriber.SendMessage ("HandleOnlineEvent", ge);
		}
	}
	
	public static void BroadcastEvent(OnlineGameEvent _ge) {
		Initialize();
		foreach (MonoBehaviour _subscriber in evtSubscribers [(int)_ge.gameEvent]) {
			_subscriber.SendMessage ("HandleOnlineEvent", _ge);
		}
	}
}