﻿using UnityEngine;
using System.Collections;

public class GameMode_RuleSet1 : GameMode {

	void _OnStart() {
		numOfGoalsToWin = 3;
	}
	
	protected override void _OnUpdate () {
		for( int i=0; i<teams.Length; ++i ) {
			if( teams[i].isInPossession ) {
				int index = (i==0)? 1: 0;
				teams[index].EnableAllPlayers();
			}
		}
	}
	
	protected override void _OnPlayerShot () {
		for( int i=0; i<teams.Length; ++i ) {
			if( teams[i].isInPossession ) {
				teams[i].DisableLastPlayerWithBall();
			}
		}
	}
	
	protected override void _OnPlayerBallPossession() {
		for( int i=0; i<teams.Length; ++i ) {
			teams[i].UpdatePossession();
			if( teams[i].isInPossession ) {
				teams[i].DisableAllPlayerMovements();
			}
		}
	}
	
	protected override void _OnGoalScored(Team _scoringTeam) {
		GetOppositeTeam(_scoringTeam).AddScore();
		_scoringTeam.GiveBallToGoalKeeper(_scoringTeam.teamGoal.GetBall);
		UpdateScoreText (teams [0].Score, teams [1].Score);
	}
	
	void ResetPositions () {
	}
}
