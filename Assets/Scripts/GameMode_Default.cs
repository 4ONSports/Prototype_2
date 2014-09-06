using UnityEngine;
using System.Collections;

public class GameMode_Default : GameMode {

//	void Start() {
//		numOfGoalsToWin = 3;
//		goalCount = new int[(int)Team.TeamSide.COUNT];
//	}
	
	protected override void _OnUpdate () {
//		for( int i=0; i<teams.Length; ++i ) {
//			if( teams[i].isInPossession ) {
//				int index = (i==0)? 1: 0;
//				teams[index].EnableAllPlayers();
//			}
//		}
	}
	
	protected override void _OnPlayerShoot () {
//		// Disable player until other team has possession of ball
//		for( int i=0; i<teams.Length; ++i ) {
//			if( teams[i].isInPossession ) {
//				teams[i].DisableLastPlayerWithBall();
//			}
//		}
	}
	
	protected override void _OnPlayerBallPossession() {
//		for( int i=0; i<teams.Length; ++i ) {
//			teams[i].UpdatePossession();
//		}
	}
	
	protected override void _OnGoalScored () {
	}
	
	void ResetPositions () {
	}
}
