using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameEvents : MonoBehaviour {

	public enum GameEvent{
		EVT_GOAL_SCORED,
		EVT_PLAYER_SHOOT,
		EVT_PLAYER_POSSESS_BALL,
		EVT_PLAYER_MOVED,
		COUNT
	}

	private static List<MonoBehaviour> evtGoalScoredSubscribers = new List<MonoBehaviour>();
	private static List<MonoBehaviour> evtPlayerShotSubscribers = new List<MonoBehaviour>();
	private static List<MonoBehaviour> evtPlayerPossessBallSubscribers = new List<MonoBehaviour>();
	private static List<MonoBehaviour> evtPlayerMovedSubscribers = new List<MonoBehaviour>();

	void Awake() {
		evtGoalScoredSubscribers.Clear ();
		evtPlayerShotSubscribers.Clear ();
		evtPlayerPossessBallSubscribers.Clear ();
		evtPlayerMovedSubscribers.Clear ();
	}

	public static void SubscribeToEvent(GameEvent _gameEvent, MonoBehaviour _subscriber) {
		switch(_gameEvent)
		{
		case GameEvent.EVT_GOAL_SCORED:
			evtGoalScoredSubscribers.Add(_subscriber);
			break;
		case GameEvent.EVT_PLAYER_SHOOT:
			evtPlayerShotSubscribers.Add(_subscriber);
			break;
		case GameEvent.EVT_PLAYER_POSSESS_BALL:
			evtPlayerPossessBallSubscribers.Add(_subscriber);
			break;
		case GameEvent.EVT_PLAYER_MOVED:
			evtPlayerMovedSubscribers.Add(_subscriber);
			break;
		}
	}

	public static void BroadcastGoalScored(Team _scoringTeam) {
		foreach (MonoBehaviour _subscriber in evtGoalScoredSubscribers) {
			_subscriber.SendMessage ("_OnGoalScored",_scoringTeam);
		}
	}

	public static void BroadcastPlayerShot() {
		foreach (MonoBehaviour _subscriber in evtPlayerShotSubscribers) {
			_subscriber.SendMessage ("_OnPlayerShot");
		}
	}

	public static void BroadcastPlayerPossessBall() {
		foreach (MonoBehaviour _subscriber in evtPlayerPossessBallSubscribers) {
			_subscriber.SendMessage ("_OnPlayerBallPossession");
		}
	}

	public static void BroadcastPlayerMoved(object[] _TeamAndPlayer) {
		foreach (MonoBehaviour _subscriber in evtPlayerMovedSubscribers) {
			_subscriber.SendMessage ("_OnPlayerMoved",_TeamAndPlayer);
		}
	}
}