using UnityEngine;
using System.Collections;

public class GameMode_RuleSet2 : GameMode {

	[SerializeField] private TextMesh timerText = null;
	[SerializeField] private TextMesh winText = null;
	[SerializeField] private int totalTurnTimeSeconds = 10;

	private float currentTime = 0;
	private bool movePhase = true;
	private bool playerMoving = false;
	private int selectedPlayerTeam = -1;
	private int selectedPlayerIndex = -1;
	private bool cancelMove = false;

	protected override void _OnStart() {
		ResetTimer ();
		numOfGoalsToWin = 3;
		winText.text = "";
	}
	
	protected override void _OnUpdate () {
		if( playerMoving ) {
			CheckValidPlayerMovement();
		}

		if(movePhase) {
			currentTime -= Time.deltaTime;
			timerText.text = (int)currentTime + "";
			if(currentTime < 0){
				DisabelAllPlayers();
				movePhase = false;
			}
		}
	}
	
	protected override void _OnPlayerSelected (object[] _TeamAndPlayer) {
		if( (int)_TeamAndPlayer[0] >= 0 && (int)_TeamAndPlayer[1] >= 0 ) {
			playerMoving = true;
			selectedPlayerTeam = (int)_TeamAndPlayer[0];
			selectedPlayerIndex = (int)_TeamAndPlayer[1];
			PlayerControl_Movement plyr = teams[selectedPlayerTeam].players[selectedPlayerIndex].GetComponent<PlayerControl_Movement>();
			if( plyr.disable ) {
				playerMoving = false;
				selectedPlayerTeam = -1;
				selectedPlayerIndex = -1;
			}
		}
		// else assert error
	}
	
	protected override void _OnPlayerDeselected (object[] _TeamAndPlayer) {
		if( (int)_TeamAndPlayer[0] == selectedPlayerTeam && (int)_TeamAndPlayer[1] == selectedPlayerIndex ) {
			if( cancelMove ) {
//				Debug.Log ("Cannot Move here");
//				PlayerControl_Movement plyr = teams[selectedPlayerTeam].players[selectedPlayerIndex].GetComponent<PlayerControl_Movement>();
//				cancelMove = false;
//				plyr.MoveToPrevPos();
			}
			playerMoving = false;
			selectedPlayerTeam = -1;
			selectedPlayerIndex = -1;
		}
		// else assert error
	}
	
	void CheckValidPlayerMovement () {
		bool validPosition = true;

		PlayerControl_Movement plyr = teams[selectedPlayerTeam].players[selectedPlayerIndex].GetComponent<PlayerControl_Movement>();
		for( int i=0; i<teams[selectedPlayerTeam].players.Length; ++i ) {
			if( selectedPlayerIndex != i ) {
				PlayerControl_Movement teamMate = teams[selectedPlayerTeam].players[i].GetComponent<PlayerControl_Movement>();
				float distBtwPlayers = (plyr.transform.position-teamMate.transform.position).magnitude;
				if( distBtwPlayers < plyr.minDistBtwTeamPlayers ) {
					validPosition = false;
//					plyr.movementLimitObj.renderer.enabled = false;
				}
			}
		}

		if( !validPosition ) {
			cancelMove = true;
			plyr.movementLimitObj.renderer.material.color = new Color(1,1,0,0.17f);
			plyr.cancelPlayerMove = true;
		}
		else {
			cancelMove = false;
			plyr.movementLimitObj.renderer.material.color = new Color(1,1,1,0.17f);
			plyr.cancelPlayerMove = false;
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
