using UnityEngine;
using System.Collections;

public class GameMode_RuleSet3 : GameMode {

	[SerializeField] private TextMesh timerText = null;
	[SerializeField] private TextMesh winText = null;
	[SerializeField] private int totalTurnTimeSeconds = 10;
	[SerializeField] private int numberOfMovments = 1;

	private float currentTime = 0;
	private float currentNumberOfMovments = 0;

	private TeamSide teamOfTheTurn = TeamSide.SIDE_AWAY;

	protected override void _OnStart() {
		ResetTimer ();
		numOfGoalsToWin = 3;
		winText.text = "";
		StartCoroutine (LateStart ());
	}

	private IEnumerator LateStart(){
		yield return null;
		NewTurn ();
	}
	
	protected override void _OnUpdate () {
		currentTime -= Time.deltaTime;
		timerText.text = (int)currentTime + "";
		timerText.color = GetTeamColorByTeamSide (teamOfTheTurn);
		if(currentTime < 0){
			NewTurn();
		}
	}

	protected override void _OnGoalScored(Team _scoringTeam) {
		ResetAllPlayersPositions ();
		GetOppositeTeam(_scoringTeam).AddScore();
		_scoringTeam.GiveBallToGoalKeeper(_scoringTeam.teamGoal.GetBall);
		UpdateScoreText (teams [0].Score, teams [1].Score);
		if (MaxScoreReached ()) {
			StartCoroutine(LastGoal());
			return;
		}
		NewTurn ();
	}
	
	protected override void _OnPlayerBallPossession() {
		NewTurn();
	}
	
	protected override void _OnPlayerMoved (object[] _TeamAndPlayer) {
		CheckNewTurn ();
	}

	void DisabelAllPlayers() {
		for( int i=0; i<teams.Length; ++i )	teams[i].DisbaleAllPlayerMovmentsAndBalls();
	}

	void EnableAllPlayers() {
		for( int i=0; i<teams.Length; ++i )	teams[i].EnableAllPlayers();
	}
	
	void ResetAllPlayersPositions () {
		for( int i=0; i<teams.Length; ++i )	teams[i].ResetTeamPosition();
	}

	void ResetTimer() {
		currentTime = totalTurnTimeSeconds;
	}

	void ResetScore() {
		teams [0].Score = 0;
		teams [1].Score = 0;
		UpdateScoreText (teams [0].Score, teams [1].Score);
	}

	void CheckNewTurn() {
		currentNumberOfMovments += 1;
		if(currentNumberOfMovments  >= numberOfMovments) NewTurn();
	}

	void NewTurn() {
		ResetTimer ();
		teamOfTheTurn = GetOppositeTeam (GetTeamByTeamSide (teamOfTheTurn)).side;//Change team of the turn
		GetTeamByTeamSide (teamOfTheTurn).EnableAllPlayers ();//Enable team of the turn
		GetOppositeTeam (GetTeamByTeamSide (teamOfTheTurn)).DisbaleAllPlayerMovmentsAndBalls ();//Disable opposite team
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
		ResetAllPlayersPositions ();
		NewTurn ();
	}
}
