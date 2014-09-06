using UnityEngine;
using System.Collections;

public class GameMode : MonoBehaviour {
	public int numOfGoalsToWin = 0;
	public int[] goalCount;
	public Team[] teams;
	
	void Start () {
	}
	
	void Update () {
		_OnUpdate ();

		if( GameEvents.GetEvent(GameEvents.GameEvent.EVT_PLAYER_SHOOT) ) {
			_OnPlayerShoot();
		}
		if( GameEvents.GetEvent(GameEvents.GameEvent.EVT_PLAYER_POSSESS_BALL) ) {
			_OnPlayerBallPossession();
		}
	}
	
	protected virtual void _OnUpdate() {
	}
	
	protected virtual void _OnPlayerShoot() {
	}
	
	protected virtual void _OnPlayerBallPossession() {
	}
	
	protected virtual void _OnGoalScored() {
	}
}
