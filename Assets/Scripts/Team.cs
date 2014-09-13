using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TeamSide {
	INVALID = -1,
	SIDE_HOME, //0 is always home, blue
	SIDE_AWAY, //1 is always away, red
	COUNT
}

public class Team : MonoBehaviour {

	public TeamSide side = TeamSide.SIDE_HOME;
	public GameObject[] players;
	public bool isInPossession = false;
	[SerializeField] private int goalKeeperIndex = 0;
	public Goal teamGoal = null;

	private PlayerControl_Ball[] pc_balls;
	private PlayerControl_Movement[] pc_mvmnts;
	private int lastPlayerWithBallIndex = -1;
	private int score = 0;
	private GameObject goalKeeper;
	private List<Vector3> teamStartingPosition = new List<Vector3> ();

	public int Score
	{
		get{return score;}
		set{score = value;}
	}

	void Start () {
		pc_balls = new PlayerControl_Ball[players.Length];
		pc_mvmnts = new PlayerControl_Movement[players.Length];
		for( int i=0; i<players.Length; ++i ) {
			pc_balls[i] = players[i].GetComponent<PlayerControl_Ball>();
			pc_mvmnts[i] = players[i].GetComponent<PlayerControl_Movement>();
			teamStartingPosition.Add (players[i].transform.position);
			pc_mvmnts[i].playerTeamSide = side;
			pc_mvmnts[i].playerIndexPosOnTeam = i;
		}
		goalKeeper = players [goalKeeperIndex];
		teamGoal.AssignTeam (this);
	}
	
	public void EnableAllPlayers () {
		for( int i=0; i<players.Length; ++i ) {
			pc_balls[i].disable = false;
			pc_mvmnts[i].disable = false;

			Color col = pc_mvmnts[i].renderer.material.color;
			col.a = (float)126/255;
			pc_mvmnts[i].renderer.material.color = col;
		}
	}
	
	public void DisableLastPlayerWithBall () {
		if( lastPlayerWithBallIndex >= 0 ) {
			pc_balls[lastPlayerWithBallIndex].disable = true;
			pc_mvmnts[lastPlayerWithBallIndex].disable = true;

			Color col = pc_mvmnts[lastPlayerWithBallIndex].renderer.material.color;
			col.a = (float)20/255;
			pc_mvmnts[lastPlayerWithBallIndex].renderer.material.color = col;
		}
	}
	
	public void UpdatePossession () {
		isInPossession = false;
		for( int i=0; i<players.Length; ++i ) {
			if( pc_balls[i].hasABall ) {
				isInPossession = true;
				lastPlayerWithBallIndex = i;
			}
		}
	}
	
	public void DisableAllPlayerMovements () {
		for( int i=0; i<players.Length; ++i ) {
			pc_mvmnts[i].disable = true;
		}
	}
	
	public void DisablePlayerMovement ( int index ) {
		pc_mvmnts[index].disable = true;
	}

	public void AddScore()  {
		score += 1;
	}

	public void GiveBallToGoalKeeper(GameObject _ball) {
		EnableAllPlayers ();
		_ball.rigidbody2D.velocity *= 0;
		_ball.transform.position = goalKeeper.transform.position;
	}
	
	public PlayerControl_Movement GetPlayerMvmntByName( string name )  {
		PlayerControl_Movement pc = null;
		for (int i=0; i<players.Length; ++i) {
			if( players[i].name == name ) {
				pc = pc_mvmnts[i];
			}
		}
		return pc;
	}
	
	public int GetPlayerIndexFromName( string name )  {
		int index = -1;
		for (int i=0; i<players.Length; ++i) {
			if( players[i].name == name ) {
				index = i;
			}
		}
		return index;
	}

	public void ResetTeamPosition () {
		for (int i=0; i<players.Length; ++i) {
			players[i].transform.position = teamStartingPosition[i];
		}
	}
}
