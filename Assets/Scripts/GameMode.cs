using UnityEngine;
using System.Collections;

public class GameMode : MonoBehaviour {
	public int numOfGoalsToWin = 0;
	public int[] goalCount;
	public Team[] teams;

	public TextMesh scoreText = null;


	void Start() {
		_OnStart ();
		GameEvents_2.SubscribeToEvent (GameEvents_2.GameEvent.EVT_GOAL_SCORED, this);
		GameEvents_2.SubscribeToEvent (GameEvents_2.GameEvent.EVT_PLAYER_SHOOT, this);
		GameEvents_2.SubscribeToEvent (GameEvents_2.GameEvent.EVT_PLAYER_POSSESS_BALL, this);
		GameEvents_2.SubscribeToEvent (GameEvents_2.GameEvent.EVT_PLAYER_MOVED, this);
	}
	
	void Update () {
		_OnUpdate ();

		//TODO: Remove this commented code.
		/*if( GameEvents.GetEvent(GameEvents.GameEvent.EVT_GOAL_SCORED)) {
			_OnGoalScored();
		}
		if( GameEvents.GetEvent(GameEvents.GameEvent.EVT_PLAYER_SHOOT) ) {
			_OnPlayerShoot();
		}
		if( GameEvents.GetEvent(GameEvents.GameEvent.EVT_PLAYER_POSSESS_BALL) ) {
			_OnPlayerBallPossession();
		}
		if( GameEvents.GetEvent(GameEvents.GameEvent.EVT_PLAYER_MOVED)) {
			_OnPlayerMoved();
		}*/

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

	protected virtual void _OnStart() {
	}
	
	protected virtual void _OnGoalScored() {
	}
	
	protected virtual void _OnPlayerShoot() {
	}
	
	protected virtual void _OnPlayerBallPossession() {
	}
	
	protected virtual void _OnPlayerMoved() {
	}

/*******************************************************************/
	protected virtual void _OnGoalScored2(Team _scoringTeam) {
	}
	
	protected virtual void _OnPlayerShot2() {
	}
	
	protected virtual void _OnPlayerBallPossession2() {
	}
	
	protected virtual void _OnPlayerMoved2(object[] _TeamAndPlayer) {
	}
}
