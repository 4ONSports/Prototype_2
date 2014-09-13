using UnityEngine;
using System.Collections;

public class GameMode_RuleSet2 : GameMode {

	[SerializeField] private TextMesh timerText = null;
	[SerializeField] private TextMesh winText = null;
	[SerializeField] private int totalTurnTimeSeconds = 10;

	private float currentTime = 0;
	private bool movePhase = true;

	protected override void _OnStart() {
		ResetTimer ();
		numOfGoalsToWin = 3;
		goalCount = new int[(int)TeamSide.COUNT];
		winText.text = "";
	}
	
	protected override void _OnUpdate () {
		if(movePhase) {
			currentTime -= Time.deltaTime;
			timerText.text = (int)currentTime + "";
			if(currentTime < 0){
				DisabelAllPlayers();
				movePhase = false;
			}
		}
	}

	protected override void _OnGoalScored(Team _scoringTeam) {
		ResetPositions ();
		GetOppositeTeam(_scoringTeam).AddScore();
		_scoringTeam.GiveBallToGoalKeeper(_scoringTeam.teamGoal.GetBall);
		UpdateScoreText (teams [0].Score, teams [1].Score);
		if (MaxScoreReached ()) {
			StartCoroutine(LastGoal());
			return;
		}
		SetMovePhaseOn ();
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
			SetMovePhaseOn();
		}
	}
	
	protected override void _OnPlayerMoved (object[] _TeamAndPlayer) {
		teams[(int)_TeamAndPlayer[0]].DisablePlayerMovement((int)_TeamAndPlayer[1]);
	}

	void DisabelAllPlayers() {
		for( int i=0; i<teams.Length; ++i )	teams[i].DisableAllPlayerMovements();
	}

	void EnablePlayers() {
		for( int i=0; i<teams.Length; ++i )	teams[i].EnableAllPlayers();
	}
	
	void ResetPositions () {
		for( int i=0; i<teams.Length; ++i )	teams[i].ResetTeamPosition();
	}

	void SetMovePhaseOn() {
		ResetTimer ();
		EnablePlayers ();
		movePhase = true;
	}

	void ResetTimer() {
		currentTime = totalTurnTimeSeconds;
	}

	void ResetScore() {
		teams [0].Score = 0;
		teams [1].Score = 0;
		UpdateScoreText (teams [0].Score, teams [1].Score);
	}

	bool MaxScoreReached() {
		for (int i=0; i<teams.Length; ++i) {
			if(teams[i].Score >= numOfGoalsToWin)
				return true;
		}
		return false;
	}

	private IEnumerator LastGoal() {
		for (int i=0; i<teams.Length; ++i) {
			if(teams[i].Score >= numOfGoalsToWin) {
				if(teams[i].side == 0) {
					winText.text = "Blue Wins!";
					winText.color = Color.blue;
				} else {
					winText.text = "Red Wins!";
					winText.color = Color.red;
				}
			}
		}
		yield return new WaitForSeconds (2);
		winText.text = "";
		ResetScore ();
		ResetPositions ();
		SetMovePhaseOn ();
	}
}
