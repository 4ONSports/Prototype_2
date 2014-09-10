using UnityEngine;
using System.Collections;

public class GameMode_RuleSet2 : GameMode {

	protected override void _OnStart() {
		numOfGoalsToWin = 3;
		goalCount = new int[(int)TeamSide.COUNT];
	}
	
	protected override void _OnUpdate () {
//		for( int i=0; i<teams.Length; ++i ) {
//			if( teams[i].isInPossession ) {
//				int index = (i==0)? 1: 0;
//				teams[index].EnableAllPlayers();
//			}
//		}
	}

	protected override void _OnGoalScored(Team _scoringTeam) {
		GetOppositeTeam(_scoringTeam).AddScore();
		_scoringTeam.GiveBallToGoalKeeper(_scoringTeam.teamGoal.GetBall);
		UpdateScoreText (teams [0].Score, teams [1].Score);
	}
	
	protected override void _OnPlayerShot () {
		for( int i=0; i<teams.Length; ++i ) {
			if( teams[i].isInPossession ) {
				teams[i].DisableLastPlayerWithBall();
			}
		}
	}
	
	protected override void _OnPlayerBallPossession() {
		int prevTeamInPossession = (teams[0].isInPossession)? 0:
									(teams[1].isInPossession)? 1: -1;
		int newTeamInPossession = -1;
		for( int i=0; i<teams.Length; ++i ) {
			teams[i].UpdatePossession();

			if( teams[i].isInPossession ) {
				newTeamInPossession = i;
			}
		}

		// if there is a change in possession, enable all players
		if ( prevTeamInPossession>=0 && newTeamInPossession>=0 && prevTeamInPossession!=newTeamInPossession ) {
			for( int i=0; i<teams.Length; ++i ) {
				teams[i].EnableAllPlayers();
			}
		}
	}
	
	protected override void _OnPlayerMoved (object[] _TeamAndPlayer) {
		teams[(int)_TeamAndPlayer[0]].DisablePlayerMovement((int)_TeamAndPlayer[1]);
	}
	
	void ResetPositions () {
	}
}
