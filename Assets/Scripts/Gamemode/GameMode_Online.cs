using UnityEngine;
using System.Collections;

public class GameMode_Online : GameMode {
	
	[SerializeField] private TextMesh timerText = null;
	[SerializeField] private TextMesh winText = null;
	[SerializeField] private int totalTurnTimeSeconds = 10;
	[SerializeField] private int numberOfMovments = 1;
	
	private float currentTime = 0;
	private float currentNumberOfMovments = 0;
	private bool playerMoving = false;
	private int selectedPlayerTeam = -1;
	private int selectedPlayerIndex = -1;
	private bool cancelMove = false;
	
	private TeamSide teamOfTheTurn = TeamSide.SIDE_AWAY;
	private TeamSide localTeamSide = TeamSide.SIDE_HOME;
	
	protected override void _OnStart() {
		ResetTimer ();
		winText.text = "";

		OnlineGameEventsHandler.SubscribeToEvent (EOnlineGameEvent.ONL_EVT_START_MATCH, this);
		OnlineGameEventsHandler.SubscribeToEvent (EOnlineGameEvent.ONL_EVT_SWITCH_TURN, this);
		OnlineGameEventsHandler.SubscribeToEvent (EOnlineGameEvent.ONL_EVT_PLAYER_MOVED, this);
		StartCoroutine (LateStart ());
	}
	
	private IEnumerator LateStart(){
		yield return null;
		
		GetTeamByTeamSide (localTeamSide).DisbaleAllPlayerMovmentsAndBalls ();
		GetTeam_Opponent (localTeamSide).DisbaleAllPlayerMovmentsAndBalls (); // Disable opponent's team
	}

	public void HandleOnlineEvent(OnlineGameEvent _event) {
		switch(_event.gameEvent) {
		case EOnlineGameEvent.ONL_EVT_START_MATCH:
			Debug.Log ("Start Match");
			_Restart();
			break;
		case EOnlineGameEvent.ONL_EVT_SWITCH_TURN:
			Debug.Log ("Switch Match");
			break;
		case EOnlineGameEvent.ONL_EVT_PLAYER_MOVED:
			Debug.Log ("Player Moved");
			break;
		}
	}
	
	protected override void _OnUpdate () {
//		if( teamOfTheTurn != localTeamSide ) {
//			return;
//		}

		if( playerMoving ) {
			CheckValidPlayerMovement();
		}
		
		if( !gameOver && currentTime>=0 ) {
			currentTime -= Time.deltaTime;
			timerText.text = (int)currentTime + "";
			timerText.color = GetTeamColorByTeamSide (teamOfTheTurn);
			if(currentTime < 0 && teamOfTheTurn == localTeamSide){
				SwitchTurns();
			}
		}
	}
	
	protected override void _OnPlayerSelected (object[] _TeamAndPlayer) {
		if( teamOfTheTurn != localTeamSide ) {
			return;
		}

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
		if( teamOfTheTurn != localTeamSide ) {
			return;
		}

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
		ResetAllPlayersPositions ();
		GetTeam_Opponent(_scoringTeam.side).AddScore();
		_scoringTeam.GiveBallToGoalKeeper(_scoringTeam.teamGoal.GetBall);
		UpdateScoreText (teams [0].Score, teams [1].Score);
		if (MaxScoreReached ()) {
			StartCoroutine(LastGoal());
			return;
		}
		SwitchTurns ();
	}
	
	protected override void _OnPlayerShot () {
		for( int i=0; i<teams.Length; ++i ) {
			if( teams[i].isInPossession ) {
				teams[i].DisableLastPlayerWithBall();
			}
		}
	}
	
	protected override void _OnPlayerBallPossession() {
		SwitchTurns();
	}
	
	protected override void _OnPlayerMoved (object[] _TeamAndPlayer) {
		CheckNewTurn ();
	}
	
	void DisabelAllPlayers() {
		for( int i=0; i<teams.Length; ++i )	{
			teams[i].DisbaleAllPlayerMovmentsAndBalls();
		}
	}
	
	void EnableAllPlayers() {
		for( int i=0; i<teams.Length; ++i )	{
			teams[i].EnableAllPlayers();
		}
	}
	
	void ResetAllPlayersPositions () {
		for( int i=0; i<teams.Length; ++i ) {
			teams[i].ResetTeamPosition();
		}
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
		if(currentNumberOfMovments  >= numberOfMovments) {
			SwitchTurns();
		}
	}
	
	void SwitchTurns() {
		ResetTimer ();
		teamOfTheTurn = GetTeam_Opponent (teamOfTheTurn).side;//Change team of the turn

		if( teamOfTheTurn == localTeamSide ) {
			GetTeamByTeamSide (localTeamSide).EnableAllPlayers ();
		}
		else {
			GetTeamByTeamSide (localTeamSide).DisbaleAllPlayerMovmentsAndBalls ();
		}
	}
	
	bool MaxScoreReached() {
		for (int i=0; i<teams.Length; ++i) {
			if(teams[i].Score >= numOfGoalsToWin) {
				return true;
			}
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
		gameOver = true;
		yield return new WaitForSeconds (2);
		//		winText.text = "";
		//		ResetScore ();
		//		ResetAllPlayersPositions ();
		//		NewTurn ();
	}
	
	protected override void _Restart() {
		ResetTimer ();
		ResetScore ();
		gameOver = false;
		winText.text = "";
	}
}
