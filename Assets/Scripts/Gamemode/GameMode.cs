using UnityEngine;
using System.Collections;

public class GameMode : MonoBehaviour {
	public int numOfGoalsToWin = 0;
	public Team[] teams;

	public TextMesh scoreText = null;
	protected bool gameOver = false;


	void Start() {
		_OnStart ();
		GameEventsHandler.SubscribeToEvent (EGameEvent.EVT_GOAL_SCORED, this);
		GameEventsHandler.SubscribeToEvent (EGameEvent.EVT_PLAYER_SHOOT, this);
		GameEventsHandler.SubscribeToEvent (EGameEvent.EVT_PLAYER_POSSESS_BALL, this);
		GameEventsHandler.SubscribeToEvent (EGameEvent.EVT_PLAYER_MOVED, this);
		GameEventsHandler.SubscribeToEvent (EGameEvent.EVT_PLAYER_SELECTED, this);
		GameEventsHandler.SubscribeToEvent (EGameEvent.EVT_PLAYER_DESELECTED, this);
	}
	
	void Update () {
		_OnUpdate ();
	}

	public void HandleEvent(GameEvent _event) {
		switch(_event.gameEvent)
		{
		case EGameEvent.EVT_GOAL_SCORED:
			_OnGoalScored((Team)_event.gameEventProperty);
			break;
		case EGameEvent.EVT_PLAYER_SHOOT:
			_OnPlayerShot();
			break;
		case EGameEvent.EVT_PLAYER_POSSESS_BALL:
			_OnPlayerBallPossession();
			break;
		case EGameEvent.EVT_PLAYER_MOVED:
			_OnPlayerMoved((object[])_event.gameEventProperty);
			break;
		case EGameEvent.EVT_PLAYER_SELECTED:
			_OnPlayerSelected((object[])_event.gameEventProperty);
			break;
		case EGameEvent.EVT_PLAYER_DESELECTED:
			_OnPlayerDeselected((object[])_event.gameEventProperty);
			break;
		}
	}

	public Team GetOppositeTeam (Team _team) {
		if(_team.side == TeamSide.SIDE_HOME)
		return teams[1];
		else
		return teams[0];
	}

	public Team GetTeamByTeamSide (TeamSide _teamSide) {
		foreach(Team t in teams) {
			if(t.side == _teamSide) return t;
		}
		Debug.LogError ("Not able to find TeamSide for any of the teams in memory.");
		return teams [0];
	}

	public Color GetTeamColorByTeamSide (TeamSide _teamSide) {
		if(_teamSide == 0) return Color.blue;
		else return Color.red;
	}

	public void UpdateScoreText(int scoreHome, int scoreAway) {
		if(!scoreText)return;
		string h = "";
		string t = "";
		if(scoreHome<10)h+= "0";
		t+= "             ";
		if(scoreAway<10)t+= "0";
		this.scoreText.text = h + scoreHome + t + scoreAway;
	}

	protected virtual void _OnStart() {
	}

	protected virtual void _OnUpdate() {
	}

	protected virtual void _OnGoalScored(Team _scoringTeam) {
	}
	
	protected virtual void _OnPlayerShot() {
	}
	
	protected virtual void _OnPlayerBallPossession() {
	}
	
	protected virtual void _OnPlayerMoved(object[] _TeamAndPlayer) {
	}
	
	protected virtual void _OnPlayerSelected(object[] _TeamAndPlayer) {
	}
	
	protected virtual void _OnPlayerDeselected(object[] _TeamAndPlayer) {
	}
	
	public void Restart() {
		_Restart ();
	}
	
	protected virtual void _Restart() {
	}
}
