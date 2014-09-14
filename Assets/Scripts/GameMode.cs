﻿using UnityEngine;
using System.Collections;

public class GameMode : MonoBehaviour {
	public int numOfGoalsToWin = 0;
	public int[] goalCount;
	public Team[] teams;

	public TextMesh scoreText = null;


	void Start() {
		_OnStart ();
		GameEvents.SubscribeToEvent (GameEvents.GameEvent.EVT_GOAL_SCORED, this);
		GameEvents.SubscribeToEvent (GameEvents.GameEvent.EVT_PLAYER_SHOOT, this);
		GameEvents.SubscribeToEvent (GameEvents.GameEvent.EVT_PLAYER_POSSESS_BALL, this);
		GameEvents.SubscribeToEvent (GameEvents.GameEvent.EVT_PLAYER_MOVED, this);
	}
	
	void Update () {
		_OnUpdate ();
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
		t+= "  ";
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
}
