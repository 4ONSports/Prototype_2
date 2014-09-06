using UnityEngine;
using System.Collections;

public class GameMode : MonoBehaviour {
	public int numOfGoalsToWin = 0;
	public int[] goalCount;
	public Team[] teams;

	public TextMesh scoreText = null;
	
	void Update () {
		_OnUpdate ();

		if( GameEvents.GetEvent(GameEvents.GameEvent.EVT_PLAYER_SHOOT) ) {
			_OnPlayerShoot();
		}
		if( GameEvents.GetEvent(GameEvents.GameEvent.EVT_PLAYER_POSSESS_BALL) ) {
			_OnPlayerBallPossession();
		}
		if( GameEvents.GetEvent(GameEvents.GameEvent.EVT_GOAL_SCORED)) {
			_OnGoalScored();
		}

	}

	public Team GetOppositeTeam (Team _team) {
		if(_team.side == TeamSide.SIDE_HOME)
		return teams[1];
		else
		return teams[0];
	}

	public void UpdateScoreText(int scoreHome, int scoreAway) {
		if(!scoreText)return;
		string h = "";
		string t = "";
		if(scoreHome<10)h+= "0";
		t+= "  ";
		if(scoreAway<10)t+= "0";
		this.scoreText.text = h + scoreHome + t + scoreAway;
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
